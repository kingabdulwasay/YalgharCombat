using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    public void Start()
    {
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
    public void HandleJump(bool flag){
            animator.SetBool("Jump", flag);
    }

        public void HandleBlock(){
        
            animator.SetTrigger("Block");
    }

    public void HandleDodge(){   
        if(Input.GetKeyDown(KeyCode.Tab)){
            animator.SetTrigger("Dive");
        }else if(Input.GetKeyDown(KeyCode.UpArrow)){
            animator.SetTrigger("DodgeForward");
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
            animator.SetTrigger("HeadAttack");
            break;

            case 1.1:
            animator.SetTrigger("LeftHandAttack");
            break;

            case 1.2:
            animator.SetTrigger("RightHandAttack");
            break;

            case 2.0:
            animator.SetTrigger("BellyAttack");
            break;

            case 2.1:
            animator.SetTrigger("LeftLegAttack");
            break;

            case 2.2:
            animator.SetTrigger("RightLegAttack");
            break;

           

            default:
            break;
        }
    }
    public void GreatAttack(){
        animator.SetTrigger("GreatSlash");
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
