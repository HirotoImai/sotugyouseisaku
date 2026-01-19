using UnityEngine;

public class SaveSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip saveSuccessClip;
    public AudioClip saveFailClip;

    public void PlaySuccess()
    {
        audioSource.PlayOneShot(saveSuccessClip);
    }

    public void PlayFail()
    {
        audioSource.PlayOneShot(saveFailClip);
    }
}