using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using EZCameraShake;
public class PlayerController : MonoBehaviour
{

    //Get components on object
    Animator animator;
    Rigidbody rb;
    Vector3 movement;
    public AudioSource dying;
    public AudioSource hit;
    public AudioSource attacking;
    public AudioSource specialAttack;
    public AudioSource hit2;
    public AudioSource hitFireBall;
    
    
    //Get Colliders for Attacking
    Collider blade;
    Collider foot;
    Collider hand;
    
    //Get Materials
    public Material glowMaterial;
    public Material skinMaterial;
    public Material flash;
    public Material origMat;
    
    //Get Objects in Scene and from prefabs
    public GameObject playerMesh;
    public GameObject beam;
    public Transform enemy;
    public GameObject deathScreen;
    //Phase system and cooldowns
    public bool PhaseIsCD = false;
    private bool phaseAttackNoInvis = false;
    private float phaseCDTimer;
    public float phaseDashCDTime;
    public float phaseSlashCDTime;
    
    //Lock on boolean
    public static bool lockedOn = false;
    
    //Dash invis timer
    public float phaseInvis = 0.8f;
    public float endPhaseInvis = 0.6f;
   
    //Gravity
    private float gravity = 30.0f;
    private float groundClamp = 0f;
    
    //PLAYER STATS
    public static float HP = 100;      //Current Health
    public int MaxHP = 100;            //Max Health
    public bool Regen = true;          //if regening health
    public float Regen_T = 0;          //timer till regening starts
    public static int p_DMG = 1;       //Damage given
    private float speed;               //Current speed
    public float startRunSpeed;        //Start Speed
    public float dashSpeed;            //Dash Speed
    public float turnSpeed;            // Turn to face move direction speed
    public float attackrotspeed = 1f;  //Turn to face enemy speed
    public float IFrameT = 0.5f;       //Invinsibility time


    private void Awake()
    {
        enemy = null;
        EnemyDetection.enemies.Clear();
        Time.timeScale = 1;
    }
    public void Start()
    {
        
        //Gets the colliders for dmg
        blade = GameObject.Find("Blade").gameObject.GetComponent<Collider>();
        foot = GameObject.Find("RightFoot").gameObject.GetComponent<Collider>();
        hand = GameObject.Find("LeftHand").gameObject.GetComponent<Collider>();
        
        //Gets the Mesh on the Player
        playerMesh = GameObject.Find("Mesh");
        
        //Get objects on the body
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        
        //Set HP and Spped to max 
        HP = MaxHP;
        PPVolumeControl.vgI = 0.15f;
        speed = startRunSpeed;

        origMat = skinMaterial;
        
    }

    public void FixedUpdate()
    {
        //If the player isn't attacking they can move 
        if (!animator.GetBool("Attack") && !animator.GetBool("Attack2")&& !animator.GetBool("Attack3") &&!animator.GetBool("Attack4") && !animator.GetBool("Attack5") && !animator.GetBool ("Power Up") && !animator.GetCurrentAnimatorStateInfo(0).IsName("React") && !animator.GetBool("Death"))
        {
            PlayerMovement();
        }   
        

    }

    public void Update()
    {
        enemy = EnemyDetection.GetClosestEnemy(EnemyDetection.enemies, transform);
        //Add self created gravity
        rb.AddForce(new Vector3(0, -gravity * rb.mass, 0));
        
        

            
            StartCoroutine(Die());
        
        Sprint();
        Attack();
        PhaseAttack();
        PhaseDash();
        Health();
        PhaseCD();

    }

