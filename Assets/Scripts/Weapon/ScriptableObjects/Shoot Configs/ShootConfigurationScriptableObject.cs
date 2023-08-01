using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Guns/Shoot Configuration", order = 2)]
public class ShootConfigurationScriptableObject : ScriptableObject
{
    public LayerMask hitMask;
    public Vector3 spread = new Vector3(0.1f, 0.1f, 0.1f);
    public float fireRate = 0.25f;





}
