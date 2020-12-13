using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    PhotonView PV;
    public GameObject cameramain;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (PV.IsMine)
            GameFlow.instance.PlayerContainer = gameObject;
        else
        {
            Destroy(cameramain.GetComponent<CamMove>().PosC.gameObject);
            Destroy(cameramain.GetComponent<CamMove>().PosD.gameObject);
            Destroy(cameramain.GetComponent<CamMove>().PosS.gameObject);
            Destroy(cameramain.GetComponent<CamMove>().PosJumpC.gameObject);
            Destroy(cameramain.GetComponent<CamMove>().PosJumpD.gameObject);
            Destroy(cameramain.GetComponent<CamMove>().PosJumpS.gameObject);

            Destroy(cameramain);
        }
    }

    // Update is called once per frame
    void Update()
    {
       // if(PV.IsMine) transform.Translate(Vector3.forward * Player_Behaviour.instance.force * Time.deltaTime);
    }
}
