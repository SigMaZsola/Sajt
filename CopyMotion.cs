using Unity.Hierarchy;
using UnityEngine;

public class CopyMotion : MonoBehaviour
{
    public Transform targetLimb;
    ConfigurableJoint cjoint;
    public bool inverse;
    
    private Quaternion startRotation;
    public PlayerController  hips;
    void Start()
    {
        cjoint = GetComponent<ConfigurableJoint>();
        
        startRotation = targetLimb.localRotation;
    }

    void FixedUpdate()
    {

        Quaternion rotationDelta = Quaternion.Inverse(startRotation) * targetLimb.localRotation;
        if (hips.healthPoints != 3.3)
        {
            return;
        }
        if (inverse)
        {
            cjoint.targetRotation = Quaternion.Inverse(rotationDelta);
        }
        else
        {
            cjoint.targetRotation = rotationDelta;
        }
    }
}