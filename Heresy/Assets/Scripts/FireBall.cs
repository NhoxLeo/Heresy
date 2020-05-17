using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class FireBall : MonoBehaviour
{
    public GameObject player;

    

    public float speed;
    public GameObject pauseMenu;
    public Rigidbody rb;
    private Vector3 scaleChange;
    public bool moving = false;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("PlayerBody");
        scaleChange = new Vector3(0.01f, 0.01f, 0.01f);
        moving = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (moving == false)
        {
            StartCoroutine(FireBallMove());
        }

        if (moving == true)
        {
            gameObject.transform.parent = null;
            Vector3 direction = (player.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            gameObject.transform.localScale += scaleChange;
            CameraShaker.Instance.ShakeOnce(5f, 0.1f, 0.5f, 0.5f);
            Instantiate(explosion, gameObject.transform);
           
            Destroy(gameObject,0.1f);
        

        }
        
        if (collision.gameObject.layer == 9)
        {
            Debug.Log("Hit");
            gameObject.transform.localScale += scaleChange;
          
            Instantiate(explosion, gameObject.transform);
           
            Destroy(gameObject,0.1f);
        

        }
    }


    public IEnumerator FireBallMove()
    {

        gameObject.transform.localScale += scaleChange;

        yield return new WaitForSeconds(1);

        moving = true;

    }

}
