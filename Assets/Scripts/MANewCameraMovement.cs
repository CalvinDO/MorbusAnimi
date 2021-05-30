using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MANewCameraMovement : MonoBehaviour
{

    public float mouseSensitivity = 100f;

    public Camera mainCamera;
    public Transform playerBody;

    private Vector3 defaultCameraPosition;

    float xRotation = 11f;
    float yRotation = 270f;

    // Start is called before the first frame update
    void Start()
    {
        defaultCameraPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        this.mainCamera.transform.position = Vector3.Lerp(this.mainCamera.transform.position, this.transform.position, 0.01f);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -7f, 7f);
        yRotation -= mouseX;
        yRotation = Mathf.Clamp(yRotation, -7f, 7f);

        this.mainCamera.transform.localRotation = Quaternion.Euler(11f + xRotation, 270f - yRotation, 0f);
    }
}
