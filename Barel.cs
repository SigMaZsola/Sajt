using Unity.VisualScripting;
using UnityEngine;

public class Barel : MonoBehaviour
{
    [SerializeField] ParticleSystem expolison;

[SerializeField] private BlowUpDetector explosionTrigger; 
    bool isFlying = false;

    void Start()
    {
        isFlying = false;
    }

    public void BlowUp()
    {
        expolison.Play();
        gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        Destroy(gameObject,2f);
        if(explosionTrigger.inside.Count == 0)return;
        foreach (PlayerController figura in explosionTrigger.inside)
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
        if (isFlying && other.CompareTag("ground"))
        {
            BlowUp();
        }
    }

        void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ground"))
        {
            isFlying = true;
        }
    }

}
