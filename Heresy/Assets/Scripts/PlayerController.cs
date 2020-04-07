using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    public float turnTimeS = 0;
    public float turnTimeE = 0.2f;

    public float speed;
    public float turnSpeed;

    Rigidbody rb;
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        PlayerMovement();   
    }


    void PlayerMovement()
    {
        
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector3 movementFace = new Vector3(transform.position.x, 0, transform.position.z);
        
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0f;
       
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement+ movementFace),Time.deltaTime * turnSpeed);
       


        transform.Translate(movement.normalized * speed * Time.deltaTime, Space.World);


    }
}