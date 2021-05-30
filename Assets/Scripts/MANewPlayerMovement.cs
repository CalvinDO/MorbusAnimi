using System;
using UnityEngine;

public class MANewPlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -29.43f;
    public float jumpHeight = 3f;
    public float interactDistance = 1f;

    public Transform groundCheck;
    public float groundDistance = 1f;
    public LayerMask groundMask;
    public GameObject playerInventory;

    Vector3 velocity;
    bool isGrounded;
    bool isClimbing;

    Camera cam;
    public Material inputHighlightMaterial;

    MAInteractable hover;

    public static Material highlightMaterial;

    private void Start()
    {
        MAPlayerMovement2.highlightMaterial = inputHighlightMaterial;
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * -z + transform.forward * x;
        Vector3 climb = transform.up * z + transform.forward * x; 

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.forward, out hit))
        {
            float distance = hit.distance;
            Debug.Log(distance);
            if (Input.GetButton("Interact"))
            {
                GameObject climbable = hit.collider.gameObject;
                Debug.Log(climbable.name);
                if (climbable.layer == LayerMask.NameToLayer("Climb") && distance <= interactDistance)
                {
                    gravity = 0f;
                    isClimbing = true;
                }
                else
                {
                    gravity = -29.43f;
                    isClimbing = false;
                }
            }
        }

        if (isClimbing)
        {
            controller.Move(climb * (speed/2) * Time.deltaTime);
        } else
        {
            controller.Move(move * speed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
           velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(ray, out hit))
            {
                MAInteractable interactable = hit.collider.GetComponent<MAInteractable>();
                if (this.hover == interactable)
                {
                    if (Input.GetMouseButtonDown(0) && interactable != null)
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
        }

        // Button Inputs
        if (Input.GetButtonDown("Menu"))
        {
            bool currentActivity = playerInventory.activeSelf;
            playerInventory.SetActive(!currentActivity);
        }
    }
}
