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
    private PlayerSounds playerSounds;

    public float speed = 4f;
    public float sensitivity = 10f;
    public float gravity = 9.8f;
    public float lookAtSpeed = 8f;
    public Animator itemAnimator;

    public GameObject menuUI;

    float rotationY = 0f;
    void Start()
    {
        characterController = this.GetComponent<CharacterController>();
        interactEnvironment = this.GetComponent<InteractEnvironment>();
        playerSounds = GetComponent<PlayerSounds>();
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

            //Debug.Log("direction.x: " + direction.x + " direction.y: " + direction.y);
            if (direction.x > 0.0001f || direction.y > 0.0001f)
            {
                playerSounds.TryPlayMoveSound();
                itemAnimator.SetBool("IsWalk", true);
            }
            else
            {
                itemAnimator.SetBool("IsWalk", false);
            }

            Vector3 movement = transform.right * direction.x + transform.forward * direction.y;
            characterController.Move(movement);
            Vector3 gravityMove = -transform.up * Time.deltaTime * gravity;
            characterController.Move(gravityMove);
        }

        //TODO ��������� � ��������� �����
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void OnEnable()
    {
        gameControl.Gameplay.Interact.performed += Interact;
        gameControl.Gameplay.Drop.performed += DropItem;
        gameControl.Gameplay.Menu.performed += ShowMenu;

        gameControl.Dialog.Yes.performed += DialogYes;
        gameControl.Dialog.No.performed += DialogNo;
        gameControl.Dialog.Next.performed += DialogNext;

        gameControl.Menu.UnshowMenu.performed += UnshowMenu;
    }
    public void OnDisable()
    {
        gameControl.Gameplay.Interact.performed -= Interact;
        gameControl.Gameplay.Drop.performed -= DropItem;
        gameControl.Gameplay.Menu.performed -= ShowMenu;

        gameControl.Dialog.Yes.performed -= DialogYes;
        gameControl.Dialog.No.performed -= DialogNo;
        gameControl.Dialog.Next.performed -= DialogNext;

        gameControl.Menu.UnshowMenu.performed -= UnshowMenu;
    }
    private void Interact(InputAction.CallbackContext ctx)
    {
        interactEnvironment.Interact(this.gameObject);
    }
    private void DropItem(InputAction.CallbackContext ctx)
    {
        interactEnvironment.Drop();
    }
    private void DialogYes(InputAction.CallbackContext ctx)
    {
        _dialog.InteractSure(this.gameObject, true);
    }
    private void DialogNo(InputAction.CallbackContext ctx)
    {
        _dialog.InteractSure(this.gameObject, false);
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void ExitDialog()
    {
        gameControl.Gameplay.Enable();
        gameControl.Dialog.Disable();
    }
    public void DisableCharacterController() 
    {
        characterController.enabled = false;
    }
    private void ShowMenu(InputAction.CallbackContext ctx) 
    {
        menuUI.SetActive(true);
        gameControl.Gameplay.Disable();
        gameControl.Menu.Enable();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    private void UnshowMenu(InputAction.CallbackContext ctx)
    {
        menuUI.SetActive(false);
        gameControl.Gameplay.Enable();
        gameControl.Menu.Disable();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void DisableGameplay() 
    {
        gameControl.Gameplay.Disable();
        gameControl.Block.Enable();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
