using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private GameControl gameControl;
    private CharacterController characterController;
    private Camera camera;
    private InteractEnvironment interactEnvironment;
    private GameObject lookAtTarget;
    private Dialog _dialog;

    public float speed = 4f;
    public float sensitivity = 10f;
    public float gravity = 9.8f;
    public float lookAtSpeed = 8f;

    float rotationY = 0f;
    void Start()
    {
        characterController = this.GetComponent<CharacterController>();
        interactEnvironment = this.GetComponent<InteractEnvironment>();
        camera = this.GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (gameControl.Gameplay.enabled)
        {
            var direction = gameControl.Gameplay.Movement.ReadValue<Vector2>() * speed * Time.deltaTime;
            var look = gameControl.Gameplay.Look.ReadValue<Vector2>() * sensitivity * Time.deltaTime;
            this.transform.Rotate(Vector3.up, look.x);
            rotationY -= look.y;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f);
            camera.transform.localRotation = Quaternion.Euler(rotationY, 0f, 0f);

            Vector3 movement = transform.right * direction.x + transform.forward * direction.y;
            characterController.Move(movement);
            Vector3 gravityMove = -transform.up * Time.deltaTime * gravity;
            characterController.Move(gravityMove);
        }

        //TODO Перенести в отдельный класс
        if (gameControl.Dialog.enabled)
        {
            Vector3 lookDirection = lookAtTarget.transform.position - camera.transform.position;
            lookDirection.Normalize();

            camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, Quaternion.LookRotation(lookDirection), lookAtSpeed * Time.deltaTime);
        }
    }
    public void Awake()
    {
        gameControl = new GameControl();
        gameControl.Enable();
        gameControl.Dialog.Disable();
    }
    public void OnEnable()
    {
        gameControl.Gameplay.Interact.performed += Interact;
        gameControl.Dialog.Next.performed += DialogNext;
    }
    public void OnDisable()
    {
        gameControl.Gameplay.Interact.performed -= Interact;
        gameControl.Dialog.Next.performed -= DialogNext;
    }
    private void Interact(InputAction.CallbackContext ctx)
    {
        interactEnvironment.Interact(this.gameObject);
    }

    private void DialogNext(InputAction.CallbackContext ctx) 
    {
        //Debug.Log("DiallogNext");
        _dialog.Interact(this.gameObject);
    }

    public void OnDialog(GameObject focusPoint, Dialog dialog) 
    {
        _dialog = dialog;
        gameControl.Gameplay.Disable();
        gameControl.Dialog.Enable();
        lookAtTarget = focusPoint;
    }
    public void ExitDialog()
    {
        gameControl.Gameplay.Enable();
        gameControl.Dialog.Disable();
    }
}
