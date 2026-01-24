using UnityEngine;
using UnityEngine.UI;
public class WeaponSwitching : MonoBehaviour
{
    int index;
    public RawImage weaponImage;
    public GameObject[] weapons;
    public Texture[] weaponTextures;
    public GameObject activeWeapon;
    public enum WeaponType{
        None,
        Ranged,
        Meele
    }
    public WeaponType currentType;
    void Start()
    {
        currentType = WeaponType.None;
        foreach (GameObject weapon in weapons) {
        weapon.SetActive(false);
    }
       
    }

    void Update()
    {
        Switching();
    }

    void Switching(){
        if(Input.GetKeyDown(KeyCode.Alpha0)){
            Switch(0);
            currentType = WeaponType.None;
        }else if(Input.GetKeyDown(KeyCode.Alpha1)){
            Switch(1);
            currentType = WeaponType.Meele;
        }else if(Input.GetKeyDown(KeyCode.Alpha2)){
             Switch(2);
             currentType = WeaponType.Ranged;
        }
    }

  

    void Switch(int index){
        if(activeWeapon != null){
            activeWeapon.SetActive(false);
        }
        activeWeapon = weapons[index];
        activeWeapon.SetActive(true);
        weaponImage.texture = weaponTextures[index];
    }


    public void DisableWeapon(){
        activeWeapon.SetActive(false);
    }

    public void EnableWeapon(){
        activeWeapon.SetActive(true);
    }
}
