using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;
using Unity.PlasticSCM.Editor.WebApi;

public class CanvasManager : SingletonInScene<CanvasManager>
{
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
    public GameObject GameOverMenu;

    [Header("Text")]
    public TMP_Text ScoreText;
    public TMP_Text HighScoreText;

    [Header("Image")]
    public Image[] LifeImages;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        if (StartButton)
        {
            StartButton.onClick.AddListener(() => GameManager.Instance.RestartGame());
            StartButton.onClick.AddListener(() => GameManager.Instance.ChangeScene("Game"));
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
            PlayAgainButton.onClick.AddListener(() => GameManager.Instance.RestartGame());
            PlayAgainButton.onClick.AddListener(() => SetMenus(null, GameOverMenu));
        }
           

        if (ResumeButton)
            ResumeButton.onClick.AddListener(() => SetMenus(null, PauseMenu));

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
    void Update()
    {
        if (!PauseMenu) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseMenu.SetActive(!PauseMenu.activeSelf);

        }

        if (PauseMenu.activeInHierarchy)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        if (GameManager.Instance.Lives == 0)
        {
            GameOverMenu.SetActive(true);
        }

        if (ScoreText)
        {
            ScoreText.text = GameManager.Instance.Score.ToString();
        }

        //setting high-score text.
        if(HighScoreText)
        {
            HighScoreText.text = GameManager.Instance.HighScore(GameManager.Instance.Score).ToString();
        }
    }

    public void UpdateLifeImage(int lives)
    {
        for(int i = 0; i < LifeImages.Length; ++i)
        {
            Debug.Log("current index " + i.ToString() + "Current number of lives " + lives.ToString());
            LifeImages[i].enabled = i < lives;
        }
    }
}
