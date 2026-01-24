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

        
    }

    public void Attack(){
        if(Input.GetButtonDown("Fire1")){
           
            Transform cam = GetComponent<PlayerMovement>().cam;
            origin = cam.position;
            dir = cam.forward;


                WeaponSwitching weaponSwitching = GetComponent<WeaponSwitching>();
  if(Time.time - lastAttackTime > coolDownTime){
            lastAttackTime = Time.time;  
            if (Physics.Raycast(origin, dir, out hit, 20f, enemyLayer)){
            
                float distance = Vector3.Distance(transform.position, hit.point);

                if(distance < 1.5f){
                    enemyPosition = hit.collider.transform;
                    bleedingPoint = hit.point;
                    FacePlayerToHit();
                   
                if (weaponSwitching.currentType == WeaponSwitching.WeaponType.Meele){


                     Debug.Log(hit.collider.name);

                    if(hit.collider.name == "Belly") GetComponent<PlayerAnimation>().AttackAnimation(3.0);
                     
                    }
                }else{
                Debug.Log("Current Weapon Type: " + weaponSwitching.currentType);

                if(weaponSwitching.currentType == WeaponSwitching.WeaponType.Ranged){ 
                    Vector3 closestPoint = hit.collider.ClosestPoint(attackPoint.position);
                    bleedingPoint = closestPoint;
                    Debug.Log($"Closest Point: x={closestPoint.x}, y={closestPoint.y}, z={closestPoint.z}");
                    GetComponent<PlayerAnimation>().AttackAnimation(0);
                }
                  
        }
              
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
