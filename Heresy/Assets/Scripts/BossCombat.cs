using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossCombat : MonoBehaviour
{
    // Variables for boss health
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject Baal;
    Collider leftHandCollider;
    Collider rightHandCollider;
    public SkinnedMeshRenderer baalSMR;
    public Material flash;
    public Material skin;
    public BossHealthBar bossHealthBar;

    // Animator controller variable
    public Animator animator;
    public NavMeshAgent agent;

    // Transform for empty game object of where the boss attacks
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    // Variable for boss movement speed
    public float speed = 2.5f;

    // Variable for boss attack range
    public float attackRange = 3f;

    // Variable for the player and navmesh agent to find the player
    public GameObject player;
    public GameObject minion;
    public GameObject blood;

    public static int leftFistDamage = 20;
    public static int rightFistDamage = 10;
    public float hitSlow_T;
    public float HPercent;
    Collider sword;

    public bool roaring = false;
    // Start is called before the first frame update
    void Start()
    {
        Baal = GameObject.FindGameObjectWithTag("Baal");
        baalSMR = GameObject.Find("Baal").GetComponentInChildren<SkinnedMeshRenderer>();
        // Find and set colliders
        leftHandCollider = GameObject.Find("LeftHandCollider").GetComponent<Collider>();
        rightHandCollider = GameObject.Find("RightHandCollider").GetComponent<Collider>();
        sword = GameObject.Find("Blade").gameObject.GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        // boss starting health is equal to its max health
        currentHealth = maxHealth;
        bossHealthBar.SetMaxHealth(maxHealth);

        // When the script runs, find the game object with "Player" tag and find the navmesh agent
        player = GameObject.FindGameObjectWithTag("Player");

        HPercent = currentHealth / maxHealth;
    }


    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Roar());
        Die();
        Physics.IgnoreCollision(leftHandCollider, sword);
        Physics.IgnoreCollision(rightHandCollider, sword);
        


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            StartCoroutine(TakeDamage(1));
            //Instantiate(blood, transform.position + (transform.up *2 + transform.forward / 2), transform.rotation);
        }
    }

    public void Die()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Enemy died!");
        }
    }


    public IEnumerator TakeDamage(int damage)
    {      
        currentHealth -= damage;
        
        baalSMR.material = flash;
        bossHealthBar.SetHealth(currentHealth);
        //Slow time
        Time.timeScale = 0.33f;
        //Turn off attack collider
        leftHandCollider.enabled = false;
        rightHandCollider.enabled = false;
        //wait for slow down time 
        yield return new WaitForSeconds(hitSlow_T);
        baalSMR.material = skin;
        //turn time back
        Time.timeScale = 1;
    }

    public void SummonMinion()
    {
        Instantiate(minion, spawnPoint1);
        Instantiate(minion, spawnPoint2);
    }

    public IEnumerator Roar()
    {

        if (currentHealth == 75 && roaring == false||currentHealth == 50 && roaring == false || currentHealth == 25 && roaring == false)
        {
            agent.isStopped = true;
            animator.SetBool("Running", false);
            animator.SetBool("Roar", true);
            roaring = true;
            yield return new WaitForSeconds(30);
            roaring = false;
        }
        

    }

    public void ResetRoar()
    {
        animator.SetBool("Roar", false);
        
    }

    public void StopAgent()
    {
        agent.isStopped = true;
            
    }
    public void StartAgent()
    {
        
        agent.isStopped = false;
            
    }        
    public void Airtele()
    {
        
        agent.Warp(player.transform.position - player.transform.forward *-1);
            
    }



    public void LHCollOn()
    {
        leftHandCollider.enabled = true;
    }
    public void LHCollOff()
    {
        leftHandCollider.enabled = false;
    }    
    public void RHCollOn()
    {
        rightHandCollider.enabled = true;
    }
    public void RHCollOff()
    {
        rightHandCollider.enabled = false;
    }




}

