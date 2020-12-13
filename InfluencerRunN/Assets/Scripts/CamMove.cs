using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{

    public static CamMove instance;
    public GameObject PosC, PosS, PosD, PosJumpC, PosJumpS, PosJumpD;

    private void Awake()
    {
        instance = this;
    }

    public Transform Jumping(Transform pos)
    {
        if (pos.name == PosC.name)
        {
            return PosJumpC.transform;
        }
        else if (pos.name == PosS.name)
        {
            return PosJumpS.transform;
        }
        else if (pos.name == PosD.name)
        {
            return PosJumpD.transform;
        }
        else
            return null;
        
    }
}
