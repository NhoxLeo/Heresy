using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{

    public Animator animator;
    Collider wings;
    Collider sword;

    public float hitSlow_T;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        wings = GameObject.Find("Heresy Enemy 1").gameObject.GetComponent<Collider>();
        sword = GameObject.Find("Blade").gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
       Physics.IgnoreCollision(wings, sword); 
            
        
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
        Time.timeScale = 0.33f;
        Debug.Log("Hit");
        animator.SetTrigger("Enemy_Hit");
        wings.enabled = false;

        yield return new WaitForSeconds(hitSlow_T);

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
}
