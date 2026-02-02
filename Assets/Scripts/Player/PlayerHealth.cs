using UnityEngine;
using Photon.Pun;
public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public GameObject bloodSplash;
    void Start()
    {
        currentHealth = 100f;
    }

    
    [PunRPC]
    public void TakeDamage(float damage, Vector3 point, string part)
    {
                    Debug.Log("PUN Executed");
        
                    if (part == "Head")  Debug.Log("HEAD ATTACK");
                                
                    else if (part == "Belly") Debug.Log("BELLY ATTACK");
                                
                    else if (part == "Left Leg" || part == "Right Leg") Debug.Log("LEGS ATTACK");
                                
                    else if (part  == "Right Hand" || part == "Left Hand") Debug.Log("HAND ATTACK");
                                
                    else Debug.Log("NO MATCHING BODY PART FOUND");

        currentHealth -= damage;
        GetComponent<PlayerAnimation>().DamageAnimation();
        GetComponent<AudioManager>().Damage();
        GameObject splash = PhotonNetwork.Instantiate(bloodSplash.name, point, Quaternion.identity);
        Destroy(splash, 3f);
    }
}
