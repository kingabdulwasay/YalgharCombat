using UnityEngine;
using Photon.Pun;

public class PlayerCombat : MonoBehaviour
{
    public LayerMask enemyLayer;
    private Vector3 bleedingPoint;
    Vector3 origin;
    Vector3 dir;
    public float rotationSpeed = 10f;  
    RaycastHit hit;
    public bool isBlocking = false;
    public bool isCrouching = false;

    float lastAttackTime;
    public float coolDownTime = 1.1f;


    public bool isAttacking;

    public string hitPart;

    public bool isDodging;

    PhotonView view;


    void Start(){
        view = GetComponent<PhotonView>();
        if(!view.IsMine) return;
        isAttacking = false;
        isDodging = false;
    }

    void Update()
    {
        if(!view.IsMine) return;
  
        Attack();

        Block();

        Crouch();

        Dodge();

        HeavyAttack();

    }


    public void Attack(){
        if(!view.IsMine) return;

        if (Input.GetButtonDown("Fire1"))
        {
            Transform cam = GetComponent<PlayerMovement>().cam;
            origin = cam.position;
            dir = cam.forward;


                if (Physics.Raycast(origin, dir, out hit, 20f, enemyLayer))
                {

                    float distance = Vector3.Distance(transform.position, hit.point);
                    Debug.Log("Distance: " + distance);

                    if (distance <= 1.75f)
                    {
                        FacePlayerToHit();

                        
                            hitPart = hit.collider.gameObject.name;
                            Debug.Log("Hit Part Used In Code: " + hitPart);
                            
                  

                                 if (hitPart == "Head")
                                
                                    GetComponent<PlayerAnimation>().AttackAnimation(1.0);
                                
                                else if (hitPart == "Belly")
                                
                                    GetComponent<PlayerAnimation>().AttackAnimation(3.0);

                                
                                else if (hitPart == "Left Leg" || hitPart == "Right Leg")
                                
                                    GetComponent<PlayerAnimation>().AttackAnimation(4.0);

                                
                                else if (hitPart == "Right Hand" || hitPart == "Left Hand")
                                
                                    GetComponent<PlayerAnimation>().AttackAnimation(2.0);
                                
                                else
                                
                                    Debug.Log("NO MATCHING BODY PART FOUND");
                                
                                
                            
                        
                    }
                    else
                    {
                        
                            Debug.Log("RANGED THROW");
                            GetComponent<PlayerAnimation>().AttackAnimation(0);
                        
                    }
                }
                else
                {
                    Debug.Log("RAYCAST DID NOT HIT ENEMY");
                }
            
        }
    }


public void HeavyAttack(){
        if(!view.IsMine) return;

    if(Input.GetButtonDown("Fire2")){
        PlayerAnimation animations = GetComponent<PlayerAnimation>();
        animations.EnableRootMotionEvent();
        animations.GreatAttack();
    }
}



public void OnThrow(){
        if(!view.IsMine) return;

    GetComponent<AudioManager>().ThrowKnife();
    GetComponent<WeaponSwitching>().DisableWeapon();
}

public void OnHit(){
        if(!view.IsMine) return;

    GetComponent<AudioManager>().Cut();
    GetComponent<WeaponSwitching>().EnableWeapon();
}
     

public void ResetDodge(){
        if(!view.IsMine) return;

    isDodging = false;
    Debug.Log("Dodge and Dive Reset");
    PlayerAnimation animations = GetComponent<PlayerAnimation>();
    animations.DisableRootMotionEvent();
}

    void FacePlayerToHit(){
        if(!view.IsMine) return;

        Vector3 direction = hit.point - transform.position;
        direction.y = 0f;
         if (direction != Vector3.zero){
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void Block(){
        if(!view.IsMine) return;

        if(Input.GetKeyDown(KeyCode.B)){   
            isBlocking = !isBlocking;
        } 
         if(Input.GetKey(KeyCode.B)){   
            GetComponent<PlayerAnimation>().HandleBlock(isBlocking);        
        }
    }

    void Crouch(){
        if(!view.IsMine) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
        }
        if (Input.GetKey(KeyCode.C))
        {
            GetComponent<PlayerAnimation>().HandleCrouch(isCrouching);
        }
    }

    public void DisableBlock(){
        if(!view.IsMine) return;

            isBlocking = false;
            Debug.Log("Disabled");
    }

  

public void AttackEvent()
{
    if (!view.IsMine) return;

    PhotonView target = hit.collider.GetComponentInParent<PhotonView>();
    if (target == null) return;

    Debug.Log("I am " + view.ViewID + "and hit: " + target.ViewID);
    // view.RPC(
    //     "TakeDamage",
    //     target.Owner,
    //     10f,
    //     hit.point,
    //     hitPart
    // );
}




    void Dodge(){
        if(!view.IsMine) return;

            if(Input.GetKeyDown(KeyCode.Tab)){
                PlayerAnimation animations = GetComponent<PlayerAnimation>();
                animations.EnableRootMotionEvent();
                GetComponent<PlayerAnimation>().HandleDodge("Dive");
                isDodging  = true;
                Debug.Log("Diving");
            }else if (Input.GetKeyDown(KeyCode.Z)){
                  PlayerAnimation animations = GetComponent<PlayerAnimation>();
                animations.EnableRootMotionEvent();
                GetComponent<PlayerAnimation>().HandleDodge("Dodge");
                isDodging  = true;
                Debug.Log("Dodging");
            }else if (Input.GetKeyDown(KeyCode.Space)){
                GetComponent<PlayerAnimation>().HandleJump();
                isDodging  = true;
                Debug.Log("Dodging");
            }

    }
 

   
}
