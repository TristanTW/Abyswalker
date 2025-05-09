using UnityEngine;

public class AudioControllerScript : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _takeDamage, _doDamage, _collectCoin;
    public AudioClip currentclip = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _audioSource.clip = currentclip;
        _audioSource.Play();
    }
}
