using System.Collections.Generic;
using UnityEngine;

public class BlowUpDetector : MonoBehaviour
{
    
    public List<PlayerController> inside = new List<PlayerController>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("blue")||other.CompareTag("red"))
        {
        inside.Add(other.transform.parent.GetComponent<PlayerController>());

        }
    }

    void OnTriggerExit(Collider other)
    {
            
        if (other.CompareTag("blue")||other.CompareTag("red"))
        {
        inside.Remove(other.transform.parent.GetComponentInChildren<PlayerController>());

        }
    }

}
