using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MACameraController : MonoBehaviour {
    [Range(0, 50f)]
    public float movementAcceleration;

    [Range(0, 10f)]
    public float maxMovementSpeed;

    [Range(0, 10f)]
    public float maxMovementSprintSpeed;

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

    float xRotationAmount = 0;
    float yRotationAmount = 0;


    float xRotationClamp = 0;


    public Camera camera;

    bool isGrounded = true;

    bool sprinting = false;
    float timeSinceSprintStarted = 0;
    public float SprintFOVLerpFactor;

    bool movementEnabled = true;
    int framesTillStart = 0;

    int framesTillJump = 0;
    bool inJump = false;

    void Start() {

        //this.xRotationClamp = Input.GetAxis("Mouse X") * this.mouseSensitivity;
        //this.yRotationClamp = Input.GetAxis("Mouse Y") * this.mouseSensitivity;

        this.xRotationClamp = 0;
        this.xRotationAmount = 0;

        Cursor.lockState = CursorLockMode.Locked;

    }


    void Update() {

        this.CalculateMovement();
        if (this.framesTillStart > 1) {
            this.CalculateRotation();
        }

        if (this.inJump) {
            this.framesTillJump++;
        }

        this.framesTillStart++;
    }

    private void OnCollisionStay(Collision collision) {
        if (this.framesTillJump > 10) {
            this.isGrounded = true;
            this.inJump = false;
            this.framesTillJump = 0;
        }
    }



    private void CalculateMovement() {
        this.AccelerateXZ();

        this.LimitSpeed();

        this.SlowDown();

        this.ManageJump();
    }


    private void AccelerateXZ() {

        if (!this.movementEnabled) {
            return;
        }

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

        ManageSprinting();

        Vector3 normalizedSum = resultingVector.normalized;

        Vector3 scaledNormalizedResult = normalizedSum * this.movementAcceleration;

        this.rb.AddForce(scaledNormalizedResult, ForceMode.Acceleration);
    }

    private void ManageSprinting() {
        if (Input.GetKey("left shift")) {
            this.sprinting = true;
            this.camera.fieldOfView = Mathf.Lerp(70, 85, this.timeSinceSprintStarted);

            this.timeSinceSprintStarted += Time.deltaTime * this.SprintFOVLerpFactor;
        }
        else {
            this.sprinting = false;
            this.camera.fieldOfView = Mathf.Lerp(70, 85, this.timeSinceSprintStarted);


            this.timeSinceSprintStarted -= Time.deltaTime * this.SprintFOVLerpFactor;

        }

        this.timeSinceSprintStarted = Mathf.Clamp(this.timeSinceSprintStarted, 0, 1);
    }

    private Vector3 GetVectorInDirection(Vector3 direction) {

        Vector3 rotated = this.xRotator.transform.rotation * direction;
        return Vector3.ProjectOnPlane(rotated, Vector3.up).normalized;
    }


    private void LimitSpeed() {

        //Limit the Player Speed because without it acceleration would result in infinite speed!
        Vector3 velocityXZ = Vector3.ProjectOnPlane(this.rb.velocity, Vector3.up);

        switch (this.sprinting) {
            case true:
                if (velocityXZ.magnitude > this.maxMovementSprintSpeed) {

                    Vector3 newVelocityXZ = velocityXZ.normalized * this.maxMovementSpeed;

                    this.rb.velocity = new Vector3(newVelocityXZ.x, this.rb.velocity.y, newVelocityXZ.z);
                }
                break;
            case false:
                if (velocityXZ.magnitude > this.maxMovementSpeed) {

                    Vector3 newVelocityXZ = velocityXZ.normalized * this.maxMovementSpeed;

                    this.rb.velocity = new Vector3(newVelocityXZ.x, this.rb.velocity.y, newVelocityXZ.z);
                }
                break;
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
        if (Input.GetKeyDown("space") && this.isGrounded) {
            Vector3 force = Vector3.up * this.jumpForce;

            rb.AddForce(force, ForceMode.Impulse);

            this.transform.Translate(Vector3.up * 0.01f);
            this.isGrounded = false;
            this.inJump = true;
        }
    }


    private void CalculateRotation() {
        this.xRotationAmount = -Input.GetAxis("Mouse Y") * this.mouseSensitivity;
        this.xRotationClamp += this.xRotationAmount;

        this.yRotationAmount = Input.GetAxis("Mouse X") * this.mouseSensitivity;

        this.yRotator.transform.Rotate(Vector3.up, this.yRotationAmount);

        if (this.xRotationClamp > this.maxRotationAngle) {
            this.xRotationClamp = this.maxRotationAngle;

        }
        else if (this.xRotationClamp < -this.maxRotationAngle) {
            this.xRotationClamp = -this.maxRotationAngle;
        }
        else {
            this.xRotator.transform.Rotate(Vector3.right, this.xRotationAmount);
        }
    }
}
