using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRight : StateMachineBehaviour
{
    private Transform right;
    private GameObject _player;
    private Rigidbody2D rb;
    private MagentaGod red;
    private bool unactive;
    [SerializeField] private float timerCounter = 3;

    private float timer = 0;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        red = animator.GetComponent<MagentaGod>();
        rb = animator.GetComponent<Rigidbody2D>();
        right = red.right;
        // red.platformLeft.SetActive(true);
        // red.platformRight.SetActive(false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        // if (timer >= timerCounter)
        // {
        //     red.platformRight.SetActive(false);
        //     unactive = true;
        // }
        // else if(!unactive)
        // {
        //     timer += Time.deltaTime;
        // }
        
            Vector2 target = new Vector2(right.transform.position.x, rb.transform.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, red.speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            if (newPos.x == right.position.x)
            {
                animator.SetTrigger("coolDown");
            }
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("coolDown");
        animator.ResetTrigger("left");
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
