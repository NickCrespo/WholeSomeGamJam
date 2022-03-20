using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    [SerializeField] CanvasGroup pauseMenu;
    [SerializeField] bool pausing = true; // false in menu scenes

    void Start()
    {
        if (pausing && pauseMenu != null)
        {
            Resume();
        }
    }
    void Update()
    {
        if (pausing && Input.GetKeyDown(KeyCode.Tab)) // TODO: make into pause button for tapping for mobile
        {
            Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.interactable = true;
        pauseMenu.alpha = 1;
        pauseMenu.blocksRaycasts = true;
    }
    public void Resume()
    {
        pauseMenu.interactable = false;
        pauseMenu.alpha = 0;
        pauseMenu.blocksRaycasts = false;
    }
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void LoadNextScene()
    {
        int currSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currSceneIndex + 1);
    }
    public void QuitScene()
    {
        LoadScene("MainMenu");
    }
    public void ExitGame()
    {
        Game_Manager.Instance.Exit();
    }
}
