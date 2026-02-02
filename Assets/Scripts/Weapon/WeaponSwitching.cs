using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class WeaponSwitching : MonoBehaviour
{
    public GameObject activeWeapon;
    PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();    
    }




  



    public void DisableWeapon(){
        if(!view.IsMine) return;

        activeWeapon.SetActive(false);
    }

    public void EnableWeapon(){
        if(!view.IsMine) return;

        activeWeapon.SetActive(true);
    }
}
