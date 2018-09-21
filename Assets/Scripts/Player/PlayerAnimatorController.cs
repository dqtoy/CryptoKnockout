﻿using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{

    public bool DEBUG = false; // REMOVE ME LATER
    public GameObject groundParticles;

    bool IDLE = false;

    public enum ANIMATION_STATE {IDLE, BLOCK, DEAD, JUMP, HURT, LOWPUNCH, LOWKICK, HIGHPUNCH, HIGHKICK, SPECIALATTACKONE, DUCK , LAND};


    Player player;
    /* Animator Object */

    Animator playerAnimator;

    /* State Vars */

    ANIMATION_STATE currentAnimationState;
    Rigidbody2D rgbody;

    void Update()
    {
        playerAnimator.SetFloat("SPEED", Mathf.Abs(rgbody.velocity.x));
        playerAnimator.SetBool("GROUNDED", player.IsGrounded());
        playerAnimator.SetBool("ALIVE", player.IsAlive());
        playerAnimator.SetBool("BLOCKING", player.IsBlocking());
        playerAnimator.SetBool("DUCKING", player.IsDucking());

        if(player.IsGrounded() && Mathf.Abs(rgbody.velocity.x) > 1)
        {
            groundParticles.SetActive(true);
        } else
        {
            groundParticles.SetActive(false);
        }
        

    }

    void Start()
    {
        UpdateController();
        SetAnimationState(ANIMATION_STATE.IDLE);
        player = GetComponent<Player>();
        rgbody = GetComponent<Rigidbody2D>();
    }

    /* Animation State */
    public ANIMATION_STATE GetCurrentAnimationState()
    {
        return currentAnimationState;
    }

    public void SetAnimationState(ANIMATION_STATE animationState)
    {
            switch (animationState)
            {
                case ANIMATION_STATE.DEAD:
                    playerAnimator.SetTrigger("DIE");
                    return;

                case ANIMATION_STATE.DUCK:
                    if(!player.IsDucking()) playerAnimator.SetTrigger("DUCK");
                    return;

                case ANIMATION_STATE.IDLE:
                    playerAnimator.SetTrigger("GROUNDED");
                    return;

                case ANIMATION_STATE.JUMP:
                    playerAnimator.SetTrigger("JUMP");
                    return;

                case ANIMATION_STATE.LAND:
                    playerAnimator.SetTrigger("LAND");
                    return;

                case ANIMATION_STATE.HURT:
                    playerAnimator.SetTrigger("HURT");
                    return;

                case ANIMATION_STATE.BLOCK:
                    if (!playerAnimator.GetBool("BLOCKING") && !player.AnyLock())
                    {
                        playerAnimator.SetTrigger("BLOCK");
                        player.Locks().Lock(ActionLocks.Locks.BLOCK);
                    }
                    return;

                case ANIMATION_STATE.LOWPUNCH:
                    return;

                case ANIMATION_STATE.LOWKICK:
                    return;

                case ANIMATION_STATE.HIGHPUNCH:
                    if (!player.AnyLock())
                    {
                        playerAnimator.SetTrigger("PUNCHING");
                        player.Locks().Lock(ActionLocks.Locks.PUNCH);
                    }
                    return;

                case ANIMATION_STATE.HIGHKICK:
                    if (!player.AnyLock())
                    {
                        playerAnimator.SetTrigger("KICKING");
                        player.Locks().Lock(ActionLocks.Locks.KICK);
                    }
                    return;

                case ANIMATION_STATE.SPECIALATTACKONE:
                    if (!player.AnyLock())
                    {
                        playerAnimator.SetTrigger("SPECIAL");
                        player.Locks().Lock(ActionLocks.Locks.SUPER);
                    }
                    return;

                default:
                    playerAnimator.SetTrigger("GROUNDED");
                    return;
            }
       
    }


    public void UpdateController()
    {
        playerAnimator = GetComponent<Animator>();
    }

}
