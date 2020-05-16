using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamSlash : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = gameObject.transform.position + (transform.forward * speed);

        Destroy(gameObject, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
    private void OnTriggerEnter(Collider other)
    {
                if(other.gameObject.layer == 9)
        {
            Destroy(gameObject);
        }
    }
}
