using UnityEngine;
using UnityEngine.UI;

public class AttackUIcontrol : MonoBehaviour
{
    [SerializeField] private Image _attackImage;
    private Combat chrcnt;
    void Start()
    {
        chrcnt = GameObject.FindGameObjectWithTag("Player").GetComponent<Combat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chrcnt != null) 
        {
            if(chrcnt.isAttacking != true)
            {
                _attackImage.color = Color.green;
            }
            else
            {
                _attackImage.color = Color.red;
            }
        }
    }
}
