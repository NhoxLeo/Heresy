using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{

    public GameObject hitParticle;
    public Animator animator;

    public Collider wings;
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
        wings = gameObject.GetComponentInChildren<BoxCollider>();

        hitMat.SetFloat("Vector1_1F4E68D2", 0.1f);
        hitMat2.SetFloat("Vector1_3D6E13D4", 0f);
        hitColour = 0f;

        sword = GameObject.Find("Blade").gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {

        Physics.IgnoreCollision(wings, sword);
        Death();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {

            StartCoroutine(TakeDmg());

        }
    }

    public IEnumerator TakeDmg()
    {
        Debug.Log("Hit");
        //Change colour
        hitMat.SetFloat("Vector1_1F4E68D2", hitColour += 0.2f);
        hitMat2.SetFloat("Vector1_3D6E13D4", 50f);
        //Lose HP
        e1_HP -= PlayerController.p_DMG;
        //Instantiate partile effect
        Instantiate(hitParticle, transform.position + (transform.forward / 2), hitParticle.transform.rotation);
        //Slow time
        Time.timeScale = 0.33f;
        //Start animation
        animator.SetTrigger("Enemy_Hit");
        //Turn off attack collider
        wings.enabled = false;
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
