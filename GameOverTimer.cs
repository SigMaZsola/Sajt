using TMPro;
using UnityEngine;

public class GameOverTimer : MonoBehaviour
{
    public float time = 600;
    public float endTime = 0;
    [SerializeField] TMP_Text timerText;
    public bool canStart = false;
    [SerializeField] StartTimer startTimer;
    void Update()
    {
        if(startTimer.timerText.text == "GO!")canStart = true;
        if(!canStart)return;
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