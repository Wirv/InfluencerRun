﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    public static PlayerContainer instance;
    PhotonView PV;
    public GameObject cameramain;
    public bool changedir = false;
    public GameObject pointDirection;
    private Transform Target;
    public string tagSpawn;

    private void Awake()
    {
        instance = this;
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        pointDirection = null;
        Target = null;
        if (PV.IsMine)
            GameFlow.instance.PlayerContainer = gameObject;
        else
        {            
            Destroy(cameramain.GetComponent<Camera>());
        }
    }

    // Update is called once per frame
    void Update()
    {
       if(PV.IsMine && GameFlow.start == true)
        {
            if (changedir == false && pointDirection == null)
            {
                gameObject.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward) * Player_Behaviour.instance.force * Time.deltaTime;
                Target = null;
            }
            else if (changedir == true && pointDirection != null)
            {
                if (Target == null)
                    for (int i = 0; i < pointDirection.transform.childCount; i++)
                    {
                        if (pointDirection.transform.GetChild(i).tag == tagSpawn)
                            Target = pointDirection.transform.GetChild(i).transform;
                    }
                transform.rotation = Quaternion.Lerp(transform.rotation, PlayerRotation(), Player_Behaviour.instance.speed * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, Target.position, (Player_Behaviour.instance.speed / 2) * Time.deltaTime);
            }
        }
    }

    private Quaternion PlayerRotation()
    {
        switch (GameFlow.direction)
        {
            case Direzione.Nord:
                return Quaternion.Euler(new Vector3(0, 0, 0));
                break;

            case Direzione.Ovest:
                return Quaternion.Euler(new Vector3(0, -90, 0));
                break;

            case Direzione.Est:
                return Quaternion.Euler(new Vector3(0, 90, 0));
                break;

            case Direzione.Sud:
                return Quaternion.Euler(new Vector3(0, 180, 0));
                break;
            default:
                return Quaternion.Euler(new Vector3(0, 0, 0));
                break;
        }
    }
}
