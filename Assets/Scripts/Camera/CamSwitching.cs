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
    lookCam.GetComponent<AudioListener>().enabled = view.IsMine;
    if(!view.IsMine) return;
        GameObject camObj = GameObject.FindGameObjectWithTag("CombatCam");
        combatCam = camObj.GetComponent<Camera>();
        combatCam.enabled = false;
        lookCam.enabled = true;
        lockRotation = false;
    }

    public void FilmingwithAttacking(){
        lockRotation = true;
        lookCam.enabled = false;
        combatCam.enabled = true;
    }
    public void FilmingwithoutAttacking(){
        combatCam.enabled = false;
        lookCam.enabled = true;
        lockRotation = false;
       
    }
}
