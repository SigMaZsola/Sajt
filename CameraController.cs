using System.Linq;
using Asino;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController: MonoBehaviour
{

    [SerializeField] Camera cam;
    
public Transform c1;
public Transform c2;


    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }
    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
{   

    FindCrowd();
    if(c1 == null || c2 == null) return;

        float crowdDistance = Vector3.Distance(c2.position, c1.position);


        float targetFOV = Mathf.Clamp(crowdDistance * 0.95f, 21, 29);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * 10);
}

    void FindCrowd()
    {

        if(c1 != null && c2 != null) return;


        var crowds = FindObjectsByType<CrowdController>(FindObjectsSortMode.None);


            if (crowds.Length == 1)
            {
                c1 = crowds[0].transform;

            }
            if (crowds.Length == 2)
            {
                c2= crowds[1].transform;
            }
    }
}
