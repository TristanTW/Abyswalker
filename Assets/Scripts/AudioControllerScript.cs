using UnityEngine;

public class AudioControllerScript : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    public void PlaySound(AudioClip audioClip)
    {

        _audioSource.PlayOneShot(audioClip);
    }
}
