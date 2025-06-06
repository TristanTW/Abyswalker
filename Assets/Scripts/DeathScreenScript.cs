using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenScript : MonoBehaviour
{
    [SerializeField] private Button _quit;
    [SerializeField] private Button _respawn;

    private string _sceneName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sceneName = SceneManager.GetActiveScene().name;
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
        StartCoroutine(ReloadSceneAsync());
        
        //SceneManager.LoadScene(_sceneName);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Respawn");
    }

    IEnumerator ReloadSceneAsync()
    {
        // Optionally unload unused assets first
        yield return Resources.UnloadUnusedAssets();

        // Start async load
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        loadOp.allowSceneActivation = false;

        // Wait until it's nearly ready
        while (loadOp.progress < 0.9f)
            yield return null;

        // Optional: do some fade-out or UI here

        // Then activate
        loadOp.allowSceneActivation = true;
    }
}
