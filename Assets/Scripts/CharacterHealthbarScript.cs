using UnityEngine;
using UnityEngine.UI;


public class CharacterHealthbarScript : MonoBehaviour
{

    [SerializeField] private Slider healthBarSlider;
    [SerializeField] private float characterCurrentHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBarSlider.value = characterCurrentHealth;
    }
}
