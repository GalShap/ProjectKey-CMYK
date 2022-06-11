using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowGodStandState : StateMachineBehaviour
{
    private GameObject _player;
    private Rigidbody2D rb;
    
    private YellowGod yellow;
    private EnemyHealth hl;
    [SerializeField] public float timerCounter = 5;

    private float timer = 2;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // player = GameObject.FindGameObjectWithTag("Player");
        yellow = animator.GetComponent<YellowGod>();
        _player = yellow.player;
        hl = animator.GetComponent<EnemyHealth>();
        rb = animator.GetComponent<Rigidbody2D>();
        yellow.healtChange();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        yellow.resetBossActive();
        yellow.LookAtPlayer();
        timer += Time.deltaTime;
        if (timer >= timerCounter)
        {
            timer = 0;
            int k = Random.Range(0, 3);
            switch (k)
            {
                case 0 :
                    animator.SetTrigger("attackOneStairs");
                    break;
                case 1:
                    animator.SetTrigger("upAndDownAttack");
                    break;
                case 2:
                    animator.SetTrigger("shoot");
                    break;
                default:
                    animator.SetTrigger("attackOneStairs");
                    break;
                
            }
            // rb.AddForce(Vector2.up);
            // animator.SetTrigger( "shoot");
        }
        
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
