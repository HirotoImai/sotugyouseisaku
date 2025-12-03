using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CostManager : MonoBehaviour
{
    public TMP_Dropdown cost_dropdown;
    public TMP_Dropdown atack_dropdown;
    public TMP_Dropdown hp_dropdown;
    public TMP_Text fp_text;

    private int totalFP = 0;

    void Start()
    {
        cost_dropdown.onValueChanged.AddListener(OnCostChanged);
        atack_dropdown.onValueChanged.AddListener(OnStatChanged);
        hp_dropdown.onValueChanged.AddListener(OnStatChanged);

        OnCostChanged(cost_dropdown.value); // 初期化
    }

    void OnCostChanged(int index)
    {
        totalFP = (index +1) * 2;

        // 最大値を制限（例：0〜totalFP）
        SetDropdownOptions(atack_dropdown, totalFP);
        SetDropdownOptions(hp_dropdown, totalFP);

        atack_dropdown.value = 0;
        hp_dropdown.value = 0;

        UpdateFPText();
    }

    void OnStatChanged(int _)
    {
        int attack = atack_dropdown.value;
        int hp = hp_dropdown.value;
        int remainingFP = totalFP - (attack + hp);

        if (remainingFP < 0)
        {
            // FPを超えたら調整（例：HPを減らす）
            if (attack >= hp && hp > 0) hp_dropdown.value--;
            else if (attack < hp && attack > 0) atack_dropdown.value--;
            else if (attack > 0) atack_dropdown.value--;
            else if (hp > 0) hp_dropdown.value--;
        }

        UpdateFPText();
    }

    void UpdateFPText()
    {
        int used = atack_dropdown.value + hp_dropdown.value;
        int remaining = totalFP - used;
        fp_text.text = $"FreePoint: {remaining}";
    }

    void SetDropdownOptions(TMP_Dropdown dropdown, int maxValue)
    {
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i <= maxValue; i++)
        {
            options.Add(i.ToString());
        }
        dropdown.AddOptions(options);
    }
}