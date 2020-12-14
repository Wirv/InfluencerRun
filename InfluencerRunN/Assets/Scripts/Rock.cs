using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameFlow.direction == Direzione.Nord)
             GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -1) * 600 * Time.deltaTime, ForceMode.Force);
        else if (GameFlow.direction == Direzione.Ovest)
            GetComponent<Rigidbody>().AddForce(new Vector3(+1, 0, 0) * 600 * Time.deltaTime, ForceMode.Force);
        else if (GameFlow.direction == Direzione.Est)
            GetComponent<Rigidbody>().AddForce(new Vector3(-1, 0, 0) * 600 * Time.deltaTime, ForceMode.Force);
        else if (GameFlow.direction == Direzione.Sud)
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, +1) * 600 * Time.deltaTime, ForceMode.Force);
    }
}
