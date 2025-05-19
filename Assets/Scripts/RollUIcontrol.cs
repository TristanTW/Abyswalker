using UnityEngine;
using UnityEngine.UI;

public class RollUIcontrol : MonoBehaviour
{
    [SerializeField] private Image rollImage;
    private CharacterControll chrcnt;
    void Start()
    {
        chrcnt = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterControll>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chrcnt != null) 
        {
            if(chrcnt.canDodge == true)
            {
                rollImage.color = Color.green;
            }
            else
            {
                rollImage.color = Color.red;
            }
        }
    }
}
