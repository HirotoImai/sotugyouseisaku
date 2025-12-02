using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HomeUI : MonoBehaviour
{
    [SerializeField] private Canvas homeCanvas;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI coinText;

    private void Awake()
    {
        // すでに存在していたら重複を防ぐ
        var existing = FindObjectsOfType<HomeUI>();
        if (existing.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateDisplay();

        // シーン変更時に呼ばれるイベント登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void UpdateDisplay()
    {
        playerNameText.text = "player: " + GameManager.Instance.PlayerName;
        coinText.text = "coin: " + GameManager.Instance.PlayerCoins.ToString();
    }

    // シーンが変わったときに呼ばれる
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Home")
        {
            homeCanvas.enabled = true; // 表示
            UpdateDisplay();
        }
        else
        {
            homeCanvas.enabled = false; // 非表示
        }
    }

    // 各ボタンから呼ばれる関数
    public void GoToBattle() => SceneManager.LoadScene("Battle");
    public void GoToDeck() => SceneManager.LoadScene("Deck");
    public void GoToCard() => SceneManager.LoadScene("Card");
    public void GoToTitle() => SceneManager.LoadScene("Title");
}