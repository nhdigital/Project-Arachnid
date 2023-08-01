using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    
    public GunType Gun;
    [SerializeField]
    private Transform gunParent;
    [SerializeField]
    private List<GunScriptableObject> guns;
    //[SerializeField]
    // ik

    [Space]
    [Header("Runtime Filled")]
    public GunScriptableObject activeGun;


    private void Start()
    {
        GunScriptableObject gun = guns.Find(gun => gun.type == Gun);
        if (gun == null)
        {
            Debug.LogError($"No GunScriptableObject found for GunType: {gun}");
        }

        activeGun = gun;
        gun.Spawn(gunParent, this);

        // IK stuff


    }

}
