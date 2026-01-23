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

        
        animator.SetBool("WalkForward",(Input.GetKey(KeyCode.W)));
        animator.SetBool("WalkBackward",(Input.GetKey(KeyCode.S)));
        animator.SetBool("WalkLeft", (Input.GetKey(KeyCode.A)));
        animator.SetBool("WalkRight", (Input.GetKey(KeyCode.D)));
        
        animator.SetBool("RunForward",running);
 
        

    }
    public void HandleJump(){
            animator.SetTrigger("Jump");
    }

        public void HandleBlock(bool flag){
            animator.SetBool("Block", flag);
    }

    public void HandleDodge(){   
        if(Input.GetKeyDown(KeyCode.Tab)){
            animator.SetTrigger("Dive");
        }
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
            animator.SetTrigger("HandsHeadAttack");
            break;

            case 2.0:
            animator.SetTrigger("HandsAttack");
            break;

            case 3.0:
            animator.SetTrigger("BellyAttack");
            break;
  
            case 4.0:
            animator.SetTrigger("BottomAttack");
            break;

            case 5.0:
            animator.SetTrigger("punch");
            break;

            case 6.0:
            animator.SetTrigger("Stabb");
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
