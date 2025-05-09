using UnityEngine;
using UnityEngine.UI;


public class CharacterHealthbarScript : MonoBehaviour
{

    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private float characterCurrentHealth;

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
        healthBarSlider.value = characterControllerScript.PushHealth();
    }
}
