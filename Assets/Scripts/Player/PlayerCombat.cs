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

    public bool isAttacking;

    void Start(){
    disabledCrosshair.SetActive(false);
    enabledCrosshair.SetActive(true);
        isAttacking = false;
    }

    void Update()
    {
        Attack();
        Dodging();
        Block();

        
    }

    public void Attack(){
        if(Input.GetButtonDown("Fire1")){
           
            Transform cam = GetComponent<PlayerMovement>().cam;
            origin = cam.position;
            dir = cam.forward;


            if (Physics.Raycast(origin, dir, out hit, 20f, enemyLayer)){
            
                WeaponSwitching weaponSwitching = GetComponent<WeaponSwitching>();
                float distance = Vector3.Distance(transform.position, hit.point);
                if(distance < 1f){
                    bleedingPoint = hit.point;
                    FacePlayerToHit();
                    if(weaponSwitching.currentType == WeaponSwitching.WeaponType.None){
                        GetComponent<PlayerAnimation>().HandleRootMotion(true);
                        GetComponent<CamSwitching>().FilmingwithAttacking();
                        GetComponent<PlayerAnimation>().AttackAnimation(5.0);
                    }else if (weaponSwitching.currentType == WeaponSwitching.WeaponType.Meele){
                        GetComponent<PlayerAnimation>().HandleRootMotion(true);
                        GetComponent<CamSwitching>().FilmingwithAttacking();
                    if(hit.collider.name == "Head"){
                    GetComponent<PlayerAnimation>().AttackAnimation(1.0);
                        }else if(hit.collider.name == "Left Hand"){
                    GetComponent<PlayerAnimation>().AttackAnimation(1.0);
                        }else if(hit.collider.name == "Right Hand"){
                    GetComponent<PlayerAnimation>().AttackAnimation(1.0);
                        }else if(hit.collider.name == "Belly"){
                    GetComponent<PlayerAnimation>().AttackAnimation(3.0);
                        }else if(hit.collider.name == "Left Leg"){
                    GetComponent<PlayerAnimation>().AttackAnimation(4.0);
                        }else if(hit.collider.name == "Right Leg"){
                    GetComponent<PlayerAnimation>().AttackAnimation(4.0);
                    }else if(weaponSwitching.currentType == WeaponSwitching.WeaponType.Ranged){
                        GetComponent<PlayerAnimation>().HandleRootMotion(true);
                        GetComponent<CamSwitching>().FilmingwithAttacking();
                        GetComponent<PlayerAnimation>().AttackAnimation(6.0);
                    }
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

    void Dodging(){
            GetComponent<PlayerAnimation>().HandleDodge();
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
        // if(Time.time - lastAttackTime > coolDownTime){
        //     lastAttackTime = Time.time;  
        //     Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
        //     foreach(var hit in hits){
        //         Vector3 closestPoint = hit.GetComponent<Collider>().ClosestPoint(attackPoint.position);
        //         GameObject splash = Instantiate(bloodSplash, closestPoint, Quaternion.identity);
        //         Destroy(splash, 3f);
        //     }
        // }
                Bleeding();
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
