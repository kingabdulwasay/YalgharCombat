using UnityEngine;

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
    public GameObject bloodSplash;

    public EnemyAI enemyPosition;

    public bool isAttacking;

    public string hitPart;

    public bool isDodging;



    void Start(){
        isAttacking = false;
        isDodging = false;
    }

    void Update()
    {
        Attack();

        Block();

        Crouch();

        Dodge();

        
    }


    public void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Transform cam = GetComponent<PlayerMovement>().cam;
            origin = cam.position;
            dir = cam.forward;

            WeaponSwitching weaponSwitching = GetComponent<WeaponSwitching>();

                if (Physics.Raycast(origin, dir, out hit, 20f, enemyLayer))
                {
                    Debug.Log("===== RAYCAST HIT =====");
                    Debug.Log("Hit Object Name: " + hit.collider.gameObject.name);
                    Debug.Log("Hit Transform Name: " + hit.transform.name);
                    Debug.Log("Hit Root Object: " + hit.transform.root.name);
                    Debug.Log("Hit Tag: " + hit.collider.tag);
                    Debug.Log("Hit Layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));

                    float distance = Vector3.Distance(transform.position, hit.point);
                    Debug.Log("Distance: " + distance);
                    Debug.Log("Weapon Type: " + weaponSwitching.currentType);

                    if (distance <= 1.75f)
                    {
                        enemyPosition = hit.collider.GetComponentInParent<EnemyAI>();
                        bleedingPoint = hit.point;
                        FacePlayerToHit();

                        if (weaponSwitching.currentType == WeaponSwitching.WeaponType.Meele)
                        {
                            hitPart = hit.collider.gameObject.name;
                            Debug.Log("Hit Part Used In Code: " + hitPart);
                            if (isCrouching)
                            {
                                GetComponent<PlayerAnimation>().AttackAnimation(6.0);
                            }else
                            {
                  

                                 if (hitPart == "Head")
                                {
                                    Debug.Log("HEAD ATTACK");
                                    GetComponent<PlayerAnimation>().AttackAnimation(1.0);
                                    hit.collider.GetComponentInParent<EnemyHealth>().TakeDamage(10f);
                                }
                                else if (hitPart == "Belly")
                                {
                                    Debug.Log("BELLY ATTACK");
                                    GetComponent<PlayerAnimation>().AttackAnimation(3.0);
                                    hit.collider.GetComponentInParent<EnemyHealth>().TakeDamage(5f);

                                }
                                else if (hitPart == "Left Leg" || hitPart == "Right Leg")
                                {
                                    Debug.Log("LEGS ATTACK");
                                    GetComponent<PlayerAnimation>().AttackAnimation(4.0);

                                }
                                else if (hitPart == "Right Hand" || hitPart == "Left Hand")
                                {
                                    Debug.Log("HAND ATTACK");
                                    GetComponent<PlayerAnimation>().AttackAnimation(2.0);
                                }
                                else
                                {
                                    Debug.Log("NO MATCHING BODY PART FOUND");
                                }
                                
                            }
                        }
                    }
                    else
                    {
                        if (weaponSwitching.currentType == WeaponSwitching.WeaponType.Ranged)
                        {
                            bleedingPoint = hit.point;
                            Debug.Log("RANGED THROW");
                            GetComponent<PlayerAnimation>().AttackAnimation(0);
                        }
                    }
                }
                else
                {
                    Debug.Log("RAYCAST DID NOT HIT ENEMY");
                }
            
        }
    }





    public void OnKnifeThrow(){
    GetComponent<AudioManager>().ThrowKnife();
    GetComponent<WeaponSwitching>().DisableWeapon();
}

public void OnKnifeHit(){
    GetComponent<AudioManager>().Cut();
    Bleeding();
    GetComponent<WeaponSwitching>().EnableWeapon();
}
     

public void ResetDodge(){
    isDodging = false;
    Debug.Log("Dodge and Dive Reset");
}



    void FacePlayerToHit(){
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
        if(Input.GetKeyDown(KeyCode.B)){   
            isBlocking = !isBlocking;
        } 
         if(Input.GetKey(KeyCode.B)){   
            GetComponent<PlayerAnimation>().HandleBlock(isBlocking);        
        }
    }

    void Crouch()
    {
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
            isBlocking = false;
            Debug.Log("Disabled");
    }

    public void Bleeding(){
            GameObject splash = Instantiate(bloodSplash, bleedingPoint, Quaternion.identity);
            Destroy(splash, 3f);
    }
    public void AttackEvent(){

                Bleeding();
                enemyPosition.GetHit();
                GetComponent<PlayerAnimation>().HandleRootMotion(false);
                GetComponent<CamSwitching>().FilmingwithoutAttacking();
        
    }


          void Dodge(){
            if(Input.GetKeyDown(KeyCode.Tab)){
                GetComponent<PlayerAnimation>().HandleDodge("Dive");
                isDodging  = true;
                Debug.Log("Diving");
            }else if (Input.GetKeyDown(KeyCode.Z)){
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
