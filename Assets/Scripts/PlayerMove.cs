using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float moveSpeed, gravityForce, jumpForce, sprintSpeed;
    public CharacterController characterController;
    private Vector3 moveInput;

    public Transform cameraTransform;

    public float mouseSensitivity;

    private bool canJump;
    public Transform groundCheckPoint;
    public LayerMask ground;

    public Animator animator;

    public GameObject bullet;
    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float yVelocity = moveInput.y;

        Vector3 verticalMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horizontalMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = horizontalMove + verticalMove;
        moveInput.Normalize();

        if(Input.GetKey(KeyCode.LeftShift))
        {
            moveInput = moveInput * sprintSpeed;
        }
        else
        {
            moveInput = moveInput * moveSpeed;
        }

        //moveInput = moveInput * moveSpeed;

        moveInput.y = yVelocity;
        moveInput.y += Physics.gravity.y * gravityForce * Time.deltaTime;

        if(characterController.isGrounded)
        {
            moveInput.y = Physics.gravity.y * gravityForce * Time.deltaTime;
        }

        canJump = Physics.OverlapSphere(groundCheckPoint.position, 0.2f, ground).Length > 0;

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            moveInput.y = jumpForce;
        }


        characterController.Move(moveInput * Time.deltaTime);

        //Camera rotation functionality
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        cameraTransform.rotation = Quaternion.Euler(cameraTransform.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

        //Shooting
        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }

        animator.SetFloat("moveSpeed", moveInput.magnitude);
        animator.SetBool("onGround", canJump);
    }
}
