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
    public int e1_HP = 10;
    public float hitSlow_T;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
        
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
        
        hitMat.SetFloat("Vector1_1F4E68D2", 10f);
        e1_HP -= PlayerController.p_DMG;
        Instantiate(hitParticle, transform.position + (transform.forward / 2), hitParticle.transform.rotation);
        Time.timeScale = 0.33f;
        Debug.Log("Hit");
        animator.SetTrigger("Enemy_Hit");
        wings.enabled = false;
       
        yield return new WaitForSeconds(hitSlow_T);
        
        hitMat.SetFloat("Vector1_1F4E68D2", 1f);
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
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            //Destroy(gameObject);
       }
    }
}
