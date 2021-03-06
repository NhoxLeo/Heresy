﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraBehaviour : MonoBehaviour
{

    public Transform target;

    public Vector3 offSet;

    public Transform pivot;

    public float SmoothFactor = 0.5f;

    public float rotateSpeed = 5.0f;

    public bool rotate = false;
    
    public GameObject pauseMenu;
    public GameObject deathMenu;
    // Start is called before the first frame update
    void Start()
    {
        
        target = GameObject.Find("PlayerTarget").transform;

        offSet = transform.position - target.position;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Continue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !deathMenu.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }


    void FixedUpdate()
    {
        Quaternion camTurnAngleX = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotateSpeed, Vector3.up);
        Quaternion camTurnAngleY = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * rotateSpeed, Vector3.right);

        offSet = camTurnAngleX * offSet;
        offSet = camTurnAngleY * offSet;

        Vector3 newPos = target.position + offSet;

        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

        transform.Rotate(Mathf.Clamp(camTurnAngleY.x, -20, 20), 0, 0);

        if (transform.position.y <= target.position.y - 2)
        {

            Debug.Log("ahhhhhhh");

        }
           
        if (rotate)
            {
                transform.LookAt(target);

            }

        if(PlayerController.lockedOn == true)
        {
            
        }

    }


}
    

