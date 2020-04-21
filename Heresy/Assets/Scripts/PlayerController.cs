using UnityEngine;


public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;

    Vector3 movement;
    Collider blade;
    Collider foot;
    Collider hand;
    
    public Material glowMaterial;
    public Material skinMaterial;
    
    public GameObject playerMesh;
    public GameObject beam;
    
    public bool PhaseIsCD = false;
    private bool phaseAttackNoInvis = false;
    public static bool lockedOn = false;

    public float startRunSpeed;
    public float turnSpeed;
    public float dashSpeed;

    
    private float phaseCDTimer;
    public float phaseDashCDTime;
    public float phaseSlashCDTime;
    public float homingSpeed;

    public float phaseInvis = 0.8f;
    public float endPhaseInvis = 0.6f;
    public float attackrotspeed = 1f;

    private float gravity = 30.0f;
    private float groundClamp = 0f;
    private float speed;

    public Transform enemy;
    public void Start()
    {
        

        //Gets the collider on the Blade
        blade = GameObject.Find("Blade").gameObject.GetComponent<Collider>();
        foot = GameObject.Find("RightFoot").gameObject.GetComponent<Collider>();
        hand = GameObject.Find("LeftHand").gameObject.GetComponent<Collider>();
        //Gets the Mesh on the Player
        playerMesh = GameObject.Find("Mesh");

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        
        //Makes the Players speed equal the set speed
        speed = startRunSpeed;


    }

    public void FixedUpdate()
    {
        //If the player isn't attacking they can move
        if (!animator.GetBool("Attack") && (!animator.GetBool("Attack2"))&& (!animator.GetBool("Attack3")) && (!animator.GetBool("Attack4")) && (!animator.GetBool("Attack5")) && (!animator.GetBool ("Power Up")) && (!animator.GetBool("Dash")))
        {
            PlayerMovement();
        }   
        

    }

    public void Update()
    {
        //Add self created gravity
        rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));
        
        Sprint();
        Attack();
        PhaseAttack();
        PhaseDash();
       
        //if Phase is on CD
        if (PhaseIsCD == true)
        {   
            //Dash CoolDown Satrts
            phaseCDTimer -= Time.deltaTime;
        }            
        
        //if Phase CD has ended
        if (phaseCDTimer <= 0)
        {
            //set CoolDown to false
            PhaseIsCD = false;
            phaseAttackNoInvis = false;
            SkinnedMeshRenderer[] skinMeshList = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach (SkinnedMeshRenderer skin in skinMeshList)
            {
                //Set player skin back to glowing
                var materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
                materials[0] = skinMaterial;
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
            
            //if moving play move animation
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Moving");
        }
        else
        {
            //if not moving play idle animation
            animator.ResetTrigger("Moving");
            animator.SetTrigger("Idle");
        }
        
                


    }


    void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            speed *= 1.5f;
            animator.SetBool("Sprinting", true);
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            speed = startRunSpeed;
            animator.SetBool("Sprinting", false);
        }
    }   
    
    public void Attack()
    {


        if (animator.GetBool("Attack") == true && enemy)
        {
            Vector3 direction = enemy.transform.position - rb.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            rb.transform.rotation = Quaternion.Lerp(rb.transform.rotation, rotation, attackrotspeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !animator.GetBool("Dash"))
        {   
            enemy = EnemyDetection.GetClosestEnemy(EnemyDetection.enemies, transform);

            if (enemy)
            {
                //Debug.Log(enemy.name);

            }

            animator.ResetTrigger("Moving");
            animator.ResetTrigger("Idle");           
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            lockedOn = !lockedOn;
    
        }            
        if (lockedOn == true)
        {
                enemy = EnemyDetection.GetClosestEnemy(EnemyDetection.enemies, transform);

                if (enemy)
                {

                    //Debug.Log(enemy.name);
                    Camera.main.transform.LookAt(enemy);


                }
            else
            {
                lockedOn = false;
            }
        }
    }

    public void PhaseAttack()
    {
        if (PhaseIsCD == false)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && animator.GetCurrentAnimatorStateInfo(0).IsName("Moving") || Input.GetKeyDown(KeyCode.Mouse1) && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.SetBool("Power Up", true);

            }

        }

    }    
    public void PhaseDash()
    {
        //if dash button  is pressed when no cooldown
        if (Input.GetKeyDown(KeyCode.LeftShift) && PhaseIsCD == false && !animator.GetBool("Power Up") && animator.GetCurrentAnimatorStateInfo(0).IsName("Moving") || Input.GetKeyDown(KeyCode.LeftShift) && PhaseIsCD == false && !animator.GetBool("Power Up") && animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            //dash force
            rb.AddForce(movement * dashSpeed, ForceMode.Impulse);
            //dash cooldown timer sets to Phase timer
            phaseCDTimer = phaseDashCDTime;
            //Phase goes on cooldown  
            PhaseIsCD = true;


            SkinnedMeshRenderer[] skinMeshList = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach (SkinnedMeshRenderer skin in skinMeshList)
            {
                //Set skin to Phasing
                var materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
                materials[0] = glowMaterial;
                materials[1] = glowMaterial;
                GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;

            }
        }

        if (phaseCDTimer <= endPhaseInvis)
        {
            SkinnedMeshRenderer[] skinMeshList = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach (SkinnedMeshRenderer skin in skinMeshList)
            {
                //Changes skin to standard, no glow
                var materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
                materials[0] = skinMaterial;
                materials[1] = skinMaterial;
                GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;

            }

            // turns player mesh on
            playerMesh.SetActive(true);


        }             
        
        if (phaseCDTimer >= phaseInvis && phaseCDTimer <= 0.9 && phaseAttackNoInvis == false) 
        {                
            playerMesh.SetActive(false);

        }
        if (phaseCDTimer >= phaseInvis && phaseCDTimer <= phaseDashCDTime && Input.GetKeyDown(KeyCode.Mouse0) && animator.GetCurrentAnimatorStateInfo(0).IsName("Moving"))
        {
            animator.SetBool("Dash", true);

        }
    }


    
    
    public void AttackColliderOn()
    {
        blade.enabled = true;
    }
    public void AttackColliderOff()
    {
        blade.enabled = false;
    }    
    public void FootColliderOn()
    {
        foot.enabled = true;
    }
    public void FootColliderOff()
    {
        foot.enabled = false;
    }    
    public void HandColliderOn()
    {
        hand.enabled = true;
    }
    public void HandColliderOff()
    {
        hand.enabled = false;
    }

    public void beamSpawn()
    {  
        Instantiate(beam,transform.position + (transform.forward * 2), transform.rotation);
        
        animator.SetBool("Power Up", false);                
        
        SkinnedMeshRenderer[] skinMeshList = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer skin in skinMeshList)
        {
            //Changes skin to standard, no glow
            var materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
            materials[0] = skinMaterial;
            materials[1] = skinMaterial;
            GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;

        }
    }
    public void Attack1()
    {
        animator.SetBool("Attack", false);
    }   
    public void Attack2()
    {
        animator.SetBool("Attack2", false);
    }       
    public void Attack3()
    {
        animator.SetBool("Attack3", false);
    }    
    public void Attack4()
    {
        animator.SetBool("Attack4", false);
    }    
    public void Attack5()
    {
        animator.SetBool("Attack5", false);
    }
    public void StartPhaseAttack()
    {

        phaseCDTimer = phaseSlashCDTime;
        PhaseIsCD = true;
        phaseAttackNoInvis = true;
        SkinnedMeshRenderer[] skinMeshList = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer skin in skinMeshList)
        {
            //Set skin to Phasing
            var materials = GetComponentInChildren<SkinnedMeshRenderer>().materials;
            materials[0] = glowMaterial;
            materials[1] = glowMaterial;
            GetComponentInChildren<SkinnedMeshRenderer>().materials = materials;

        }
    }  
    
    public void EndDashAnimation()
    {
        animator.SetBool("Dash", false);
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