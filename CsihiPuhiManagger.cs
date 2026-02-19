using Asino;

using UnityEngine;


public class CsihiPuhiManagger : MonoBehaviour
{
    private Transform crowd1;
    private Transform crowd2;

    [SerializeField] StartTimer startTimer;


    void Update()
    {
        FindCrowd();
        if(crowd1 == null || crowd2 == null)return;
        startTimer.canStart = true;
        float cToCDistance = Vector3.Distance(crowd1.transform.position, crowd2.transform.position);

    }

    void FindCrowd()
    {
        if (crowd1 == null || crowd2 == null)
        {
            var crowds = FindObjectsByType<CrowdController>(FindObjectsSortMode.None);

            if (crowds.Length == 1)
            {
                crowd1 = crowds[0].transform;

            }
            if (crowds.Length == 2)
            {
                crowd2 = crowds[1].transform;
            }
        }
        
    }


}
