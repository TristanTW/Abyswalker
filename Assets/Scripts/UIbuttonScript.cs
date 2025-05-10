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
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
        Debug.Log("Quit");
    }

    void Respawn()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Respawn");

    }
}
