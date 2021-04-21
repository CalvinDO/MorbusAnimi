using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAMovingEye : MonoBehaviour

{
    float mouseSensitivity = 200f;

    public Transform eyeBody;

    float xRotation = 0f;

    float yRotation = 0f;

    float maxRotationX = 30f;

    float maxRotationY = 10f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = (Input.mousePosition.x - (Screen.width / 2)) / (Screen.width / 2);
        float mouseY = (Input.mousePosition.y - (Screen.height / 2)) / (Screen.width / 2);

        xRotation = Mathf.Clamp(maxRotationX * mouseY, -maxRotationX, maxRotationX);

        yRotation = Mathf.Clamp((-maxRotationY * mouseX), -maxRotationY, maxRotationY);

        transform.localRotation = Quaternion.Euler(180 + xRotation, yRotation, 0f);
        eyeBody.Rotate(Vector3.up * mouseX + Vector3.left * mouseY);
    }
}
