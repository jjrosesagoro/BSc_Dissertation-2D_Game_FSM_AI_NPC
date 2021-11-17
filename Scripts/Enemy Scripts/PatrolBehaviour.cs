using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour
{
    private int RandomLocation;
    public float speed;
    private LocationsPatrolled patrol;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        patrol = GameObject.FindGameObjectWithTag("LocationsPatrolled").GetComponent<LocationsPatrolled>(); /*creating a list of potential vectors to be 
        patrolled for the enemy to identify and follow*/

        RandomLocation = Random.Range(0, patrol.PointsOfPatrol.Length);
        /*calling the PointsOfPatrol method within an external class which is holding the details regarding the configured waypoints and 
        giving the range a value of 0 so that whilst the AI patrols from waypoint to waypoint there will be no preferred waypoint*/
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(animator.transform.position, patrol.PointsOfPatrol[RandomLocation].position) > 0.2f)
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, patrol.PointsOfPatrol[RandomLocation].position, speed * Time.deltaTime);
        }
         //transforming the position of the AI based on the random locations of the waypoints, by storing the range in an extremely finite float
        else
        {
            RandomLocation = Random.Range(0, patrol.PointsOfPatrol.Length);
            /*calling the PointsOfPatrol method within an external class which is holding the details regarding the configured waypoints and 
        giving the range a value of 0 so that whilst the AI patrols from waypoint to waypoint there will be no preferred waypoint*/
        }

        if (Input.GetKeyDown(KeyCode.Space)) { //fetches the input from the spacebar keypress to use as a trigger
            animator.SetBool("isPatrolling", false); //setting the pool parameter for the animator to false upon key press
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
