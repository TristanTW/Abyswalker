using UnityEngine;
using UnityEngine.UI;

public class UIbuttonScript : MonoBehaviour
{

    [SerializeField] private Button _play;

    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject gameplayMenu;
    [SerializeField] private AudioSource startAudio;

    void Start()
    {
        _play.onClick.AddListener(PlayGame);

        // Ensure gameplay menu is hidden at start
        if (gameplayMenu != null)
            gameplayMenu.SetActive(false);
    }
    void PlayGame()
    {
        // Play audio
        if (startAudio != null)
            startAudio.Play();

        // Hide start menu
        if (startMenu != null)
            startMenu.SetActive(false);

        // Show gameplay menu
        if (gameplayMenu != null)
            gameplayMenu.SetActive(true);

        Debug.Log("Game Started");
    }
}
