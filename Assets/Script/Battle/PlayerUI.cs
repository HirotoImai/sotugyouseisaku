//using UnityEngine;
//using UnityEngine.UI;

//public class PlayerUI : MonoBehaviour
//{
//    public Slider hpBar;
//    public Slider manaBar;
//    public PlayerManager_RT player;

//    void Start()
//    {
//        if (player != null)
//        {
//            hpBar.maxValue = player.health;
//            manaBar.maxValue = 10f;
//        }
//    }

//    void Update()
//    {
//        if (player != null)
//        {
//            hpBar.value = Mathf.Max(0, player.health);
//            manaBar.value = player.currentMana;
//        }
//    }
//}