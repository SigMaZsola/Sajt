using System.Collections;
using Asino;
using UnityEngine;

public class Grab : MonoBehaviour
{
    public GameObject grabbedObject;
    GameObject nearbyObject;

    public CrowdController crowd;
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void grabCsacsi()
    {
        if (nearbyObject == null || grabbedObject != null)
            return;

        grabbedObject = nearbyObject;

        Rigidbody grb = grabbedObject.GetComponent<Rigidbody>();
        grb.isKinematic = false;
        grb.WakeUp();
        grb.linearVelocity = Vector3.zero;
        grb.angularVelocity = Vector3.zero;

        CsacsiController script = grabbedObject.GetComponent<CsacsiController>();
        if (script != null)
            script.toTarget = false;

        FixedJoint fj = grabbedObject.AddComponent<FixedJoint>();
        fj.connectedBody = rb;
        fj.breakForce = 99901;

        StartCoroutine(EndPush());
    }

    public void ThrowCsacsi()
    {
        if (grabbedObject == null)
            return;

        StartCoroutine(ThrowRoutine());
    }

    IEnumerator ThrowRoutine()
    {
        GameObject obj = grabbedObject;

        FixedJoint fj = obj.GetComponent<FixedJoint>();
        if (fj) Destroy(fj);

        yield return new WaitForFixedUpdate();

        Rigidbody rbObj = obj.GetComponent<Rigidbody>();
        rbObj.linearVelocity = Vector3.zero;
        rbObj.angularVelocity = Vector3.zero;

        Vector3 throwDir = crowd.main.transform.forward + Vector3.up * 2f;
        throwDir.Normalize();

        rbObj.AddForce(throwDir * 50f, ForceMode.Impulse);

        StartCoroutine(EnableAI(obj));

        grabbedObject = null;
    }

    IEnumerator EnableAI(GameObject obj)
    {
        yield return new WaitForSeconds(1f);

        var script = obj.GetComponent<CsacsiController>();
        if (script != null)
            script.toTarget = true;
    }

    void ungrabCsacsi()
    {
        if (grabbedObject == null)
            return;

        FixedJoint fj = grabbedObject.GetComponent<FixedJoint>();
        if (fj) Destroy(fj);

        var script = grabbedObject.GetComponent<CsacsiController>();
        if (script != null)
            script.toTarget = true;

        grabbedObject = null;
    }

    IEnumerator EndPush()
    {
        yield return new WaitForSeconds(5f);
        ungrabCsacsi();
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "BlueCsacsi" || other.name == "RedCsacsi" || other.CompareTag("hordo"))
        {
            nearbyObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.name == "BlueCsacsi" || other.name == "RedCsacsi" || other.CompareTag("hordo")) 
            && nearbyObject == other.gameObject)
        {
            nearbyObject = null;
        }

    }
}