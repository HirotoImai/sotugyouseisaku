using UnityEngine;
using TMPro;
using System.Collections;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float moveUpDistance = 50f;
    [SerializeField] private float duration = 0.8f;

    public void Setup(int damage, bool isAdvantage)
    {
        damageText.text = damage.ToString();
        if (isAdvantage)
        {
            // 派手演出
            damageText.color = Color.yellow;
            damageText.fontSize *= 1.5f;
            damageText.fontStyle = FontStyles.Bold;

            // 跳ねるアニメーション
            StartCoroutine(AnimateAdvantage());
        }
        else
        {
            // 普通の演出
            damageText.color = Color.white;
            StartCoroutine(Animate());
        }
    }

    private IEnumerator Animate()
    {
        Vector3 startPos = transform.localPosition;
        Vector3 endPos = startPos + Vector3.up * moveUpDistance;

        float timer = 0f;
        while (timer < duration)
        {
            float t = timer / duration;
            transform.localPosition = Vector3.Lerp(startPos, endPos, t);
            damageText.alpha = 1f - t;

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
    private IEnumerator AnimateAdvantage()
    {
        Vector3 startPos = transform.localPosition;
        Vector3 endPos = startPos + Vector3.up * moveUpDistance * 1.2f;
        float timer = 0f;

        while (timer < duration)
        {
            float t = timer / duration;
            // 上に移動＋少し弾ませる
            transform.localPosition = Vector3.Lerp(startPos, endPos, t) + Vector3.up * Mathf.Sin(t * Mathf.PI) * 20f;
            damageText.alpha = 1f - t;

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
