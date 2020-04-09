using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{

    
    public Material glowMaterial;
    public Material skinMaterial;
    public bool attacking;

    public float gravity = 10.0f;
    public float groundClamp;
    public float speed;
    public float startSpeed;
    public float turnSpeed;
    
    public float dashSpeed;
    public bool dashIsCD = false;
    public float dashTimer;
    public float dashCD;
    public GameObject playerMesh;

    Vector3 movement;

    Animator playerAnim;
    Rigidbody rb;

    Collider blade;

    public void Start()
    {
        blade = GameObject.Find("Blade").gameObject.GetComponent<Collider>();

        playerMesh = GameObject.Find("ganfaul_m_aure (2)");

        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        speed = startSpeed;
    }

    public void FixedUpdate()
    {

        if (!playerAnim.GetBool("Attack") && (!playerAnim.GetBool("Attack2")))
        {
            PlayerMovement();
        }   
        

    }

    public void Update()
    {
        rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));

        Sprint();
        Attack();
        if (dashIsCD == true)
        {
            dashTimer -= Time.deltaTime;
                       if(dashTimer <= 0.6)
            {                
                SkinnedMeshRenderer[] skinMeshList = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

                foreach (SkinnedMeshRenderer skin in skinMeshList)
                {

                    var materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
                    materials[0] = skinMaterial;
                    materials[1] = glowMaterial;
                    GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;

                }
                playerAnim.SetBool("Dash", false);
                playerMesh.SetActive(true);
            } 
            if (dashTimer <= 0)
            {
                dashIsCD = false;

                SkinnedMeshRenderer[] skinMeshList = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

                foreach (SkinnedMeshRenderer skin in skinMeshList)
                {

                    var materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
                    materials[0] = skinMaterial;
                    materials[1] = skinMaterial;
                    GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;

                }

            }
           
            if(dashTimer >= 0.8 && dashTimer <= 0.9)
            {                
                

                playerMesh.SetActive(false);


            }
        
        }
           
            if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0)
            {
                rb.AddForce(movement * dashSpeed, ForceMode.Impulse);
                dashTimer = dashCD;
                dashIsCD = true;
                playerAnim.SetBool("Dash", true);

               SkinnedMeshRenderer[] skinMeshList = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

               foreach (SkinnedMeshRenderer skin in skinMeshList)
               {

                     var materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
                     materials[0] = glowMaterial;
                     materials[1] = glowMaterial;
                     GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;
                     
               }
            }
    }

    public void PlayerMovement()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {


            transform.position = transform.position + new Vector3(0, groundClamp, 0);

            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");

            movement = new Vector3(moveHorizontal, -0.0f, moveVertical);

            movement = Camera.main.transform.TransformDirection(movement);
            movement.y = 0f;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * turnSpeed);



            transform.Translate(movement.normalized * speed * Time.deltaTime, Space.World);
            playerAnim.ResetTrigger("Idle");
            playerAnim.SetTrigger("Moving");




        }            
        else
            {

                playerAnim.ResetTrigger("Moving");
                playerAnim.SetTrigger("Idle");
            }
                


    }


    void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            speed *= 1.5f;
            playerAnim.SetBool("Sprinting", true);
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            speed = startSpeed;
            playerAnim.SetBool("Sprinting", false);
        }
    }   
    
    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           // playerAnim.SetBool("Attack",true);
           
            //playerAnim.SetBool("Attack2", true);
            playerAnim.ResetTrigger("Moving");
            playerAnim.ResetTrigger("Idle");           
        }



    }

    public void Attack1()
    {
        playerAnim.SetBool("Attack", false);

        
        
       
        

    }   
    
    public void Attack2()
    {
        
        playerAnim.SetBool("Attack2", false);

    }
    public void attackColliderOn()
    {
        blade.enabled = true;
    }
    public void attackColliderOff()
    {
        blade.enabled = false;
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