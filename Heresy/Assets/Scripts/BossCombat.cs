using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossCombat : MonoBehaviour
{
    
    //components
    public BossHealthBar bossHealthBar;
    public Animator animator;
    public Animator fadeOut;
    public NavMeshAgent agent;
    public SkinnedMeshRenderer baalSMR;
    public AudioSource hit;
    public AudioSource summonMinion;
    public AudioSource bigHit;
    public AudioSource flameWoosh;
    

    //GameObjects
    public GameObject player;
    public GameObject minion;
    public GameObject minionParticle;
    public GameObject blood;
    public GameObject fireBall;
    public GameObject baalBlowUp;
    public GameObject baalBlowUp2; 
    public GameObject baalIntro;
    //Colliders
    Collider leftHandCollider;
    Collider rightHandCollider;
    Collider sword;
    Collider baal;
    //materials
    public Material flash;
    public Material skin;
    
    // Transform for empty game object of where the boss attacks
    public Transform e_spawnPoint1;
    public Transform e_spawnPoint2;
    public Transform fireBallSpawn;
    
    //Stats
    public int maxHealth = 100; //Max Hp
    public int currentHealth;   //Actual health at any time   
    public float attackRange = 3f; // boss attack range
    public static int leftFistDamage = 20; //DMG with left attack
    public static int rightFistDamage = 10; //Damage with right
    public float hitSlow_T; //Game low down time after being hit
    
    //If boss is roaring
    public bool roaring = false;
    public bool fireBallCharge = false;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(baalIntro, gameObject.transform);
        //Get and set boss, boss mesh and boss NavMeshAgent
        baalSMR = GameObject.Find("Baal").GetComponentInChildren<SkinnedMeshRenderer>();
        agent = GetComponent<NavMeshAgent>();
        //Get and set the player
        player = GameObject.FindGameObjectWithTag("Player");
        //Get and set Colliders
        leftHandCollider = GameObject.Find("LeftHandCollider").GetComponent<Collider>();
        rightHandCollider = GameObject.Find("RightHandCollider").GetComponent<Collider>();
        sword = GameObject.Find("Blade").gameObject.GetComponent<Collider>();
        baal = GetComponent<Collider>();
        //Set boss health to max
        currentHealth = maxHealth;
        bossHealthBar.SetMaxHealth(maxHealth);

        
    }


    // Update is called once per frame
    void Update()
    {
        //starts to roar
        StartCoroutine(Roar());

        StartCoroutine(Fireball());
        //boss dies
        StartCoroutine(Die());
        //Ignore the collision between the bosses attack colliders and the players sword collider
        Physics.IgnoreCollision(leftHandCollider, sword);
        Physics.IgnoreCollision(rightHandCollider, sword);
        
        

    }

    //If a trigger enters collider
    private void OnTriggerEnter(Collider other)
    {
        //if the sword hits 
        if (other.gameObject.CompareTag("Sword"))
        {
            //Take damage of int
            StartCoroutine(TakeDamage(1));
            //Instantiate(blood, transform.position + (transform.up *2 + transform.forward / 2), transform.rotation);
        }
    }

    //Boss Takes Damage from player
    public IEnumerator TakeDamage(int damage)
    {   
        //Boss takes DMG
        currentHealth -= damage;
        // replace skin MAT with flash MAT
        baalSMR.material = flash;
        //change boss health bar to current health
        bossHealthBar.SetHealth(currentHealth);
        //Slow time
        Time.timeScale = 0.33f;
        //Turn off attack collider
        leftHandCollider.enabled = false;
        rightHandCollider.enabled = false;
        hit.Play();
        //wait for slow down time 
        yield return new WaitForSeconds(hitSlow_T);
        
        //turn skin back to normal
        baalSMR.material = skin;
        //turn time back
        Time.timeScale = 1;
    }
    
    //Boss is Roaring
    public IEnumerator Roar()
    {
        //If the boss is at these hp values and it is not roaring
        if (currentHealth == 75 && roaring == false||currentHealth == 50 && roaring == false || currentHealth == 25 && roaring == false)
        {   
            //boss is roaring
            roaring = true;
            //boss stops moving
            agent.isStopped = true;
            //run animation is false
            animator.SetBool("Running", false);
            //Boss roar animation
            animator.SetBool("Roar", true);
         
            //Wait
            yield return new WaitForSeconds(20);
            
            //boss is not roaring
            roaring = false;
        }
        
    }   
    
    public IEnumerator Fireball()
    {
        if (currentHealth <= 47 && fireBallCharge == false )
        {
            //boss is roaring
            fireBallCharge = true;
            //run animation is false
            agent.enabled = false;
            animator.SetBool("Running", false);
            //boss stops moving
            //Boss roar animation
            animator.SetBool("FireBall",true);
           

            //Wait
            yield return new WaitForSeconds(25); 

            //boss is not roaring
            fireBallCharge = false;
        }
    }

    //Boss Dies
    public IEnumerator Die()
    {
        //If health is/ is below 0
        if (currentHealth <= 0)
        {     
            
            agent.isStopped = true;
            animator.SetBool("IsDead", true);
            baal.enabled = false;
            yield return new WaitForSeconds(4);
            Instantiate(baalBlowUp, gameObject.transform);
            Instantiate(baalBlowUp2, gameObject.transform);
            yield return new WaitForSeconds(0.1f);
            gameObject.SetActive(false);
            fadeOut.SetBool("Fade", true);
        }
    }

    //Events
    public IEnumerator SummonMinion()
    {   //Spawn minions at these points  
        summonMinion.Play();
        Instantiate(minionParticle, e_spawnPoint1);
        Instantiate(minionParticle, e_spawnPoint2);
        
        yield return new WaitForSeconds(3);
        Instantiate(minion, e_spawnPoint1);
        Instantiate(minion, e_spawnPoint2);        
 
    }    
    
    public void FireBall()
    {   
        //Spawn minions at these points
        Instantiate(fireBall, fireBallSpawn);
        
    }
    public void ResetRoar()
    {
        //set Roar False
        animator.SetBool("Roar", false);
    }
    public void StopAgent()
    {
        //Stop boss moving
        agent.enabled = false;
            
    }
    public void StartAgent()
    {
        //Start boss moving
        agent.enabled = true;
            
    }        
    public void Airtele()
    {
        //Warp Boss
        agent.Warp(player.transform.position - player.transform.forward * -4);
        flameWoosh.Play();
    }
    public void LHCollOn()
    {
        //Turn on attack collider
        leftHandCollider.enabled = true;
    }
    public void LHCollOff()
    {
        //Turn on attack collider
        leftHandCollider.enabled = false;
    }    
    public void RHCollOn()
    {
        //Turn on attack collider
        rightHandCollider.enabled = true;
    }
    public void RHCollOff()
    {
        //Turn on attack collider
        rightHandCollider.enabled = false;
    }
    public void FireBallFalse()
    {
        animator.SetBool("FireBall", false);
    }
    
    public void SummonSound()
    {
        summonMinion.pitch = 1.2f;
        summonMinion.Play();
    }   
    
    public void BigHitSound()
    {
        bigHit.Play();
        flameWoosh.Play();
    } 
    
    public void SmallHitSound()
    {
        flameWoosh.Play();
    }


}

