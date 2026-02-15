using TMPro;
using UnityEngine;

public class GameOverTimer : MonoBehaviour
{
    public float time = 600;
    public float endTime = 0;
    [SerializeField] TMP_Text timerText;

    void Update()
    {
        if (time < endTime)
        {
            time -= 0;
            return;
        }
        
        
        time -= Time.deltaTime;
        

        
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}