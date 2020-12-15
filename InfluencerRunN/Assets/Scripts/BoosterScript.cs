using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterScript : MonoBehaviour
{

    public float SpeedBoost = 1f; //la velocità da aggiungere al player
    public float BoostDuration = 1f;//quanti secondi dura il boost

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player_Behaviour pScript = other.GetComponent<Player_Behaviour>(); // prendi lo script del player

            if (pScript!=null)
            {
                pScript.SpeedBoost(SpeedBoost, BoostDuration);  //fagli partire il metodo per lo speedboost
            }
            else
            {
                Debug.LogError($"PlayerBehaviour non trovato nel gameobject {other.name}");
            }
        }
    }
}
