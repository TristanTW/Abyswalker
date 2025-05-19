using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIbuttonScript : MonoBehaviour
{
    [SerializeField] private Button _quit;
    [SerializeField] private Button _respawn;

    void Start()
    {
        _quit.onClick.AddListener(Quitgame);
        _respawn.onClick.AddListener(Respawn);
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

    void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Respawn");
    }
}