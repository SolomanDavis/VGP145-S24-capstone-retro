using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : SingletonInScene<CanvasManager>
{
    // Events for other game systems to listen to
    public event UnityAction GamePaused; // Other game systems should pause their activities if applicable
    public event UnityAction GameUnpaused; // Other game systems should resume their activities if applicable
   

    [Header("Button")]
    public Button StartButton;
    public Button MainMenuQuitButton;
    public Button ResumeButton;
    public Button PlayAgainButton;
    public Button MainMenuButton;
    public Button GameOverQuitButton;
    public Button PauseQuitButton;

    [Header("Menus")]
    public GameObject MainMenu;
    public GameObject PauseMenu;
    public GameObject EndGameMenu;
   

    [Header("Text")]
    public TMP_Text ScoreText;
    public TMP_Text HighScoreText;
    public TMP_Text GameOverText;
    public TMP_Text WinText;
   

    [Header("Image")]
    public Image[] LifeImages;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        if (StartButton)
        {
            // Load game scene
            // Note: Need to explicitly load the scene as we don't want to start a new game instance in the current title scene
            StartButton.onClick.AddListener(() => SceneManager.LoadSceneAsync("Game"));

            // This needs to be here so that when the game ends and the player returns to the main menu, then presses start again, the game is set back to the beginning. 
            //StartButton.onClick.AddListener(() => GameManager.Instance.RestartGame());
        }

        if (GameOverQuitButton || PauseQuitButton)
        {
            GameOverQuitButton.onClick.AddListener(Quit);
            PauseQuitButton.onClick.AddListener(Quit);
        }

        if (MainMenuQuitButton)
            MainMenuQuitButton.onClick.AddListener(Quit);

        if (PlayAgainButton)
        {
            PlayAgainButton.onClick.AddListener(() => SetMenus(null, EndGameMenu));
            PlayAgainButton.onClick.AddListener(() => GameManager.Instance.RestartGame());
        }

        if (ResumeButton)
        {
            ResumeButton.onClick.AddListener(() => SetMenus(null, PauseMenu));
            ResumeButton.onClick.AddListener(() => GameUnpaused?.Invoke());
        }

        if (MainMenuButton)
            MainMenuButton.onClick.AddListener(() => GameManager.Instance.ChangeScene("Title"));
 
        if (LifeImages.Length > 0)
            UpdateLifeImage(GameManager.Instance.Lives);
    }

    //Sets menus from active to inactive.
    void SetMenus(GameObject menuToActive, GameObject menuToInactive)
    {
        if (menuToActive)
            menuToActive.SetActive(true);

        if (menuToInactive)
            menuToInactive.SetActive(false);
    }

    //Quits the game and stops playing in unity.
    private void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!PauseMenu) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseMenu.SetActive(!PauseMenu.activeSelf);

            // Trigger event for other game systems to pause
            if (PauseMenu.activeSelf)
                GamePaused?.Invoke();
            else
                GameUnpaused?.Invoke();
        }

        if (PauseMenu.activeInHierarchy || EndGameMenu.activeInHierarchy)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }       

        if (ScoreText)
        {
            ScoreText.text = GameManager.Instance.Score.ToString();
        }

        //setting high-score text.
        if (HighScoreText)
        {
            HighScoreText.text = HighScoreManager.Instance.GetHighScore(GameManager.Instance.Score).ToString();
        }
    }

    //Enables end game screen and displays the Winner text.
    public void GameWon()
    {
        EndGameMenu.SetActive(true);
        GameOverText.enabled = false;
        WinText.enabled = true;
        GamePaused?.Invoke();
        
    }

    public void GameOver()
    {
        EndGameMenu.SetActive(true);
        WinText.enabled = false;
        GameOverText.enabled = true;
        GamePaused?.Invoke();
        
    }

    public void UpdateLifeImage(int lives)
    {
        for(int i = 0; i < LifeImages.Length; ++i)
        {
            LifeImages[i].enabled = i < lives;
        }
    }
}
