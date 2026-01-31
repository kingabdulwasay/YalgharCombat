using UnityEngine;
using Photon.Pun;
public class PlayerHeallth : MonoBehaviour
{
    public float currentHealth;
    public GameObject bloodSplash;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = 100f;
    }

    [PunRPC]
    public void TakeDamage(float damage, Vector3 point)
    {
        Debug.Log("PUN Executed");
        GetComponent<PlayerAnimation>().DamageAnimation();
        GetComponent<AudioManager>().Damage();
        GameObject splash = Instantiate(bloodSplash, point, Quaternion.identity);
        Destroy(splash, 3f);
    }
}
