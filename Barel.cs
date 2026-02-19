using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barel : MonoBehaviour
{
    [SerializeField] ParticleSystem expolison;
    [SerializeField] hordo pHordo;
        [SerializeField] LayerMask ground;
        [SerializeField] private AudioClip audioClip;


    public List<PlayerController> inside = new List<PlayerController>();
    bool isGrounded;
    bool blowUpStarted = false;


    void Update()
    {   

        CheckGround();

        if (!isGrounded && !blowUpStarted)
        {
            blowUpStarted = true;
            StartCoroutine(BlowUpTimer());
        }
    }

    IEnumerator BlowUpTimer(){
    yield return new WaitForSeconds(3f);
    BlowUp();
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.down * 5.0f
        );
    }
        private void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position,
            Vector3.down,
            5.0f,
            ground
        );
    }

    public void BlowUp()
    {

        expolison.Play();
        gameObject.transform.parent.GetComponent<CanvasGroup>().alpha = 0f;
        gameObject.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
        pHordo.BlowUp();
        SoundFXManager.instance.PlaySoundFXClip(audioClip,transform, 10f);
        if(inside.Count == 0)return;
        foreach (PlayerController figura in inside)
        {
            if(figura == null)return;
            Rigidbody hips =  figura.GetComponent<Rigidbody>();
            Vector3 direction = (hips.transform.position - transform.position).normalized;
            hips.AddForce(direction * 100.0f , ForceMode.Impulse);
            figura.healthPoints = 0.0f;
        }

    }

    


    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("blue")||other.CompareTag("red"))
        {
        inside.Add(other.GetComponent<PlayerController>());
        }
    }

    void OnTriggerExit(Collider other)
    {
            
        if (other.CompareTag("blue")||other.CompareTag("red"))
        {
        inside.Remove(other.GetComponentInChildren<PlayerController>());

        }
    }

}
