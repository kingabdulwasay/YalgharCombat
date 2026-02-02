using UnityEngine;
using Photon.Pun;
public class CamSwitching : MonoBehaviour
{
   public Camera lookCam;
    Camera combatCam;
    public bool lockRotation;
    PhotonView view;

    void Start(){ 
        view = GetComponent<PhotonView>();
        if(view.IsMine){
        // GameObject camObj = GameObject.FindGameObjectWithTag("CombatCam");
        // combatCam = camObj.GetComponent<Camera>();
        // combatCam.enabled = false;
        lookCam.enabled = true;
        lockRotation = false;
        }else{
            lookCam.enabled = false;
            lookCam.GetComponent<AudioListener>().enabled = false;
        }
    }

    public void FilmingwithAttacking(){
        if(view.IsMine){
            lockRotation = true;
            lookCam.enabled = false;
            combatCam.enabled = true;
        }
        
    }
    public void FilmingwithoutAttacking(){
        if(view.IsMine){
            combatCam.enabled = false;
            lookCam.enabled = true;
            lockRotation = false;
        }
       
    }
}
