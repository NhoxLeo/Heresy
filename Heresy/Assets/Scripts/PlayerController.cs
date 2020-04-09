using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{

    
    public Material glowMaterial;
    public Material skinMaterial;
    public bool attacking;
    public bool dashStab = false;
    public float gravity = 10.0f;
    public float groundClamp;
    public float speed;
    public float startSpeed;
    public float turnSpeed;
    public float beamSlashT;

    public float dashSpeed;
    public bool dashIsCD = false;
    public float dashTimer;
    public float dashCD;
    
    public GameObject playerMesh;
    public GameObject beam;
    Vector3 movement;

    Animator playerAnim;
    Rigidbody rb;

    Collider blade;
    public Transform enemy;
    public void Start()
    {
        blade = GameObject.Find("Blade").gameObject.GetComponent<Collider>();

        playerMesh = GameObject.Find("Mesh");

        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        speed = startSpeed;
    }

    public void FixedUpdate()
    {

        if (!playerAnim.GetBool("Attack") && (!playerAnim.GetBool("Attack2"))&& (!playerAnim.GetBool("Attack3")) && (!playerAnim.GetBool("Attack4")) && !playerAnim.GetBool("Attack5") && (!playerAnim.GetBool("Power Up")))
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
                    materials[1] = skinMaterial;
                    GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;

                }
                playerMesh.SetActive(true);
                
                playerAnim.SetBool("Dash", false);
                
            } 
            if (dashTimer <= 0)
            {
                dashIsCD = false;

                SkinnedMeshRenderer[] skinMeshList = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

                foreach (SkinnedMeshRenderer skin in skinMeshList)
                {

                    var materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
                    materials[0] = skinMaterial;
                    materials[1] = glowMaterial;
                    GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;

                }

            }
           
            if(dashTimer >= 0.8 && dashTimer <= 0.9)
            {                
                playerMesh.SetActive(false);

            }                      
            
            
            if(dashTimer <=0.6)
            {
                playerAnim.SetBool("Dash", false);
               
            }
            if (dashTimer >= 0.8 && dashTimer <= 1 && Input.GetKeyDown(KeyCode.Mouse0))
            {
                playerAnim.SetBool("Dash", true);

            }




        }
           
            if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0)
            {
                rb.AddForce(movement * dashSpeed, ForceMode.Impulse);
                dashTimer = dashCD;
                dashIsCD = true;
                

               SkinnedMeshRenderer[] skinMeshList = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

               foreach (SkinnedMeshRenderer skin in skinMeshList)
               {

                     var materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
                     materials[0] = glowMaterial;
                     materials[1] = glowMaterial;
                     GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;
                     
               }
            }       
        
        if (Input.GetKeyDown(KeyCode.Mouse1) && playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Moving")|| Input.GetKeyDown(KeyCode.Mouse1) && playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle")) 
        { 
              playerAnim.SetBool("Power Up", true);

        }

        if (Input.GetKey(KeyCode.E))
        {
            enemy = EnemyDetection.GetClosestEnemy(EnemyDetection.enemies, transform);

            if (enemy)
            {
                Debug.Log(enemy.name);
                transform.LookAt(enemy);
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                enemy = EnemyDetection.GetClosestEnemy(EnemyDetection.enemies, transform);

                if (enemy)
                {
                    Debug.Log(enemy.name);
                    transform.LookAt(enemy.transform);
                }
            }
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
    
    public void Attack3()
    {
        
        playerAnim.SetBool("Attack3", false);

    }    
    public void Attack4()
    {
        
        playerAnim.SetBool("Attack4", false);

    }    
    public void Attack5()
    {
        
        playerAnim.SetBool("Attack5", false);

    }
    
    
    public void attackColliderOn()
    {
        blade.enabled = true;
    }
    public void attackColliderOff()
    {
        blade.enabled = false;
    }

    public void beamSpawn()
    {
        
        Instantiate(beam,transform.position + (transform.forward * 2), transform.rotation);
        playerAnim.SetBool("Power Up", false);
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