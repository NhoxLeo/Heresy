using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{
    
    
    public GameObject hitParticle;

    public Animator animator;
    Collider wings;
    Collider sword;

    public Material hitMat;
    public Material hitMat2;

    public int e1_HP = 10;
    public static int e1_ATK = 5; 
    public float hitSlow_T;
    private float hitColour;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        hitMat.SetFloat("Vector1_1F4E68D2", 0.1f);
        hitMat2.SetFloat("Vector1_3D6E13D4", 0f);
        hitColour = 0f;
        
        sword = GameObject.Find("Blade").gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
       wings = GameObject.Find("Heresy Enemy 1").gameObject.GetComponent<Collider>();
       Physics.IgnoreCollision(wings, sword);
       Death();    
        
    }

    private void OnTriggerEnter(Collider other)
    {       
        if (other.gameObject.CompareTag("Sword"))
        {

            StartCoroutine(Hit());
            
        }
    }

    public IEnumerator Hit()
    {
        
        hitMat.SetFloat("Vector1_1F4E68D2", hitColour += 0.2f);
        hitMat2.SetFloat("Vector1_3D6E13D4", 50f);
       
        e1_HP -= PlayerController.p_DMG;
        
        Instantiate(hitParticle, transform.position + (transform.forward / 2), hitParticle.transform.rotation);
        
        Time.timeScale = 0.33f;
        
        Debug.Log("Hit");
       
        animator.SetTrigger("Enemy_Hit");

        wings.enabled = false;
       
        yield return new WaitForSeconds(hitSlow_T);

        hitMat2.SetFloat("Vector1_3D6E13D4", 1f);
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
      if(e1_HP <= 0)
       {
            hitMat2.SetFloat("Vector1_3D6E13D4", 1f);
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            //Destroy(gameObject);
       }
    }
}
