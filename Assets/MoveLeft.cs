using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MoveLeft : StateMachineBehaviour
{
    private Transform left;
    private GameObject player;
    private Rigidbody2D rb;
    private MagentaGod red;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        red = animator.GetComponent<MagentaGod>();
        rb = animator.GetComponent<Rigidbody2D>();
        left = red.left;
        red.platformRight.SetActive(true);
        red.platformLeft.SetActive(false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 target = new Vector2(left.transform.position.x, rb.transform.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, red.speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        if (newPos.x == left.position.x)
        {
            animator.SetTrigger("coolDown");
        }
    }
    

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // animator.ResetTrigger("left");
        animator.ResetTrigger("coolDown");
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
