using UnityEngine;
using TMPro;


using System.Collections;

public class csacsiBubi : MonoBehaviour
{
    public TMP_Text textComp;
    public UnityEngine.UI.Image bubiImg;
    public CanvasGroup cg;
    private string bubiText;
    void Update()
    {
        wawingText();
        
    }
    private void wawingText()

    {
        if(cg.alpha == 0)return;
        textComp.ForceMeshUpdate();
        textComp.text = bubiText;
        var textInfo = textComp.textInfo;

        for (int m = 0; m < textInfo.meshInfo.Length; m++)
        {

            textInfo.meshInfo[m].mesh.vertices =
                textInfo.meshInfo[m].vertices;
        }

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                int index = charInfo.vertexIndex + j;

                Vector3 basePos = verts[index];

                float wave = Mathf.Sin(Time.time * 10f + basePos.x * 0.05f) * 10f;

                verts[index] = basePos + new Vector3(0, wave, 0);
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            textComp.UpdateGeometry(meshInfo.mesh, i);
        }
    }
    public void AppearBubi(string text)
{
    if(cg.alpha == 1)return;
    bubiText = text;
    StartCoroutine(BubiFlow());
}

    IEnumerator BubiFlow()
    {
        

        yield return StartCoroutine(Fill(0, 1, 0.1f));
        cg.alpha = 1;
        yield return new WaitForSeconds(1.5f);
        cg.alpha = 0;
        yield return StartCoroutine(Fill(1, 0, 0.1f));

        
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
