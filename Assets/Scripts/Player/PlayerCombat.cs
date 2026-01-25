using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public LayerMask enemyLayer;
    public float attackRange = .5f;
    public Transform attackPoint;
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

    public GameObject enabledCrosshair;
    public GameObject disabledCrosshair;
    public Transform enemyPosition;

    public bool isAttacking;

    void Start(){
    disabledCrosshair.SetActive(false);
    enabledCrosshair.SetActive(true);
        isAttacking = false;
    }

    void Update()
    {
        Attack();

        Block();

        Crouch();
        
    }


    public void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Transform cam = GetComponent<PlayerMovement>().cam;
            origin = cam.position;
            dir = cam.forward;

            WeaponSwitching weaponSwitching = GetComponent<WeaponSwitching>();

            if (Time.time - lastAttackTime > coolDownTime)
            {
                lastAttackTime = Time.time;

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

                    if (distance < 1.5f)
                    {
                        enemyPosition = hit.collider.transform;
                        bleedingPoint = hit.point;
                        FacePlayerToHit();

                        if (weaponSwitching.currentType == WeaponSwitching.WeaponType.Meele)
                        {
                            string hitPart = hit.collider.gameObject.name;
                            Debug.Log("Hit Part Used In Code: " + hitPart);
                            if (isCrouching)
                            {
                                GetComponent<PlayerAnimation>().AttackAnimation(6.0);
                            }

                            else
                            {
                                if (hitPart == "Head")
                                {
                                    Debug.Log("HEAD ATTACK");
                                    GetComponent<PlayerAnimation>().AttackAnimation(1.0);
                                }
                                else if (hitPart == "Belly")
                                {
                                    Debug.Log("BELLY ATTACK");
                                    GetComponent<PlayerAnimation>().AttackAnimation(3.0);
                                }
                                else if (hitPart == "Legs")
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
                            Vector3 closestPoint = hit.collider.ClosestPoint(attackPoint.position);
                            bleedingPoint = closestPoint;
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
     


// void TempAttack(){
//     if(Input.GetButtonDown("Fire1")){
//         isAttacking = true;
//         GetComponent<PlayerAnimation>().HandleRootMotion(true);
//         GetComponent<PlayerAnimation>().StopMovementAnimations();
//          GetComponent<CamSwitching>().FilmingwithAttacking();
//         GetComponent<PlayerAnimation>().AttackAnimation();
//     }
//         // isAttacking = false;
// }


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
       
            // Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
            // foreach(var hit in hits){
            //     hit.GetComponent<EnemyAI>().GetHit();
            //     Vector3 closestPoint = hit.GetComponent<Collider>().ClosestPoint(attackPoint.position);
                // GameObject splash = Instantiate(bloodSplash, closestPoint, Quaternion.identity);
                // Destroy(splash, 3f);
            // }

                float distance = Vector3.Distance(transform.position, enemyPosition.position);
                Debug.Log(distance);
                Bleeding();
                GetComponent<AudioManager>().Damage();
                GetComponent<PlayerAnimation>().HandleRootMotion(false);
                GetComponent<CamSwitching>().FilmingwithoutAttacking();
        
    }

    public void HeavyAttackEvent(){
            //  bow.SetActive(false);
            // sword.SetActive(true);
            Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
            foreach(var hit in hits){
                
                Vector3 closestPoint = hit.GetComponent<Collider>().ClosestPoint(attackPoint.position);
                GameObject splash = Instantiate(bloodSplash, closestPoint, Quaternion.identity);
                Destroy(splash, 3f);
                hit.GetComponent<EnemyAI>().Fall();
            }   
    }
   

    void OnDrawGizmosSelected(){
        if (attackPoint == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
