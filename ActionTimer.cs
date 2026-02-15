using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ActionTimer : MonoBehaviour
{
    public Image pushBar;
    public float duration = 5f;
    CanvasGroup canvasGroup;
    public bool isRunning { get; private set; }

    Coroutine currentRoutine;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        pushBar.fillAmount = 1f;
        
    }

    void Update()
    {
        canvasGroup.alpha = isRunning ? 1f : 0f;
    }

    public void StartTimer()
    {
        if (isRunning)
            return;

        currentRoutine = StartCoroutine(TimerRoutine());
    }
    public void StopTimer()
    {
        isRunning = false;
        
    }

    IEnumerator TimerRoutine()
    {
        isRunning = true;
        
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            pushBar.fillAmount = 1f - (elapsed / duration);
            yield return null;
        }

        pushBar.fillAmount = 0f;
        isRunning = false;

        yield return null;
       
        pushBar.fillAmount = 1f;

        
    }
}
