using UnityEngine;
using TMPro;
using System.Collections;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private float moveUpDistance = 50f;
    [SerializeField] private float duration = 0.8f;

    public void Setup(int damage)
    {
        damageText.text = damage.ToString();
        StartCoroutine(Animate());
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
}
