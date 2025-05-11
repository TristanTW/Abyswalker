using Unity.VisualScripting;
using UnityEngine;

public class BossEndgameScript : MonoBehaviour
{
    [SerializeField] GameObject _boss;
    [SerializeField] GameObject _victoryScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_boss.IsDestroyed())
        {
            if (_victoryScreen != null)
            {
                _victoryScreen.SetActive(true);
                Time.timeScale = 0.0f;

            }

        }
    }
}
