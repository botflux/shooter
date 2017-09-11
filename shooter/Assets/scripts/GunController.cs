using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public Transform weaponHolder;
    public Gun startingGun;

    private Gun equippedGun;

    public float GunHeight
    {
        get
        {
            return weaponHolder.position.y;
        }
    }

    private void Start()
    {
        if (startingGun != null)
        {
            EquipGun(startingGun);
        }
    }

    public void EquipGun (Gun gunToEquip)
    {
        if (equippedGun != null)
            Destroy(equippedGun.gameObject);


        equippedGun = (Gun)Instantiate(gunToEquip, weaponHolder.position, weaponHolder.rotation);
        equippedGun.transform.SetParent(weaponHolder);
    }

    public void OnTriggerHold ()
    {
        if (equippedGun != null)
            equippedGun.OnTriggerHold();
    }

    public void OnTriggerRelease ()
    {
        if (equippedGun != null)
            equippedGun.OnTriggerRelease();
    }
}
