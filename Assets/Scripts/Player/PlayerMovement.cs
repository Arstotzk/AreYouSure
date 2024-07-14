using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private GameControl gameControl;
    private CharacterController characterController;
    private Camera camera;

    public float speed = 4f;
    public float sensitivity = 10f;
    public float gravity = 9.8f;

    float rotationY = 0f;
    void Start()
    {
        characterController = this.GetComponent<CharacterController>();
        gameControl = new GameControl();
        gameControl.Enable();
        camera = this.GetComponentInChildren<Camera>();
    }

    void Update()
    {
        var direction = gameControl.Gameplay.Movement.ReadValue<Vector2>() * speed * Time.deltaTime;
        var look = gameControl.Gameplay.Look.ReadValue<Vector2>() * sensitivity * Time.deltaTime;
        //var mouse = gameControl.Gameplay.
        Debug.Log("mouseCamera:" + look);
        this.transform.Rotate(Vector3.up, look.x);
        rotationY -= look.y;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);
        camera.transform.localRotation = Quaternion.Euler(rotationY, 0f, 0f);
        //camera.transform.Rotate(Vector3.left, rotationY);

        Vector3 movement = transform.right * direction.x + transform.forward * direction.y;
        characterController.Move(movement);
        Vector3 gravityMove = -transform.up * Time.deltaTime * gravity;
        characterController.Move(gravityMove);
    }
}
