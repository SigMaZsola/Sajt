using UnityEngine;

public class Directionring : MonoBehaviour
{

    public Transform csacsi;
    Transform canvas;

    void Start()
    {
        canvas = this.transform;
    }

    void Update()
    {
        canvas.position = new Vector3(csacsi.position.x, -11f, csacsi.position.z);
        canvas.rotation = csacsi.rotation;
    }
}
