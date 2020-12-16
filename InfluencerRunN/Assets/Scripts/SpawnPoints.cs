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
            try
            {
                other.gameObject.transform.parent.transform.parent.GetComponent<PlayerContainer>().tagSpawn = gameObject.tag;

                if (other.transform.parent.transform.parent.GetComponent<PlayerContainer>().tagSpawn == gameObject.tag)
                {
                    other.transform.parent.transform.parent.GetComponent<PlayerContainer>().changedir = false;
                    other.transform.parent.transform.parent.GetComponent<PlayerContainer>().pointDirection = null;
                }
            }
            catch
            {

            }            
        }
    }
}
