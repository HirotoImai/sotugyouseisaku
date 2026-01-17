using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class UnitUI : MonoBehaviour
{
    public Image unitImage;
    public TMP_Text nameText;
    public TMP_Text hpText;
    public TMP_Text attackText;
    public Slider attackGauge;
    [SerializeField] private Image imageColor;
    [SerializeField] private float preAttackMoveY = 15f;
    [SerializeField] private float preAttackDuration = 0.15f;
    [Header("ダメージ表示")]
    [SerializeField] private TextMeshProUGUI damageText;
    [Header("ダメージ演出")]
    [SerializeField] private float moveUpDistance = 30f;
    [SerializeField] private float duration = 0.6f;
    private RectTransform rect;
    private Vector2 basePos;
    private Unit_RT unit;   // ★ 追加
    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    public void Setup(CardData card, PlayerManager_RT owner)
    {
        unit = GetComponent<Unit_RT>();   // ★ 取得
        unit.Setup(card, owner);          // ★ Unit_RT を初期化

        nameText.text = card.cardName;
        unitImage.sprite = card.image;
        attackText.text = card.attack.ToString();
        hpText.text = card.hp.ToString();
        imageColor.color = card.mainColor;
    }

    // ★ HP表示更新用
    public void UpdateHP(int currentHP, int maxHP)
    {
        hpText.text = currentHP.ToString();
    }

    public void ShowDamage(int amount)
    {
        if (damageText == null) return;

        StopAllCoroutines();

        damageText.gameObject.SetActive(true);
        damageText.text = amount.ToString();
        damageText.alpha = 1f;

        RectTransform rect = damageText.rectTransform;
        Vector2 startPos = rect.anchoredPosition;

        StartCoroutine(DamageAnimation(rect, startPos));
    }

    public void UpdateAttackGauge(float rate)
    {
        if (attackGauge != null)
            attackGauge.value = Mathf.Clamp01(rate);
    }

    public void PlayPreAttackMotion()
    {
        StopAllCoroutines();
        basePos = rect.anchoredPosition;
        StartCoroutine(PreAttackMotion());
    }

    private IEnumerator PreAttackMotion()
    {
        float t = 0f;

        // 上に動く
        while (t < preAttackDuration)
        {
            t += Time.deltaTime;
            float y = Mathf.Lerp(0, preAttackMoveY, t / preAttackDuration);
            rect.anchoredPosition = basePos + Vector2.up * y;
            yield return null;
        }

        // 元に戻る
        t = 0f;
        while (t < preAttackDuration)
        {
            t += Time.deltaTime;
            float y = Mathf.Lerp(preAttackMoveY, 0, t / preAttackDuration);
            rect.anchoredPosition = basePos + Vector2.up * y;
            yield return null;
        }

        rect.anchoredPosition = basePos;
    }
    private IEnumerator DamageAnimation(RectTransform rect, Vector2 startPos)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float rate = t / duration;

            rect.anchoredPosition =
                startPos + Vector2.up * moveUpDistance * rate;

            damageText.alpha = 1f - rate;

            yield return null;
        }

        rect.anchoredPosition = startPos;
        damageText.gameObject.SetActive(false);
    }
}

