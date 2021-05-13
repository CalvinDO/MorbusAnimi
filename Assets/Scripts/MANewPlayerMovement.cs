using System;
using UnityEngine;

public class MANewPlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float interactDistance = 1f;

    public Transform groundCheck;
    public float groundDistance = 1f;
    public LayerMask groundMask;
    public GameObject playerInventory;

    Vector3 velocity;
    bool isGrounded;

    public Camera cam;
    public Material inputHighlightMaterial;

    MAInteractable hover;

    public static Material highlightMaterial;

    private void Start()
    {
        MAPlayerMovement2.highlightMaterial = inputHighlightMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * -z + transform.forward * x;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, interactDistance))
        {

            MAInteractable interactable = hit.collider.GetComponent<MAInteractable>();
            if (this.hover == interactable)
            {
                if (Input.GetButtonDown("Interact") && interactable != null)
                {
                    this.hover.MAInteract();
                }
                return;
            }
            if (interactable != null)
            {
                this.hover = interactable;
                this.hover.setHover();
            }
            else
            {
                this.hover.removeHover();
                this.hover = null;
            }
        }

        // Button Inputs
        if (Input.GetButtonDown("Menu"))
        {
            bool currentActivity = playerInventory.activeSelf;
            playerInventory.SetActive(!currentActivity);
        }
    }
}
