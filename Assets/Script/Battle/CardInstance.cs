using System.IO;
using UnityEngine;

public class CardInstance
{
    public CardData template; // Åöí«â¡
    public string cardName;
    public int cost;
    public int attack;
    public int hp;
    public Color mainColor;
    public Sprite image;

    public CardInstance(CardData template, JsonCardData json)
    {
        this.template = template; // Åöï€éù
        cardName = json.cardName;
        cost = json.cost;
        attack = json.attack;
        hp = json.hp;
        this.template = template;
        cardName = template.cardName;
        cost = template.cost;
        attack = template.attack;
        hp = template.hp;
        mainColor = template.mainColor;
        image = template.image;
        // êF
        if (!string.IsNullOrEmpty(json.color))
            ColorUtility.TryParseHtmlString("#" + json.color, out mainColor);
        else
            mainColor = template.mainColor;

        // âÊëú
        if (!string.IsNullOrEmpty(json.imagePath))
        {
            string path = Path.Combine(Application.persistentDataPath, json.imagePath);
            if (File.Exists(path))
            {
                byte[] bytes = File.ReadAllBytes(path);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);
                image = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                                      new Vector2(0.5f, 0.5f));
                return;
            }
        }

        image = template.image;
    }
}
