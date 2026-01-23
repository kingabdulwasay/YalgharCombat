using UnityEngine;

public class CamControl : MonoBehaviour
{   
     private Transform player;
    GameObject localPlayer;
    public Vector3 offset;

    private PlayerMovement playerMovement;
    private CamSwitching playerCamSwitching;


    private Quaternion cachedCameraRotation;
    private Quaternion cachedPlayerRotation;

    void LateUpdate()
    {
       FindLocalPlayer();

        if (player == null) return;

        Quaternion offsetRotation = playerMovement != null && playerCamSwitching.lockRotation
            ? cachedPlayerRotation       
            : player.rotation;          

        transform.position = player.position + offsetRotation * offset;

        if (playerMovement != null && !playerCamSwitching.lockRotation)
        {
            cachedCameraRotation = Quaternion.LookRotation(
                (player.position + Vector3.up * 0.5f) - transform.position
            );
            transform.rotation = cachedCameraRotation;

            cachedPlayerRotation = player.rotation;
        }
        else
        {
            transform.rotation = cachedCameraRotation;
        }
    }


   
    void FindLocalPlayer(){
       if (player == null)
        {
            localPlayer = GameObject.FindWithTag("Player");
            if (localPlayer != null)
            {
                player = localPlayer.transform;
                playerMovement = localPlayer.GetComponent<PlayerMovement>();
                playerCamSwitching = localPlayer.GetComponent<CamSwitching>();

                cachedCameraRotation = transform.rotation;
                cachedPlayerRotation = player.rotation;
            }
        }
    }
}
