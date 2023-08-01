using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    private Transform player;

    [SerializeField]
    private Canvas ammoBoxCanvas;
    [SerializeField]
    private float radius = 7f;


    private void Start()
    {
        player = PlayerManager.instance.player.transform;
    }
    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance < radius)
        {
            ammoBoxCanvas.enabled = true;
        }
        else
        {
            ammoBoxCanvas.enabled = false;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
