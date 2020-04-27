using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossCombat : MonoBehaviour
{
    // Variables for boss health
    public int maxHealth = 100;
    public int currentHealth;

    Collider leftHandCollider;
    Collider rightHandCollider;

    public BossHealthBar bossHealthBar;

    // Animator controller variable
    public Animator animator;

    // Transform for empty game object of where the boss attacks
    public Transform attackPoint;

    // Variable for boss movement speed
    public float speed = 2.5f;

    // Variable for boss attack range
    public float attackRange = 3f;

    // Variable for the player and navmesh agent to find the player
    public GameObject player;
    public NavMeshAgent agent;

    public int attackDamage = 40;


    // Start is called before the first frame update
    void Start()
    {
        // Find and set colliders
        leftHandCollider = GameObject.Find("LeftHandCollider").GetComponent<Collider>();
        rightHandCollider = GameObject.Find("RightHandCollider").GetComponent<Collider>();

        // boss starting health is equal to its max health
        currentHealth = maxHealth;
        bossHealthBar.SetMaxHealth(maxHealth);

        // When the script runs, find the game object with "Player" tag and find the navmesh agent
        player = GameObject.FindGameObjectWithTag("Player");
        agent = animator.GetComponent<NavMeshAgent>();
    }



    // Update is called once per frame
    void Update()
    {
        
            
        Die();
            
        // Damage test
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(10);
        }

        void TakeDamage(int damage)
        {
            currentHealth -= damage;

            bossHealthBar.SetHealth(currentHealth);


        }

        void Die()
        {
            if (currentHealth <= 0)
            {
                Debug.Log("Enemy died!");
            }
        }

        // Every frame, set the transform of the boss to the position of the player
        agent.SetDestination(player.transform.position);

        
        // Every frame, check if the boss is in range of the player
        if (Vector3.Distance(player.transform.position, agent.transform.position) <= attackRange)

        {
            // If boss is in range of player call the attack function
            Attack();
        }

    }
    void Attack()
    {
        // Attack function is called
        // Play Random attack animation
        int randomAnimation = Random.Range(0, 4);
        animator.SetInteger("Attack", randomAnimation);

        // Detect player in range of attack
        Collider[] hitPlayer = Physics.OverlapSphere(attackPoint.position, attackRange);

        // Damage player
        foreach(Collider player in hitPlayer)
        {
            //Debug.Log("We hit" + player.name);
        }
    }


}

