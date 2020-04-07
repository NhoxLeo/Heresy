using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{

    public float speed;


    public void Start()
    {
        
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
        
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0f;
        transform.LookAt(movement + transform.position);

        transform.Translate(movement.normalized * speed * Time.deltaTime, Space.World);


    }
}