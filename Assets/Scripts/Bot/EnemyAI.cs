using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class EnemyAI : MonoBehaviour
{
    Transform targetPlayer;

    NavMeshAgent agent;
    Animator animator;

    public float rotationSpeed = 10f;  

    public float detectRange;
    public float attackRange;

    public Transform attackPoint;
    public float attackPointRange = .5f;

    float lastAttackTime;
    public float coolDownTime = 1.1f;

    public LayerMask playerLayer;
    public GameObject bloodSplash;

    bool isDisable = false;

    RaycastHit hit;


 
    void Start(){
        agent  = GetComponent<NavMeshAgent>();  
        animator = GetComponent<Animator>();
    }

    void Update(){
        if(!PhotonNetwork.IsMasterClient) return;
        
        FindClosestPlayer();
        if(targetPlayer == null) return;
        float distance = Vector3.Distance(transform.position, targetPlayer.position);
        agent.updateRotation = true;
  
        if(distance <= attackRange){
            agent.ResetPath();
             animator.SetBool("Run", false);
           
             if(Time.time - lastAttackTime > coolDownTime){
                lastAttackTime = Time.time;
                // animator.SetTrigger("Attack"+Random.Range(1,4));
                Attack();
            }

        }else if(distance <= detectRange){

            Chase();
        }else{
            Idle();
        }

    }

    void Chase(){
        if(!isDisable){
        agent.SetDestination(targetPlayer.position);
        animator.SetBool("Run", true);
        }
    }

    void Idle(){
        agent.ResetPath();
        animator.SetBool("Run", false);
    }

    public void Fall(){
        isDisable = true;
        animator.SetTrigger("Fall");
    }

    public void AfterStandup(){
        isDisable = false;
    }

    public void GetHit(){
        animator.SetTrigger("Hit");
    }

void Attack()
{
    float attackRange = 1.5f;
    float radius = 0.5f;

    Debug.Log("Attack Played");
    Vector3 origin = attackPoint.position;
    Vector3 direction = attackPoint.forward;
    
    if (Physics.SphereCast(origin, radius, direction,  out hit, attackRange, playerLayer))
    {
        FacePlayerToHit();

        Debug.Log(hit.collider.name);

        PhotonView targetPV = hit.collider.GetComponent<PhotonView>();

if (targetPV != null)
{
    targetPV.RPC(
        "TakeDamage",
        targetPV.Owner,   // 🔥 THIS is the key
        10f
    );
}



    }
}


    void FacePlayerToHit(){


        Vector3 direction = hit.collider.transform.position - transform.position;
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


        void OnDrawGizmosSelected(){
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(attackPoint.position, attackPointRange);
    }

   void FindClosestPlayer(){
    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

    float shortestDistance = Mathf.Infinity;
    Transform nearestPlayer = null;

    foreach (GameObject p in players)
    {
        float dist = Vector3.Distance(transform.position, p.transform.position);
        if (dist < shortestDistance)
        {
            shortestDistance = dist;
            nearestPlayer = p.transform;
        }
    }

    targetPlayer = nearestPlayer;
}

}
