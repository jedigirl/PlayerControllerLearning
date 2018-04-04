using UnityEngine;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour {

    CharacterController controller;
    CapsuleCollider capsule;
    public float speed = 5.0f;
    public float jumpForce = 20.0f;
    private float verticalVelocity;
    private Animator anim;
    private bool isDoubleJump;
    private bool canJump = false;
    public Text grounded;
    private bool isButtonClick;
    Vector3 moveDirection;
    bool isWallJump;
    float x;
    float z;
    bool isCrouching;
    Vector3 tempCenter;

    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
        capsule = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
	}

    private void Update()
    {
        if(controller.isGrounded && Input.GetButton("Fire1"))
        {
            isCrouching = true;
            controller.height = 1.3f;
            tempCenter = controller.center;
            tempCenter.y = .78f;
            controller.center = tempCenter;
            anim.SetBool("Crouch", true);
            


        }
        if(controller.isGrounded && Input.GetButtonUp("Fire1"))
        {
            
            isCrouching = false;
            controller.height = 1.94f;
            tempCenter = controller.center;
            tempCenter.y = .94f;
            controller.center = tempCenter;
            anim.SetBool("Crouch", false);
        }
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            canJump = true;
            anim.SetTrigger("Jump");
        }
        if(Input.GetButtonDown("Jump") && !controller.isGrounded && !isDoubleJump)
        {
            canJump = true;
            anim.SetTrigger("Jump");
        }
    }
    // Update is called once per frame
    void FixedUpdate () {
        if (!isWallJump)
        {
            x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            z = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        }
        grounded.text = isDoubleJump.ToString();
        if (controller.isGrounded)
        {
            isDoubleJump = false;
            isWallJump = false;
            verticalVelocity = -1 * Time.deltaTime;

            if (canJump)
            {
                verticalVelocity = jumpForce * Time.deltaTime;
                
                canJump = false;
            }
        }
        else
        {
            verticalVelocity -= 1 * Time.deltaTime;
            if(Input.GetButtonDown("Jump"))
            {
                if (!isDoubleJump)
                {
                    verticalVelocity = jumpForce * Time.deltaTime;
                    isDoubleJump = true;
                    canJump = false;
                    isWallJump = false;
                    anim.SetTrigger("Jump");
                }
               
            }
            
        }
        if (!isWallJump)
        {
            moveDirection = new Vector3(x, 0, z);
        }
        if(moveDirection != Vector3.zero)
        transform.rotation = Quaternion.LookRotation(moveDirection);
        Vector3 moveDelta = new Vector3(x, verticalVelocity, z);
        controller.Move(moveDelta);

        
        anim.SetFloat("Speed", controller.velocity.magnitude);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!controller.isGrounded && hit.normal.y < 0.1f )
        {
            if (Input.GetButtonDown("Jump"))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);
                verticalVelocity = jumpForce * Time.deltaTime;
                isWallJump = true;
                moveDirection = hit.normal * speed * Time.deltaTime;
                x = moveDirection.x;
                z = moveDirection.z;
                //isDoubleJump = false;
            }
        }

    }

}
