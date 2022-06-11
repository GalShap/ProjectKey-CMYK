using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackByShooting : StateMachineBehaviour
{
    [SerializeField] private float timerCounter = 6;
    [SerializeField] private float timerToShoot = 1;
    private float timerCoolDown = 0;
    private float timerShoot = 0;
    // public GameObject m_Projectile; // this is a reference to your projectile prefab
    // public Transform m_SpawnTransform; // this is a reference to the transform where the prefab will spawn
    private GameObject _player;
    private Rigidbody2D rb;
    private MagentaGod red;
    [SerializeField] private int oddsToGetBlue = 5;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        red = animator.GetComponent<MagentaGod>();
        rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timerShoot += Time.deltaTime;
        timerCoolDown += Time.deltaTime;
        if (timerShoot >= timerToShoot)
        {
            red.LookAtPlayer();
            timerShoot = 0;
            int k = Random.Range(0, oddsToGetBlue);
            if (k == 1)
            {
                red.ShootBlue();
            }
            else
            {
                red.Shoot();
            }
            // animator.SetTrigger(k == 1 ? "blueWall" : "shoot");
            
        }
        if (timerCoolDown >= timerCounter)
        {
            animator.SetTrigger("rel");
            timerCoolDown = 0;
            animator.SetTrigger("coolDown");
            
        }
        

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("coolDown");
        // animator.ResetTrigger("left");
        // animator.ResetTrigger("right");
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
