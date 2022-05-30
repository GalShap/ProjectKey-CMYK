using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStand : StateMachineBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    
    private MagentaGod red;
    private EnemyHealth hl;
    [SerializeField] public float timerCounter = 5;

    private float timer = 2;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // player = GameObject.FindGameObjectWithTag("Player");
        red = animator.GetComponent<MagentaGod>();
        player = red.getPlayer();
        hl = animator.GetComponent<EnemyHealth>();
        rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        red.LookAtPlayer();
        red.healtChange();
        timer += Time.deltaTime;
        if (timer >= timerCounter)
        {
            timer = 0;
            // rb.AddForce(Vector2.up);
            animator.SetTrigger( "shoot");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("shoot");
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