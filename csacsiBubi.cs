using UnityEngine;
using TMPro;


using System.Collections;

public class csacsiBubi : MonoBehaviour
{
    
    [SerializeField] UnityEngine.UI.Image bubiImg;
    [SerializeField] CanvasGroup cg;

    public bool canShow = true;
    private string bubiText;
    void Update()
    {
        
        
    }

    public void AppearBubi(string text)
{
    if(cg.alpha == 1 ||canShow == false)return;
    
    StartCoroutine(BubiFlow());
}

    IEnumerator BubiFlow()
    {

        yield return StartCoroutine(Fill(0, 1, 0.1f));
        cg.alpha = 1;
        yield return new WaitForSeconds(1.5f);
        cg.alpha = 0;
        yield return StartCoroutine(Fill(1, 0, 0.1f));
        canShow = false;
        
    }

    IEnumerator Fill(float from, float to, float time)
{
    float t = 0;

    while (t < time)
    {
        t += Time.deltaTime;
        bubiImg.fillAmount = Mathf.Lerp(from, to, t / time);
        yield return null;
    }

    bubiImg.fillAmount = to;
}

}
