using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float currentHealth = 100f;
    Animator animator;
    
    void Start(){
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float amount, string type)
    {
        if(type == "head"){
            Debug.Log("Head Shot");
        }
        animator.SetTrigger("Hit");
        currentHealth -= amount;
        Debug.Log(transform.name  + " 's current health: "+ currentHealth);
    }
}
