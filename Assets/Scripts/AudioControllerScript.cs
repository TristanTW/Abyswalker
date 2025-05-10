using UnityEngine;

public class AudioControllerScript : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    public static AudioControllerScript Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    public void PlaySound(AudioClip audioClip)
    {
        
        _audioSource.PlayOneShot(audioClip);
    }
}
