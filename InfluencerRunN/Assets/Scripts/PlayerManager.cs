using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {


    }

    void CreateController()
    {

        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerContainer"), new Vector3(0, 1.4f, 0), Quaternion.identity);
    }
}
