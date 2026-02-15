using UnityEditor;
using UnityEngine;
using UnityEngine.LowLevel;


public class HitDetection : MonoBehaviour
{
    public PlayerController pc;


    void OnTriggerEnter(Collider other)
    {
        if (pc.isHitting)
        {
            if (other.gameObject.tag == "blue" || other.gameObject.tag == "red")
            {
            PlayerController enemyPc = other.transform.parent.GetComponentInChildren<PlayerController>();
            enemyPc = other.GetComponent<PlayerController>();
            enemyPc.healthPoints -= 3.3f;
            }
        }

    }

}
