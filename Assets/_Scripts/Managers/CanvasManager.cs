using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class CanvasManager : MonoBehaviour
{
    [Header("Button")]
    public Button startButton;
    public Button mainMenuQuitButton;
    public Button resumeButton;
    public Button playAgainButton;
    public Button mainMenuButton;
    public Button gameOverQuitButton;
    public Button pauseQuitButton;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;

    [Header("Text")]
    public TMP_Text scoreText;
    public TMP_Text HighScoreText;

    [Header("Image")]
    public Image LifeOne;
    public Image LifeTwo;
    public Image LifeThree; 


    // Only 2 scenes in play for now so no need for a dedicated Singleton class that also destroys itself on scene change.
    // If more scenes are added, a dedicated Singleton class that also destroys itself on scene change will be needed.
    static CanvasManager instance = null;
    public static CanvasManager Instance => instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (startButton)
            startButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(1));

        if (gameOverQuitButton || pauseQuitButton)
        {
            gameOverQuitButton.onClick.AddListener(Quit);
            pauseQuitButton.onClick.AddListener(Quit);
        }

        if (mainMenuQuitButton)
            mainMenuQuitButton.onClick.AddListener(Quit);

        //if(playAgainButton)
        //    playAgainButton.onClick.AddListener(() => GameManager.Instance.RestartGame());

        if (resumeButton)
            resumeButton.onClick.AddListener(() => SetMenus(null, pauseMenu));

        if (mainMenuButton)
            mainMenuButton.onClick.AddListener(() => GameManager.Instance.ChangeScene(0));


    }



    void SetMenus(GameObject menuToActive, GameObject menuToInactive)
    {
        if (menuToActive)
            menuToActive.SetActive(true);

        if (menuToInactive)
            menuToInactive.SetActive(false);
    }




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
        if (!pauseMenu) return;


        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);

        }
        if (pauseMenu.activeInHierarchy)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        if (GameManager.Instance.Lives == 0)
        {
            gameOverMenu.SetActive(true);
        }

        if (scoreText)
            scoreText.text = GameManager.Instance.Score.ToString();

        float LivesImage = GameManager.Instance.Lives;

        switch (LivesImage)
        {
            case 3:

                LifeOne.enabled = true; 
                LifeTwo.enabled = true; 
                LifeThree.enabled = true;                
                break;

             case 2:

                LifeTwo.enabled = true;
                LifeOne.enabled = true;
                LifeThree.enabled= false;
                break;



             case 1:

                LifeOne.enabled = true;
                LifeTwo.enabled = false;
                LifeThree.enabled = false;

                break;

            case 0:

                LifeOne.enabled = false;
                LifeTwo.enabled= false;
                LifeThree.enabled = false;
                break;


              default:
              break;
        }
    }

   




  





}
