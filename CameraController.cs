using UnityEngine;

public class CameraController: MonoBehaviour
{
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }



    void Start()
    {
        Cursor.visible = false;
    }
}
