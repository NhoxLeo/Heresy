using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour
{
    public float turnTimeS = 0;
    public float turnTimeE = 0.2f;

    public float speed;
    public float turnSpeed;


    void FixedUpdate()
    {
        PlayerMovement();
    }


    void PlayerMovement()
    {
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            Vector3 movementFace = new Vector3(movement.x, 0.0f, movement.z);

            movement = Camera.main.transform.TransformDirection(movement);
            movement.y = 0f;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * turnSpeed);

            transform.Translate(movement.normalized * speed * Time.deltaTime, Space.World);
        }

        
    }
}