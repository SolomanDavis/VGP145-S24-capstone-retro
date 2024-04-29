using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
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
    public TMP_Text livesText;

    // Only 2 scenes in play for now so no need for a dedicated Singleton class that also destroys itself on scene change.
    // If more scenes are added, a dedicated Singleton class that also destroys itself on scene change will be needed.
    static SceneManager instance = null;
    public static SceneManager Instance => instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (mainMenuQuitButton)
            mainMenuQuitButton.onClick.AddListener(Quit);
        if (gameOverQuitButton)
            gameOverQuitButton.onClick.AddListener(Quit);
        if (pauseQuitButton)
            pauseQuitButton.onClick.AddListener(Quit);

        if (resumeButton)
            resumeButton.onClick.AddListener(() => SetMenus(null, pauseMenu));
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
    }





}
