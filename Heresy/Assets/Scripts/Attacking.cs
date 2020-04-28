using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : StateMachineBehaviour
{
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

     //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && animator.GetBool("Attack"))
        {    
            animator.SetBool("Attack2",true);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && animator.GetBool("Attack") == false && animator.GetBool("Attack2"))
        {
             animator.SetBool("Attack3", true);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && animator.GetBool("Attack2") == false && animator.GetBool("Attack3")) 
        {
            animator.SetBool("Attack4", true);
        }        
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && animator.GetBool("Attack3") == false && animator.GetBool("Attack4")) 
        {
            animator.SetBool("Attack5", true);
        }


        


    }

     //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {



    }



}
