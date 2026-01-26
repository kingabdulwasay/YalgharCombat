using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    // float currentAttack;

    public void Start()
    {
        // currentAttack = 0;
        animator = GetComponent<Animator>();
    }



    public void MovementAnimations()
    {

        bool running = Input.GetKey(KeyCode.LeftShift);
        bool crouching = GetComponent<PlayerCombat>().isCrouching;

        animator.SetBool("WalkForward",(Input.GetKey(KeyCode.W)));
        animator.SetBool("WalkBackward",(Input.GetKey(KeyCode.S)));
        animator.SetBool("WalkLeft", (Input.GetKey(KeyCode.A)));
        animator.SetBool("WalkRight", (Input.GetKey(KeyCode.D)));
       // animator.SetBool("CrouchIdle", (Input.GetKey(KeyCode.C)));
        animator.SetBool("RunForward",running);

        animator.SetBool("CrouchForward", (Input.GetKey(KeyCode.W) && crouching));
        animator.SetBool("CrouchBackward", (Input.GetKey(KeyCode.S) && crouching));
        animator.SetBool("CrouchLeft", (Input.GetKey(KeyCode.A) && crouching));
        animator.SetBool("CrouchRight", (Input.GetKey(KeyCode.D) && crouching));
    }
    public void HandleJump(){
            animator.SetTrigger("Jump");
    }

    public void HandleCrouch(bool flag)
    {
        animator.SetBool("CrouchIdle", flag);
    }


    public void HandleBlock(bool flag){
            animator.SetBool("Block", flag);
    }

    public void HandleDodge(string type){   

            animator.SetTrigger(type);

            EnableRootMotionEvent();
        
    }

   public void HandleRootMotion(bool flag){
        animator.applyRootMotion = flag;
    }

    public void AttackAnimation(double currentAttack){
        switch(currentAttack){
            case 0:

            animator.SetTrigger("Throw");
            break;
            case 1.0:
            // HandleRootMotion(true);
            // GetComponent<CamSwitching>().FilmingwithAttacking();
            animator.SetTrigger("HeadAttack");
            break;

            case 2.0:
        //    HandleRootMotion(true);
        //     GetComponent<CamSwitching>().FilmingwithAttacking();
            animator.SetTrigger("HandsAttack");
            break;

            case 3.0:
            // HandleRootMotion(true);
            // GetComponent<CamSwitching>().FilmingwithAttacking();
            animator.SetTrigger("BellyAttack");
            break;
  
            case 4.0:
            // HandleRootMotion(true);
            // GetComponent<CamSwitching>().FilmingwithAttacking();
            animator.SetTrigger("BottomAttack");
            break;

            // case 5.0:
            // animator.SetTrigger("punch");
            // break;

            case 6.0:
            // HandleRootMotion(true);
            // GetComponent<CamSwitching>().FilmingwithAttacking();
            animator.SetTrigger("Legs");
            break;


            default:
            break;
        }
        // if(currentAttack > 3){
        //     currentAttack = 1;
        // }
        //     animator.SetTrigger("Attack"+currentAttack++);
    }
   
    public void DisableRootMotionEvent(){
         animator.applyRootMotion = false;
         GetComponent<CamSwitching>().FilmingwithoutAttacking();
    }

     public void EnableRootMotionEvent(){
         animator.applyRootMotion = true;
        //  GetComponent<CamSwitching>().FilmingwithAttacking();
    }


    public void DamageAnimation(){
        animator.SetTrigger("Hit");
    }

    public void StopMovementAnimations(){
        animator.SetBool("WalkForward", false);
        animator.SetBool("RunForward", false);
        animator.SetBool("WalkBackward", false);
        animator.SetBool("RunBackward", false);
        
    }
}
