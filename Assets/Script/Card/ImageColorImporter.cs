using UnityEngine;
using UnityEngine.UI;
using SFB;
using System.IO;

public class ImageColorImporter : MonoBehaviour
{
    public GameObject cardObject;
    public Image displayImage;

    public Color backgroundColor { get; private set; }   // 見た目用
    public CardElement element { get; private set; }     // ロジック用

    public void OnClickSelectImage()
    {
        var extensions = new[] {
            new ExtensionFilter("Image Files", "png", "jpg", "jpeg")
        };

        string[] paths = StandaloneFileBrowser.OpenFilePanel(
            "画像を選択", "", extensions, false);

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

        // 画像表示
        if (displayImage != null)
        {
            displayImage.sprite = Sprite.Create(
                tex,
                new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f)
            );
            displayImage.preserveAspect = true;
        }

        // 平均色 → 属性決定
        Color avgColor = GetAverageColor(tex);
        element = ClassifyElement(avgColor);
        backgroundColor = ElementToColor(element);

        // 見た目反映
        if (cardObject.TryGetComponent(out SpriteRenderer sr))
            sr.color = backgroundColor;
        else if (cardObject.TryGetComponent(out Image img))
            img.color = backgroundColor;
        else
            Debug.LogWarning("対象オブジェクトにRenderer/Imageがありません");
    }

    Color GetAverageColor(Texture2D tex)
    {
        Color[] pixels = tex.GetPixels();
        float r = 0, g = 0, b = 0;
        int count = 0;

        foreach (Color c in pixels)
        {
            if (c.a > 0.1f)
            {
                r += c.r;
                g += c.g;
                b += c.b;
                count++;
            }
        }

        if (count == 0) return Color.clear;
        return new Color(r / count, g / count, b / count);
    }

    CardElement ClassifyElement(Color color)
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
            return brightness < 128f ? CardElement.Black : CardElement.White;
        }

        if (r >= g && r >= b) return CardElement.Red;
        if (g >= r && g >= b) return CardElement.Green;
        return CardElement.Blue;
    }

    Color ElementToColor(CardElement element)
    {
        switch (element)
        {
            case CardElement.Red: return Color.red;
            case CardElement.Green: return Color.green;
            case CardElement.Blue: return Color.blue;
            case CardElement.Black: return Color.black;
            case CardElement.White: return Color.white;
            default: return Color.white;
        }
    }
}
