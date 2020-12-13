using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirTrigger : MonoBehaviour
{
    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            
            if (other.GetComponent<Player_Behaviour>().posDesignata == CamMove.instance.PosS.transform || other.GetComponent<Player_Behaviour>().posDesignata == CamMove.instance.PosJumpS.transform)
            {
                
                anim.SetBool("left", true);
                GameFlow.changeDir = true;
               
            }
            else if (other.GetComponent<Player_Behaviour>().posDesignata == CamMove.instance.PosD.transform || other.GetComponent<Player_Behaviour>().posDesignata == CamMove.instance.PosJumpD.transform)
            {
                anim.SetBool("right", true);
                GameFlow.changeDir = true;
                
            }
            else if (other.GetComponent<Player_Behaviour>().posDesignata == CamMove.instance.PosC.transform || other.GetComponent<Player_Behaviour>().posDesignata == CamMove.instance.PosJumpC.transform)
            {
                int x = Random.Range(0, 2);
                if(x == 0)
                {
                    anim.SetBool("right", true);
                    GameFlow.changeDir = true;
                }
                else
                {
                    anim.SetBool("left", true);
                    GameFlow.changeDir = true;
                }
            }
        }
    }
}
