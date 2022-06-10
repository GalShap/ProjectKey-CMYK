using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class horizAttack : StateMachineBehaviour
{
    private YellowGod yellow;
    // private EnemyHealth hl;
    [SerializeField] public float timerCounter = 5;
    [SerializeField] public float timerEnd = 10;
    [SerializeField] public float timerBlockWait = 1;
    private int size;
    private int counter = 0;
    private int counter2 = 0;
    private bool doReturn = false;

    private float timer = 0;
    
    private float timerBlock = 0;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        yellow = animator.GetComponent<YellowGod>();
        size = yellow.getLeftPil().Length;
        counter2 = size;
        yellow.leftBarrier.SetActive(false);
        yellow.rightBarrier.SetActive(false);
        yellow.HorizStart();
        // yellow.HorizMoveBlock(counter);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        yellow.resetBossActive();
        timer += Time.deltaTime;
        timerBlock += Time.deltaTime;
        if (timer >= timerCounter +size && counter >= size && !doReturn)
        {
            yellow.horizMoveBack(counter2);
            if (counter2 <= 0)
            {
                doReturn = true;
            }

            counter2--;

        }

        if (timer >= size + timerEnd && counter >= size && doReturn)
        {
            doReturn = false;
            counter = 0;
            animator.SetTrigger("coolDown");
        }
            
        else if (timerBlock >= timerBlockWait)
        {
            // counter++;
            timerBlock = 0;
            yellow.HorizMoveBlock(counter);
            counter++;
        }

        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        yellow.HorizEnd();
        animator.ResetTrigger("coolDown");
        counter = 0;
        timer = 0;
        timerBlock = 0;
        doReturn = false;
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