    public void Health()
    {
        //if the player can't Regen
        if(Regen == false)
        {
            //Regen Timer starts
            Regen_T += Time.deltaTime;
        }
        //if player can regen
        else
        {
            //Heal
            HP += Time.deltaTime * 2;
            PPVolumeControl.vgI -= Time.deltaTime * 0.02f;

        }
        if(Regen_T >= 5)
        {
            Regen = true;
            Regen_T = 0;   
        }

        //HP doesn't ever exceed max
        if(HP >= MaxHP)
        {
            HP = MaxHP;
        }
        if(PPVolumeControl.vgI <= 0.15f)
        {
            PPVolumeControl.vgI = 0.15f;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wings"))
        {

            StartCoroutine(TakeMinionDMG());

        }        
        
        if (other.gameObject.CompareTag("LeftFist"))
        {

            StartCoroutine(LeftFistDMG());

        }        
        
        if (other.gameObject.CompareTag("RightFist"))
        {

            StartCoroutine(RightFistDMG());

        }        
        

       
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FireBall"))
        {

            StartCoroutine(TakeDMGFireBall());

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
        
        
       
        

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.ResetTrigger("Moving");
            animator.ResetTrigger("Idle");           
        }

        if (animator.GetBool("Attack") == true && enemy != null)
        {
            
            Vector3 direction = enemy.transform.position - rb.transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            rb.transform.rotation = Quaternion.Lerp(rb.transform.rotation, rotation, attackrotspeed * Time.deltaTime);
        }
        else
        {
            enemy = null;
        }




        if (Input.GetKeyDown(KeyCode.E))
        {
            lockedOn = !lockedOn;
    
        }            
        if (lockedOn == true)
        {
            
            if (enemy != null)
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

    public void PhaseCD()
    {
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
    public void ResetReact()
    {
        animator.ResetTrigger("Hit");
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

    public IEnumerator TakeMinionDMG()
    {
       
        //LoseHP
        HP -= Minion.e1_ATK;
        PPVolumeControl.vgI += 0.05f;
        Regen_T = 0;
        Regen = false;
        
        //Turn off sword collider
        blade.enabled = false;
        
        //play damage animation and stop moving
        animator.SetTrigger("Hit");
        hit.Play();
        //Flash
        skinMaterial = flash;
        Invoke("ResetHitColor", 0.05f);

        //Make Invinsible
        gameObject.GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;

        //Instantiate particle effect 
        //Wait for IFrames
        yield return new WaitForSeconds(IFrameT);
        
        //Become vinsible
        gameObject.GetComponent<Collider>().enabled = true;
        rb.isKinematic = false;
    }

    public IEnumerator TakeDMGFireBall()
    {
        //LoseHP
        HP -= 30;
        PPVolumeControl.vgI += 0.3f;
        Regen_T = 0;
        Regen = false;
        CameraShaker.Instance.ShakeOnce(5f, 0.5f, 0.2f, 0.2f);
        //Turn off sword collider
        blade.enabled = false;
        foot.enabled = false;
        hand.enabled = false;
        //play damage animation and stop moving
        animator.SetTrigger("Hit");
        hitFireBall.Play();
        //Flash
        skinMaterial = flash;
        Invoke("ResetHitColor", 0.05f);

        //Make Invinsible
        gameObject.GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;

        //Instantiate particle effect 
        //Wait for IFrames
        yield return new WaitForSeconds(IFrameT);

        //Become vinsible
        gameObject.GetComponent<Collider>().enabled = true;
        rb.isKinematic = false;
    }
    public IEnumerator LeftFistDMG()
    {
        CameraShaker.Instance.ShakeOnce(5f, 0.5f, 0.2f, 0.2f);
        //LoseHP
        HP -= BossCombat.leftFistDamage;
        PPVolumeControl.vgI += 0.20f;
        Regen_T = 0;
        Regen = false;
        hit.Play();
        hit2.Play();
        //Turn off sword collider
        blade.enabled = false;
        foot.enabled = false;
        hand.enabled = false;
        //play damage animation and stop moving
        animator.SetTrigger("Hit");
        //Flash
        skinMaterial = flash;
        Invoke("ResetHitColor", 0.05f);

        //Make Invinsible
        gameObject.GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;

        //Instantiate particle effect 
        //Wait for IFrames
        yield return new WaitForSeconds(IFrameT);

        //Become vinsible
        gameObject.GetComponent<Collider>().enabled = true;
        rb.isKinematic = false;

    }   
    public IEnumerator RightFistDMG()
    {
        //LoseHP
        HP -= BossCombat.rightFistDamage;
        PPVolumeControl.vgI += 0.10f;
        Regen_T = 0;
        Regen = false;
        hit.Play();
        hit2.Play();
        //Turn off sword collider
        blade.enabled = false;
        foot.enabled = false;
        hand.enabled = false;
        //play damage animation and stop moving
        animator.SetTrigger("Hit");
        //Flash
        skinMaterial = flash;
        Invoke("ResetHitColor", 0.05f);

        //Make Invinsible
        gameObject.GetComponent<Collider>().enabled = false;
        rb.isKinematic = true;

        //Instantiate particle effect 
        //Wait for IFrames
        yield return new WaitForSeconds(IFrameT);

        //Become vinsible
        gameObject.GetComponent<Collider>().enabled = true;
        rb.isKinematic = false;


    }

    public void ResetHitColor()
    {
      skinMaterial = origMat;
    }

    public IEnumerator Die()
    {
        if (HP <= 0)
        {
            gameObject.GetComponent<Collider>().enabled = false;
            rb.isKinematic = true;
            //Play Death animation
            animator.SetBool("Death", true);
            

            yield return new WaitForSeconds(3);
            //reset scene
            enemy = null;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            deathScreen.SetActive(true);
        }
    }
    public void SpecialNoise()
    {
        specialAttack.Play();
    }
    public void AttackSound()
    {
        attacking.Play();
    }

    public void DeathSound()
    {
        dying.Play();
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