using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIbuttonScript : MonoBehaviour
{
    [SerializeField] private Button _quit;
    [SerializeField] private Button _respawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _quit.onClick.AddListener(Quitgame);
        _respawn.onClick.AddListener(Respawn);
    }

    void Quitgame()
    {
        Application.Quit();
    }

    void Respawn()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(scene);
    }
}
