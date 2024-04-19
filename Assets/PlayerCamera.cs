using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerBody;
    public Vector2 mouseSensitivity;


    // Update is called once per frame
    void Update()
    {
        Vector3 verticalInput = new Vector3(-Input.GetAxis("Mouse Y"), 0, 0);
        Vector3 horizontalInput = new Vector3(0, Input.GetAxis("Mouse X"), 0);
        playerBody.Rotate(horizontalInput * mouseSensitivity.x);


        float newVerticalRotation = transform.rotation.x + verticalInput.x * mouseSensitivity.y;
        Debug.Log(newVerticalRotation + " " + transform.localRotation.x);
        if (newVerticalRotation < 90 && newVerticalRotation > -90) transform.Rotate(verticalInput * mouseSensitivity.y);
    }
}
