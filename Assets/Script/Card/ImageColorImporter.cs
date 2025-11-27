using UnityEngine;
using UnityEngine.UI;
using SFB;
using System.IO;

public class ImageColorImporter : MonoBehaviour
{
    public GameObject cardObject;      // 色を変えるオブジェクト
    public Image displayImage;         // 画像を表示するImageコンポーネント（UI）

    public void OnClickSelectImage()
    {
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg")
        };

        string[] paths = StandaloneFileBrowser.OpenFilePanel("画像を選択", "", extensions, false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            LoadImageAndSetColor(paths[0]);
        }
    }

    void LoadImageAndSetColor(string path)
    {
        byte[] data = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        if (!tex.LoadImage(data))
        {
            Debug.LogError("画像読み込み失敗");
            return;
        }

        // Texture2DをSpriteに変換してImageにセット
        if (displayImage != null)
        {
            Sprite sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f));  // ピボットを中心に設定
            displayImage.sprite = sprite;
            displayImage.preserveAspect = true; // アスペクト比を保持
        }

        // 平均色を計算して色を分類し、カードの色を変える
        Color avgColor = GetAverageColor(tex);
        Color targetColor = ClassifyColor(avgColor);

        var sr = cardObject.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = targetColor;
            return;
        }

        var img = cardObject.GetComponent<Image>();
        if (img != null)
        {
            img.color = targetColor;
            return;
        }

        Debug.LogWarning("対象オブジェクトにSpriteRendererもImageもありません");
    }

    Color GetAverageColor(Texture2D tex)
    {
        Color[] pixels = tex.GetPixels();
        float r = 0, g = 0, b = 0;
        int count = 0;

        foreach (Color c in pixels)
        {
            if (c.a > 0.1f)  // アルファ値が0.1より大きい（ほぼ透明でない）ピクセルだけ集計
            {
                r += c.r;
                g += c.g;
                b += c.b;
                count++;
            }
        }

        if (count == 0) return Color.clear; // 透明しかなければ透明を返す（必要に応じて変更）

        return new Color(r / count, g / count, b / count);
    }

    Color ClassifyColor(Color color)
    {
        float r = color.r * 255f;
        float g = color.g * 255f;
        float b = color.b * 255f;

        float diffRG = Mathf.Abs(r - g);
        float diffGB = Mathf.Abs(g - b);
        float diffBR = Mathf.Abs(b - r);

        if (diffRG <= 10f && diffGB <= 10f && diffBR <= 10f)
        {
            float brightness = (r + g + b) / 3f;
            if (brightness < 128f)
            {
                Debug.Log("分類結果: Black");
                return Color.black;
            }
            else
            {
                Debug.Log("分類結果: White");
                return Color.white;
            }
        }

        if (r >= g && r >= b)
        {
            Debug.Log("分類結果: Red");
            return Color.red;
        }
        else if (g >= r && g >= b)
        {
            Debug.Log("分類結果: Green");
            return Color.green;
        }
        else
        {
            Debug.Log("分類結果: Blue");
            return Color.blue;
        }
    }
}
