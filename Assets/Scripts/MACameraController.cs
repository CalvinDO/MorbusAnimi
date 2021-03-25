using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MACameraController : MonoBehaviour {
    [Range(0, 50f)]
    public float movementAcceleration;

    [Range(0, 10f)]
    public float maxMovementSpeed;

    [Range(0, 15)]
    public float mouseSensitivity;

    [Range(0, 1000)]
    public float jumpForce;

    [Range(0, 90)]
    public float maxRotationAngle;

    [Range(0, 0.2f)]
    public float slowDownFactor;

    public Rigidbody rb;


    public GameObject xRotator;
    public GameObject yRotator;

    public float xRotation = 0;
    public float yRotation = 0;




    bool isGrounded = false;

    void Start() {
        this.xRotation = Input.GetAxis("Mouse X") * this.mouseSensitivity;
        this.yRotation = Input.GetAxis("Mouse Y") * this.mouseSensitivity;
    }


    void Update() {

        this.CalculateMovement();
        this.CalculateRotation();
    }

    private void OnCollisionStay(Collision collision) {
        this.isGrounded = true;
    }

    private void CalculateMovement() {
        this.AccelerateXZ();

        this.LimitSpeed();

        this.SlowDown();

        this.ManageJump();
    }

    private void AccelerateXZ() {

        //Accelerate Player due to Inputs

        Vector3 resultingVector = Vector3.zero;

        if (Input.GetKey("w")) {
            Vector3 forward = GetVectorInDirection(Vector3.forward);

            resultingVector += forward;
        }

        if (Input.GetKey("a")) {
            Vector3 left = GetVectorInDirection(Vector3.left);

            resultingVector += left;
        }

        if (Input.GetKey("s")) {
            Vector3 back = GetVectorInDirection(Vector3.back);

            resultingVector += back;
        }

        if (Input.GetKey("d")) {
            Vector3 right = GetVectorInDirection(Vector3.right);

            resultingVector += right;
        }

        Vector3 normalizedSum = resultingVector.normalized;
        Vector3 scaledNormalizedResult = normalizedSum * this.movementAcceleration;
        this.rb.AddForce(scaledNormalizedResult, ForceMode.Acceleration);
    }

    private Vector3 GetVectorInDirection(Vector3 direction) {

        Vector3 rotated = this.xRotator.transform.rotation * direction;
        return Vector3.ProjectOnPlane(rotated, Vector3.up).normalized;
    }

    private void LimitSpeed() {

        //Limit the Player Speed because without it acceleration would result in infinite speed!
        Vector3 velocityXZ = Vector3.ProjectOnPlane(this.rb.velocity, Vector3.up);

        if (velocityXZ.magnitude > this.maxMovementSpeed) {

            Vector3 newVelocityXZ = velocityXZ.normalized * this.maxMovementSpeed;

            this.rb.velocity = new Vector3(newVelocityXZ.x, this.rb.velocity.y, newVelocityXZ.z);
        }
    }

    private void SlowDown() {

        if (!(Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) && this.isGrounded) {
            Vector3 velocity = this.rb.velocity;

            velocity.x *= (1 - this.slowDownFactor);
            velocity.z *= (1 - this.slowDownFactor);

            this.rb.velocity = velocity;
        }
    }

    private void ManageJump() {
        //manageJump
        if (Input.GetKeyDown("space") && this.isGrounded && this.rb.velocity.y <= 0) {
            Vector3 force = Vector3.up * this.jumpForce;

            rb.AddForce(force, ForceMode.Impulse);

            this.transform.Translate(Vector3.up * 0.01f);
            this.isGrounded = false;
        }
    }

    private void CalculateRotation() {

        if (this.xRotation <= this.maxRotationAngle) {
            this.xRotation -= Input.GetAxis("Mouse Y") * this.mouseSensitivity;
        }
        else {
            this.xRotation = this.maxRotationAngle;
        }

        if (this.xRotation >= -this.maxRotationAngle) {
            this.xRotation -= Input.GetAxis("Mouse Y") * this.mouseSensitivity;
        }
        else {
            this.xRotation = -this.maxRotationAngle;
        }

        this.yRotation += Input.GetAxis("Mouse X") * this.mouseSensitivity;

        this.xRotator.transform.SetPositionAndRotation(this.xRotator.transform.position, Quaternion.Euler(this.xRotation, this.xRotator.transform.eulerAngles.y, this.xRotator.transform.eulerAngles.z));

        this.yRotator.transform.SetPositionAndRotation(this.yRotator.transform.position, Quaternion.Euler(this.yRotator.transform.eulerAngles.x, this.yRotation, this.yRotator.transform.eulerAngles.z));
    }
}
