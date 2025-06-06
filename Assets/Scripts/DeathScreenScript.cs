using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenScript : MonoBehaviour
{
    [SerializeField] private Button _quit;
    [SerializeField] private Button _respawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Respawn");
    }
}
