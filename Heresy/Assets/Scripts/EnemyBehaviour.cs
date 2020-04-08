using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public float groundClamp;
    public float speed;

    public Transform player;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.transform.LookAt(player);

        transform.position = transform.position + new Vector3(0, groundClamp, 0);

        Vector3 target = new Vector3(player.position.x, player.position.y, player.position.z);
        Vector3 newPos = Vector3.MoveTowards(rb.position, target, speed * Time.deltaTime);
        rb.MovePosition(newPos);
    }
}
