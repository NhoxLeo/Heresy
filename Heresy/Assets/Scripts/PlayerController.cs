using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{

    public float gravity = 10.0f;

    public float speed;
    public float startSpeed;
    public float turnSpeed;
    
    public float dashSpeed;
    public bool dashIsCD = false;
    public float dashTimer;
    public float dashCD;


    Rigidbody rb;
    
    public void Start()
    {
        rb = GetComponent<Rigidbody>();

        speed = startSpeed;
    }

    void FixedUpdate()
    {
        PlayerMovement();
        
    }

    public void Update()
    {
        rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));

        Sprint(); 
        
        if(dashIsCD == true)
        {
            dashTimer -= Time.deltaTime;
           
            if (dashTimer <= 0)
            {
                dashIsCD = false;
            }
        
        }
    
    }

    void PlayerMovement()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            transform.position = transform.position + new Vector3(0, -0.1f, 0);
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, -0.0f, moveVertical);

            movement = Camera.main.transform.TransformDirection(movement);
            movement.y = 0f;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * turnSpeed);
            

           
            transform.Translate(movement.normalized * speed * Time.deltaTime, Space.World);        
                      
            if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0)
            {
                rb.AddForce(movement * dashSpeed, ForceMode.Impulse);
                dashTimer = dashCD;
                dashIsCD = true;

            }



        }        
        

    }


    void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            speed *= 1.5f;
        }
        
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            speed = startSpeed;
        }
    }    


    
}





//Jump
//rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);

/*float CalculateJumpVerticalSpeed()
{
    // From the jump height and gravity we deduce the upwards speed 
    // for the character to reach at the apex.
    return Mathf.Sqrt(2 * jumpHeight * gravity);
}*/