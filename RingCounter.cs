
using UnityEngine;

public class RingCounter : MonoBehaviour
{
    public GameOverTimer gTimer;
    bool over = false;
    int blueRingCount;
    int redRingCount;

   [SerializeField]  GameObject ringCounter;


    public string determineWinner()
    {
        string result = "";
        if (blueRingCount < redRingCount)
        {
            result = "RED IS THE WINNER";
        }else if (blueRingCount > redRingCount)
        {
            result = "BLUE IS THE WINNER";
        }else if (blueRingCount == redRingCount)
        {
            result = "TIE";
        }
        return result;
    }

    void Update()
    {
        if(gTimer.time>gTimer.endTime)return;
        startCount();
    }
    void startCount()
    {
        if(over)return;
        over = true;
        Debug.Log("számol");
        foreach(pathRing pr in ringCounter.GetComponentsInChildren<pathRing>())
        {
            if(pr.blueRdProgressTracker.fillAmount == 0)
            {
                blueRingCount ++;
            }else if (pr.blueRdProgressTracker.fillAmount == 1)
            {
                redRingCount ++;
            }
        }
    }

}
