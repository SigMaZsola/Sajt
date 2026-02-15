using Asino;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinnerScript : MonoBehaviour
{
    public Image healthbar;
    public Image healthbar1;

    public PathCurver path1;
    public PathCurver path2;
    public Transform label;
    [SerializeField] RingCounter ringCounter;
    void Update()
    {
        checkHealth();
        
    }
    void checkHealth()
    {
        if(ringCounter.gTimer.time > ringCounter.gTimer.endTime)return;
        
        label.GetComponent<TMP_Text>().text = ringCounter.determineWinner();
        label.GetComponent<CanvasGroup>().alpha = 1.0f;
    }
}
