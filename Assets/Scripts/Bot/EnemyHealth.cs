using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float currentHealth = 100f;

    
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        GetComponent<EnemyAI>().GetHit();
        Debug.Log(transform.name  + " 's current health: "+ currentHealth);
    }
}
