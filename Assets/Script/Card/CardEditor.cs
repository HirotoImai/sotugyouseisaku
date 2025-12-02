using TMPro;
using UnityEngine;

public class CardEditor : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_InputField costInput;
    public TMP_InputField attackInput;
    public TMP_InputField hpInput;

    public Image previewImage;
    public Image previewColor;

    public Sprite selectedSprite;
    public Color selectedColor;

    public JsonCardData GetJsonData()
    {
        return new JsonCardData
        {
            cardName = nameInput.text,
            cost = int.Parse(costInput.text),
            attack = int.Parse(attackInput.text),
            hp = int.Parse(hpInput.text),
            r = selectedColor.r,
            g = selectedColor.g,
            b = selectedColor.b,
            a = selectedColor.a,
            imageName = selectedSprite != null ? selectedSprite.name : ""
        };
    }
}