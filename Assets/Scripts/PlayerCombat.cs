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
    public GameObject sword;
    public GameObject bow;

    void Start(){
        sword.SetActive(true);
        bow.SetActive(false);
    }

    void Update()
    {
        Attack();
        Dodging();
        Block();

        if(Input.GetButtonDown("Fire2")){
            GetComponent<PlayerAnimation>().GreatAttack();
        }
    }

    public void Attack(){
        if(Input.GetButtonDown("Fire1")){
           
            
            Transform cam = GetComponent<PlayerMovement>().cam;
            origin = cam.position;
            dir = cam.forward;


            if (Physics.Raycast(origin, dir, out hit, 20f, enemyLayer)){
                float distance = Vector3.Distance(transform.position, hit.point);
                if(distance < 1.5f){
                    bow.SetActive(false);
                    sword.SetActive(true);
                    FacePlayerToHit();
                    if(hit.collider.name == "Head"){
                    GetComponent<PlayerAnimation>().AttackAnimation(1.0);
                        }else if(hit.collider.name == "Left Hand"){
                    GetComponent<PlayerAnimation>().AttackAnimation(1.1);
                        }else if(hit.collider.name == "Right Hand"){
                    GetComponent<PlayerAnimation>().AttackAnimation(1.2);
                        }else if(hit.collider.name == "Belly"){
                    GetComponent<PlayerAnimation>().AttackAnimation(2.0);
                        }else if(hit.collider.name == "Left Leg"){
                    GetComponent<PlayerAnimation>().AttackAnimation(2.1);
                        }else if(hit.collider.name == "Right Leg"){
                    GetComponent<PlayerAnimation>().AttackAnimation(2.2);
                    }
            }else{
                    bow.SetActive(true);
                    sword.SetActive(false);
                    Vector3 closestPoint = hit.collider.ClosestPoint(attackPoint.position);
                    bleedingPoint = closestPoint;
                    Debug.Log($"Closest Point: x={closestPoint.x}, y={closestPoint.y}, z={closestPoint.z}");
                    GetComponent<PlayerAnimation>().AttackAnimation(0);
                    // Instantiate(bloodSplash, closestPoint, Quaternion.identity);
                }
                  
        }

    } 

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

    public void BleedingEvent(){
            GameObject splash = Instantiate(bloodSplash, bleedingPoint, Quaternion.identity);
            Destroy(splash, 3f);
    }
    public void AttackEvent(){
        if(Time.time - lastAttackTime > coolDownTime){
            lastAttackTime = Time.time;  
            Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
            foreach(var hit in hits){
                Vector3 closestPoint = hit.GetComponent<Collider>().ClosestPoint(attackPoint.position);
                GameObject splash = Instantiate(bloodSplash, closestPoint, Quaternion.identity);
                Destroy(splash, 3f);
            }
        }
    }

    public void HeavyAttackEvent(){
             bow.SetActive(false);
            sword.SetActive(true);
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
