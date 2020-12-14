using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirTrigger : MonoBehaviour
{
    public Animator anim;
    public Direzione direction = Direzione.None;
    public GameObject sinistra, destra;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("cambio direzione");
        if (direction == Direzione.None)
        {
            if (other.gameObject.tag == "Player")
            {

                if (other.transform.parent.GetComponent<Player_Behaviour>().posDesignata == CamMove.instance.PosS.transform || other.transform.parent.GetComponent<Player_Behaviour>().posDesignata == CamMove.instance.PosJumpS.transform)
                {
                    if (GameFlow.direction == Direzione.Nord) { GameFlow.direction = Direzione.Ovest; }
                    else if (GameFlow.direction == Direzione.Ovest) { GameFlow.direction = Direzione.Sud; }
                    else if (GameFlow.direction == Direzione.Est) { GameFlow.direction = Direzione.Nord; }
                    else if (GameFlow.direction == Direzione.Sud) { GameFlow.direction = Direzione.Est; }
                    other.gameObject.transform.parent.transform.parent.GetComponent<PlayerContainer>().changedir = true;
                    other.gameObject.transform.parent.transform.parent.GetComponent<PlayerContainer>().pointDirection = sinistra;
                    GameFlow.changeDir = true;
                    direction = Direzione.sinistra;
                }
                else if (other.transform.parent.GetComponent<Player_Behaviour>().posDesignata == CamMove.instance.PosD.transform || other.transform.parent.GetComponent<Player_Behaviour>().posDesignata == CamMove.instance.PosJumpD.transform)
                {
                    if (GameFlow.direction == Direzione.Nord) { GameFlow.direction = Direzione.Est; }
                    else if (GameFlow.direction == Direzione.Est) { GameFlow.direction = Direzione.Sud; }
                    else if (GameFlow.direction == Direzione.Ovest) { GameFlow.direction = Direzione.Nord; }
                    else if (GameFlow.direction == Direzione.Sud) { GameFlow.direction = Direzione.Ovest; }
                    other.gameObject.transform.parent.transform.parent.GetComponent<PlayerContainer>().changedir = true;
                    other.gameObject.transform.parent.transform.parent.GetComponent<PlayerContainer>().pointDirection = destra;
                    GameFlow.changeDir = true;
                    direction = Direzione.destra;
                }
                else if (other.transform.parent.GetComponent<Player_Behaviour>().posDesignata == CamMove.instance.PosC.transform || other.transform.parent.GetComponent<Player_Behaviour>().posDesignata == CamMove.instance.PosJumpC.transform)
                {
                    int x = Random.Range(0, 2);
                    if (x == 0)
                    {
                        if (GameFlow.direction == Direzione.Nord) { GameFlow.direction = Direzione.Ovest; }
                        else if (GameFlow.direction == Direzione.Ovest) { GameFlow.direction = Direzione.Sud; }
                        else if (GameFlow.direction == Direzione.Est) { GameFlow.direction = Direzione.Nord; }
                        else if (GameFlow.direction == Direzione.Sud) { GameFlow.direction = Direzione.Est; }
                        other.gameObject.transform.parent.transform.parent.GetComponent<PlayerContainer>().changedir = true;
                        other.gameObject.transform.parent.transform.parent.GetComponent<PlayerContainer>().pointDirection = sinistra;
                        GameFlow.changeDir = true;
                        direction = Direzione.sinistra;
                    }
                    else
                    {
                        if (GameFlow.direction == Direzione.Nord) { GameFlow.direction = Direzione.Est; }
                        else if (GameFlow.direction == Direzione.Est) { GameFlow.direction = Direzione.Sud; }
                        else if (GameFlow.direction == Direzione.Ovest) { GameFlow.direction = Direzione.Nord; }
                        else if (GameFlow.direction == Direzione.Sud) { GameFlow.direction = Direzione.Ovest; }
                        other.gameObject.transform.parent.transform.parent.GetComponent<PlayerContainer>().changedir = true;
                        other.gameObject.transform.parent.transform.parent.GetComponent<PlayerContainer>().pointDirection = destra;
                        GameFlow.changeDir = true;
                        direction = Direzione.destra;
                    }
                }
            }
        }
        else 
        {
            if (other.gameObject.tag == "Player")
            {
                if (direction == Direzione.sinistra)
                {
                    if (GameFlow.direction == Direzione.Nord) { GameFlow.direction = Direzione.Ovest; }
                    else if (GameFlow.direction == Direzione.Ovest) { GameFlow.direction = Direzione.Sud; }
                    else if (GameFlow.direction == Direzione.Est) { GameFlow.direction = Direzione.Nord; }
                    else if (GameFlow.direction == Direzione.Sud) { GameFlow.direction = Direzione.Est; }
                    other.gameObject.transform.parent.transform.parent.GetComponent<PlayerContainer>().changedir = true;
                    other.gameObject.transform.parent.transform.parent.GetComponent<PlayerContainer>().pointDirection = sinistra;
                    GameFlow.changeDir = true;
                    direction = Direzione.sinistra;
                }
                else if (direction == Direzione.destra)
                {
                    if (GameFlow.direction == Direzione.Nord) { GameFlow.direction = Direzione.Est; }
                    else if (GameFlow.direction == Direzione.Est) { GameFlow.direction = Direzione.Sud; }
                    else if (GameFlow.direction == Direzione.Ovest) { GameFlow.direction = Direzione.Nord; }
                    else if (GameFlow.direction == Direzione.Sud) { GameFlow.direction = Direzione.Ovest; }
                    other.gameObject.transform.parent.transform.parent.GetComponent<PlayerContainer>().changedir = true;
                    other.gameObject.transform.parent.transform.parent.GetComponent<PlayerContainer>().pointDirection = destra;
                    GameFlow.changeDir = true;
                    direction = Direzione.destra;
                }
            }

        }
    }

    
}
