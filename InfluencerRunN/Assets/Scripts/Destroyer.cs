using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Incrocio")
            Destroy(other.gameObject);
        else if(other.gameObject.tag == "Incrocio")
            if(other.gameObject.transform.parent.gameObject)
                try
                {
                    StartCoroutine(DestroyIncrocio(other.gameObject.transform.parent.gameObject));
                }
                catch
                {
                    return;
                }
           

    }

    IEnumerator DestroyIncrocio(GameObject obj)
    {
        yield return new WaitForSeconds(650 * Time.deltaTime);
        if(obj != null)
            Destroy(obj);
    }
}
