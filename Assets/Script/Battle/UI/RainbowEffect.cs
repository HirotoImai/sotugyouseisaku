using UnityEngine;
using UnityEngine.UI;

public class RainbowEffect : MonoBehaviour
{
    [Header("対象のUI Image")]
    public Image targetImage;

    [Header("虹色の変化速度（色相/秒）")]
    public float speed = 0.1f; // 10秒で1周の場合

    private float elapsed = 0f; // 独自カウンター

    void Update()
    {
        if (targetImage == null) return;

        // Time.unscaledDeltaTime を使うことで Time.timeScale = 0 でも動く
        elapsed += Time.unscaledDeltaTime;

        float hue = Mathf.Repeat(elapsed * speed, 1f);
        targetImage.color = Color.HSVToRGB(hue, 1f, 1f);
    }
}