using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBehaviour : StateMachineBehaviour
{
    private Transform PositionOfPlayer;
    public float speed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PositionOfPlayer = GameObject.FindGameObjectWithTag("Player").transform; //allowing the AI to identify the player based on its tag
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, PositionOfPlayer.position, speed * Time.deltaTime);
        /*transitioning the animation state based on the direction it's moving toward which is the position of the player. 
        And calculaiting speed based on the speed and position of the player*/

        if (Input.GetKeyDown(KeyCode.Space)) //fetches the input from the spacebar keypress to use as a trigger
        {
            animator.SetBool("isChasing", false); //setting the pool parameter for the animator to false upon key press
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetBool("isPatrolling", true);
        }
    }

    

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
