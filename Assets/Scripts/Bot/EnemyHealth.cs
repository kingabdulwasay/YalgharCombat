using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float currentHealth = 100f;

    
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(transform.name  + " 's current health: "+ currentHealth);
    }
}
