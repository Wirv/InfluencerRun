using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mesh_Behaviour : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Obstacle")
        {
            Player_Behaviour.instance.Collisions(collision.gameObject);

        }
    }

}
