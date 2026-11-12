using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sfxActivateDefault;
    [SerializeField] private AudioClip sfxActivateBeamReceiver;
    [SerializeField] private AudioClip sfxDeactivate;
    [SerializeField] private AudioClip sfxError;
    [SerializeField] private AudioClip sfxSecret;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void SFXActivationDefault()
    {
        audioSource.PlayOneShot(sfxActivateDefault);
    }

    public void SFXDeactivation()
    {
        audioSource.PlayOneShot(sfxDeactivate);
    }

    public void SFXActivationBeamReceiver()
    {
        audioSource.PlayOneShot(sfxActivateBeamReceiver);
    }

    public void SFXError()
    {
        audioSource.PlayOneShot(sfxError);
    }

    public void SFXSecret()
    {
        audioSource.PlayOneShot(sfxSecret);
    }
}
