using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Transform player;
    NavMeshAgent agent;
    Animator animator;

    public float detectRange;
    public float attackRange;

    public Transform attackPoint;
    public float attackPointRange = .5f;

    float lastAttackTime;
    public float coolDownTime = 1.1f;

    public LayerMask playerLayer;
    public GameObject bloodSplash;

    bool isDisable = false;


    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent  = GetComponent<NavMeshAgent>();  
        animator = GetComponent<Animator>();
    }

    void Update(){
        float distance = Vector3.Distance(transform.position, player.position);
        
        if(distance <= attackRange){
            agent.ResetPath();
             animator.SetBool("Run", false);

            animator.SetTrigger("Attack");

        }else if(distance <= detectRange){

            Chase();
        }else{
            Idle();
        }

    }

    void Chase(){
        if(!isDisable){
        agent.SetDestination(player.position);
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

    void Attack(){
        animator.SetBool("Run", false);
        if(Time.time - lastAttackTime > coolDownTime){
            lastAttackTime = Time.time;
           
            Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackPointRange, playerLayer);
            foreach(var hit in hits){
                Vector3 closestPoint = hit.GetComponent<Collider>().ClosestPoint(attackPoint.position);

                if(hit.GetComponent<PlayerCombat>().isBlocking){
                    Debug.Log("Blocked");
                }
                Debug.Log(hit.name);
                GameObject splash = Instantiate(bloodSplash, closestPoint, Quaternion.identity);
                Destroy(splash, 3f);
                hit.GetComponent<PlayerAnimation>().DamageAnimation();
            }
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

   
}
