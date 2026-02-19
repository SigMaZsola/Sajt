using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] CanvasGroup controls;
    void Start()
    {
        Cursor.visible = true;
    }
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

        public void ShowControls()
    {
        if(controls.alpha == 0){
            controls.alpha = 1;
            }
       else
       {controls.alpha = 0;
       }

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
