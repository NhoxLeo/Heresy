using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    //Get components
    Collider wings;
    Collider sword;

    public Animator animator;
    public NavMeshAgent agent;

    //Objects
    public GameObject hitParticle;
    

    //Materials
    public Material hitMat;
    public Material hitMat2;

    //Stats
    public int e1_HP = 10; //enemy HP
    public static int e1_ATK = 5; //Enemy DMG
    
    //effects
    public float hitSlow_T; //how long game slows for hits
    private float hitColour; //shader colour change when hit

    // Start is called before the first frame update

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
    }
    void Start()
    {
        //Get Components
        animator = GetComponent<Animator>();
        wings = gameObject.GetComponentInChildren<BoxCollider>();
        sword = GameObject.Find("Blade").gameObject.GetComponent<Collider>();


        //Set Materials
        hitMat.SetFloat("Vector1_1F4E68D2", 0.1f);
        hitMat2.SetFloat("Vector1_3D6E13D4", 0f);
        hitColour = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        
        //Ignore collision between wing and sword
        Physics.IgnoreCollision(wings, sword);

        Death();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {

            StartCoroutine(TakeDmg());

        }
        if (other.gameObject.CompareTag("Ground"))
        {
            agent.enabled = true;
        }

    }

    public IEnumerator TakeDmg()
    {       
        //Turn off attack collider
        wings.enabled = false;
        
        //Lose HP
        e1_HP -= PlayerController.p_DMG;  
        
        //Start animation
        animator.SetTrigger("Enemy_Hit");
        
        //Change colour
        hitMat.SetFloat("Vector1_1F4E68D2", hitColour += 0.2f);
        hitMat2.SetFloat("Vector1_3D6E13D4", 50f);
                        
        //Instantiate partile effect
        Instantiate(hitParticle, transform.position + (transform.forward / 2), hitParticle.transform.rotation); 

        //Slow time
        Time.timeScale = 0.33f;
        
        //wait for slow down time 
        yield return new WaitForSeconds(hitSlow_T);
        
        //change colour back
        hitMat2.SetFloat("Vector1_3D6E13D4", 1f);
        //turn time back
        Time.timeScale = 1;
    }


    public void WingAttackOn()
    {
        wings.enabled = true;

    }

    public void WingAttackOff()
    {
        wings.enabled = false;

    }

    public void Death()
    {
        if (e1_HP <= 0)
        {
            hitMat2.SetFloat("Vector1_3D6E13D4", 1f);
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }
}
