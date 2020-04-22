using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAttack : MonoBehaviour
{

    public float quickPunch = 5f;
    public float bigSwipe = 10f;
    public float attackJump = 15f;
    public float rangedAttack = 50f;

    public int maxHealth = 100;
    int currentHealth;

    public Animator animator;

    public Collider handCollision;

    public float speed = 2.5f;
    public float attackRange = 3f;

    public GameObject player;
    public NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        agent = animator.GetComponent<NavMeshAgent>();
        handCollision = GetComponent<Collider>();
    }



    public void TakeDamage(int damage)
        {

            currentHealth -= damage;

        // Play hurt animation

        if(currentHealth <= 0)
        {
            Die();
        }   
    }   

    void Die()
    {
        Debug.Log("Enemy died!");
        // Die animation

        // Disable the enemy
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.transform.position);

        int randomAnimation = Random.Range(0, 4);

        if (Vector3.Distance(player.transform.position, agent.transform.position) <= attackRange)

        {
            animator.SetInteger("Attack", randomAnimation);
            handCollision.enabled = !handCollision.enabled;
        }

    }
}

