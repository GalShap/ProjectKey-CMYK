using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopStateYellow : StateMachineBehaviour
{
    private YellowGod yellow;
    // private EnemyHealth hl;
    [SerializeField] public float timerCounter = 5;
    [SerializeField] public float timerBlockWait = 1;
    private int size;
    private int counter;

    private float timer = 0;
    
    private float timerBlock = 0;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        yellow = animator.GetComponent<YellowGod>();
        counter = 0;
        yellow.leftBarrier.SetActive(false);
        yellow.rightBarrier.SetActive(false);
        yellow.PillarEnd();
        yellow.HorizEnd();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
