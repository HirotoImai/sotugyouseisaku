using UnityEngine;
using UnityEngine.UI;

public class RainbowEffect : MonoBehaviour
{
    [Header("対象のUI Image")]
    public Image targetImage;

    [Header("虹色の変化速度")]
    public float speed = 1f;

    void Update()
    {
        if (targetImage == null) return;

        // 時間に応じて色相を0→1の間でループ
        float hue = Mathf.Repeat(Time.time * speed, 1f);

        // HSVをRGBに変換してImageの色に設定
        targetImage.color = Color.HSVToRGB(hue, 1f, 1f);
    }

}