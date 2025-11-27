using UnityEngine;
using UnityEngine.UI;

public class ResourceGauge : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider slider;             // ゲージ用スライダー
    [SerializeField] private RectTransform dividerPrefab; // 細い線のUIプレハブ
    [SerializeField] private Transform dividerParent;    // Divider を置く空オブジェクト

    [Header("設定")]
    [SerializeField] private int maxValue = 10;        // 最大値
    [SerializeField] private float regenSpeed = 1f;   // 回復速度（1秒あたり）
    private float currentValue;

    void Start()
    {
        // Sliderの設定
        slider.maxValue = maxValue;
        slider.value = 0;
        currentValue = 0;

        // 区切り線を生成
        CreateDividers();
    }

    void Update()
    {
        // 時間経過で回復
        if (currentValue < maxValue)
        {
            currentValue += regenSpeed * Time.deltaTime;
            currentValue = Mathf.Min(currentValue, maxValue);
        }

        // 区切り単位でバーをスナップ
        slider.value = Mathf.Floor(currentValue);
    }

    // ゲージ消費
    public void Consume(float amount)
    {
        currentValue -= amount;
        currentValue = Mathf.Max(currentValue, 0);
    }

    // ゲージ増加
    public void Add(float amount)
    {
        currentValue += amount;
        currentValue = Mathf.Min(currentValue, maxValue);
    }

    public float GetCurrentValue()
    {
        return currentValue;
    }

    // 区切り線を生成
    private void CreateDividers()
    {
        if (dividerPrefab == null || dividerParent == null) return;

        for (int i = 1; i < maxValue; i++)
        {
            RectTransform div = Instantiate(dividerPrefab, dividerParent);
            div.anchorMin = new Vector2((float)i / maxValue, 0);
            div.anchorMax = new Vector2((float)i / maxValue, 1);
            div.anchoredPosition = Vector2.zero;
        }
    }
}
