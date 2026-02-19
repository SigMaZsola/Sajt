using System.Collections;
using Asino;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinnerScript : MonoBehaviour
{
    public Image healthbar;
    public Image healthbar1;

    public PathCurver path1;
    public PathCurver path2;
    public Transform label;
    [SerializeField] RingCounter ringCounter;
    [SerializeField] SettingsScript settings;

    bool hasWinner = false;
    void Update()
    {

        checkHealth();
        if(!hasWinner)return;
        NewGame();
    }
    void checkHealth()
    {
        if (healthbar1.fillAmount == 0)
        {
            label.GetComponent<TMP_Text>().text = "RED IS THE WINNER";
            label.GetComponent<TMP_Text>().color = new Color32(230, 60, 60, 255);
            label.GetComponent<CanvasGroup>().alpha = 1.0f;
            hasWinner = true;
        }
       if (healthbar.fillAmount == 0)
        {
            label.GetComponent<TMP_Text>().text = "BLUE IS THE WINNER";
            label.GetComponent<TMP_Text>().color = new Color32(120, 200, 255, 255); 
            label.GetComponent<CanvasGroup>().alpha = 1.0f;
            hasWinner = true;


        }
        
        if(ringCounter.gTimer.time < ringCounter.gTimer.endTime)
        {
            string winner = ringCounter.determineWinner();
            label.GetComponent<TMP_Text>().text = winner;

            if(winner.Contains("RED"))
                label.GetComponent<TMP_Text>().color = new Color32(230, 60, 60, 255);
            else
                label.GetComponent<TMP_Text>().color = new Color32(120, 200, 255, 255);

            label.GetComponent<CanvasGroup>().alpha = 1.0f;
            hasWinner = true;
            return;
        }

    }
void NewGame()
{
    if(SettingsScript.Instance.rounds <= 1)
    {
        SettingsScript.Instance.rounds = 0;
        StartCoroutine(LoadAfterDelay(2));
    }
    else
    {
        SettingsScript.Instance.rounds -= 1;
        StartCoroutine(LoadAfterDelay(3));
    }
}

    IEnumerator LoadAfterDelay(int scene)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(scene);
    }
}
