using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MACameraController : MonoBehaviour
{
    [Range(0, 10f)]
    public float movementSpeed;

    [Range(0, 10f)]
    public float maxMovementSpeed;

    [Range(0, 360)]
    public float rotationSpeed;

    [Range(0, 50)]
    public float jumpForce;

    [Range(0, 360)]
    public float maxRotationAngle;

    public Rigidbody rb;


    public GameObject xRotator;
    public GameObject yRotator;

    public float xRotation = 0;
    public float yRotation = 0;

    bool isGrounded = false;

    void Start()
    {
        this.xRotation = Input.GetAxis("Mouse X") * this.rotationSpeed;
        this.yRotation = Input.GetAxis("Mouse Y") * this.rotationSpeed;
    }


    void Update()
    {

        this.CalculateMovement();
        this.CalculateRotation();
    }

    private void OnCollisionStay(Collision collision)
    {
        this.isGrounded = true;
    }

    private void CalculateMovement()
    {
        AccelerateXZ();

        LimitSpeed();

        ManageJump();
    }

    private void AccelerateXZ()
    {
        //Accelerate Player due to Inputs
        if (Input.GetKey("w"))
        {
            Vector3 forward = (this.xRotator.transform.rotation * Vector3.forward);
            forward = Vector3.ProjectOnPlane(forward, Vector3.up).normalized;

            //this.transform.Translate(forward * this.movementSpeed);
            this.rb.AddForce(forward * this.movementSpeed, ForceMode.Acceleration);
        }

        if (Input.GetKey("a"))
        {
            Vector3 left = this.xRotator.transform.rotation * Vector3.left;
            left = Vector3.ProjectOnPlane(left, Vector3.up).normalized;

            // this.transform.Translate(left * this.movementSpeed);
            this.rb.AddForce(left * this.movementSpeed, ForceMode.Acceleration);
        }

        if (Input.GetKey("s"))
        {
            Vector3 back = this.xRotator.transform.rotation * Vector3.back;
            back = Vector3.ProjectOnPlane(back, Vector3.up).normalized;

            //this.transform.Translate(back * this.movementSpeed);
            this.rb.AddForce(back * this.movementSpeed, ForceMode.Acceleration);
        }

        if (Input.GetKey("d"))
        {
            Vector3 right = this.xRotator.transform.rotation * Vector3.right;
            right = Vector3.ProjectOnPlane(right, Vector3.up).normalized;

            //this.transform.Translate(right * this.movementSpeed);
            this.rb.AddForce(right * this.movementSpeed, ForceMode.Acceleration);
        }
    }

    private void LimitSpeed()
    {
        //Limit the Player Speed because without it acceleration would result in infinite speed!
        Vector3 velocityXZ = Vector3.ProjectOnPlane(this.rb.velocity, Vector3.up);

        if (velocityXZ.magnitude > this.maxMovementSpeed)
        {

            Vector3 newVelocityXZ = velocityXZ.normalized * this.maxMovementSpeed;

            this.rb.velocity = new Vector3(newVelocityXZ.x, this.rb.velocity.y, newVelocityXZ.z);

        }
    }

    private void ManageJump()
    {
        //manageJump
        if (Input.GetKeyDown("space") && this.isGrounded)
        {
            Vector3 force = Vector3.up * this.jumpForce;

            rb.AddForce(force, ForceMode.Impulse);

            this.transform.Translate(Vector3.up * 0.1f);
            this.isGrounded = false;
        }
    }

    private void CalculateRotation()
    {

        if (this.xRotation <= this.maxRotationAngle)
        {
            this.xRotation -= Input.GetAxis("Mouse Y") * this.rotationSpeed;
        }
        else
        {
            this.xRotation = this.maxRotationAngle;
        }

        if (this.xRotation >= -this.maxRotationAngle)
        {
            this.xRotation -= Input.GetAxis("Mouse Y") * this.rotationSpeed;
        }
        else
        {
            this.xRotation = -this.maxRotationAngle;
        }

        this.yRotation += Input.GetAxis("Mouse X") * this.rotationSpeed;

        this.xRotator.transform.SetPositionAndRotation(this.xRotator.transform.position, Quaternion.Euler(this.xRotation, this.xRotator.transform.eulerAngles.y, this.xRotator.transform.eulerAngles.z));

        this.yRotator.transform.SetPositionAndRotation(this.yRotator.transform.position, Quaternion.Euler(this.yRotator.transform.eulerAngles.x, this.yRotation, this.yRotator.transform.eulerAngles.z));

    }
}
