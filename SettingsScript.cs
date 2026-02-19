using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SettingsScript : MonoBehaviour
{
public int rounds
{
    get => _rounds;
    set
    {
        _rounds = Mathf.Clamp(value, 1, 10);

        if (input != null)
            input.text = "ROUNDS: " + _rounds.ToString();
                    PlayerPrefs.SetInt("Rounds", _rounds);
        PlayerPrefs.Save();
    }
}


    private int _rounds = 3;
    CanvasGroup cg;
    public bool isOpen;

    [SerializeField] TMP_Text input;
    
    public static SettingsScript Instance;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        
        Instance = this;
    }


    void Start()
    {

        if (PlayerPrefs.HasKey("Rounds"))
        {
            _rounds = PlayerPrefs.GetInt("Rounds");
        }

        input.text = "ROUNDS: " + _rounds.ToString();
        cg.alpha = 0f;

    }
    
    public void ExitGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

        public void OnEscape(InputValue v)
    {
        if (v.isPressed) OpenSettings();
    }


    public void OpenSettings()
{
    isOpen  = !isOpen;

    if (isOpen)
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
    else
    {
        Time.timeScale = 0f;

        Cursor.visible = true;
        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
}

}
