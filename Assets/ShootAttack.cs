using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttack : StateMachineBehaviour
{
    private YellowGod yellow;
    // private EnemyHealth hl;
    [SerializeField] public float timerCounter = 6;
    [SerializeField] public float timerCounterShoot = 2;
    [SerializeField] public float timerBlockWait = 1;
    private int size;
    private int counter = 0;

    private float timer = 0;
    private float timerBlock = 0;

    private float timerShoot = 0;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        yellow = animator.GetComponent<YellowGod>();
        size = yellow.getLeftPil().Length;
        yellow.HorizStart();
        counter = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        timerBlock += Time.deltaTime;
        timerShoot += Time.deltaTime;
        if (timer > size + timerCounter)
        {
            yellow.HorizMoveBack();
            animator.SetTrigger("coolDown");
        }
        else if (timerBlock >= timerBlockWait && timer < size)
        {
            timerBlock = 0;
            yellow.HorizMoveBlock(counter);
            counter++;
        }
        else if (timerShoot >= timerCounterShoot && timer > size)
        {
            timerShoot = 0;
            yellow.Shoot();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        yellow.HorizEnd();
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
