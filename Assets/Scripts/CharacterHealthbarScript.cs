using UnityEngine;
using UnityEngine.UI;


public class CharacterHealthbarScript : MonoBehaviour
{

    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private float characterCurrentHealth;

    [SerializeField] private GameObject DeathScreen;
    private CharacterControll characterControllerScript;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            characterControllerScript = player.GetComponent<CharacterControll>();
            if (characterControllerScript == null)
            {
                Debug.LogError("CharacterControllerScript not found on the Player object.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found with the tag 'Player'.");
        }
    }

    void Update()
    {
        healthBarSlider.value = characterControllerScript.ReturnHealth();
        if(healthBarSlider.value <= 0 )
        {
            //die
            if (DeathScreen != null)
            {
                DeathScreen.SetActive(true);
                Time.timeScale = 0.0f;
            }
            //pause game
        }
    }
}
