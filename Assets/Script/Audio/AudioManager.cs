using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource seSource;


    [Header("SE Clips")]
    public AudioClip saveSuccess;
    public AudioClip saveFail;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        // BGM自動再生（開始時に流す場合）
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }
    // =========================
    // SE再生
    // =========================
    public void PlaySE(AudioClip clip, float volume = 1f)
    {
        seSource.PlayOneShot(clip, volume);
    }

    public void PlaySaveSuccess()
    {
        PlaySE(saveSuccess);
    }

    public void PlaySaveFail()
    {
        PlaySE(saveFail);
    }
}
