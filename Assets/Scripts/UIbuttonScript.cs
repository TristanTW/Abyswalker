using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIbuttonScript : MonoBehaviour
{

    [SerializeField] private Button _quit;
    [SerializeField] private Button _play;

    [SerializeField] private GameObject startMenu;

    void Start()
    {
        Time.timeScale = 1.0f;
        _quit.onClick.AddListener(Quitgame);
        _play.onClick.AddListener(PlayGame);
    }
    void Quitgame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        Debug.Log("Quit");
    }
    void PlayGame()
    {
        SceneManager.LoadScene("Main");
        Debug.Log("Game Started");
    }
}
