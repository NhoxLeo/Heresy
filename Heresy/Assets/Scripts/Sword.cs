using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    Collider blade;
    // Start is called before the first frame update
    void Start()
    {
        blade = gameObject.GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void attackColliderOn()
    {
        blade.enabled = true;
    }
    public void attackColliderOff()
    {
        blade.enabled = false;
    }
}
