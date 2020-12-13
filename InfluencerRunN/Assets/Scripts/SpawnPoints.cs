using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPoints : MonoBehaviourPunCallbacks
{
    public bool occupato = false;

    
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            occupato = true;
        }
    }
}
