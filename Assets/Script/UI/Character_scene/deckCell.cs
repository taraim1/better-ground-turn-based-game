using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class deckCell : MonoBehaviour
{
    [SerializeField] private CardDataSO card_data_SO;
    [SerializeField] private TMP_Text name_tmp;
    [SerializeField] private TMP_Text cost_tmp;
    [SerializeField] private TMP_Text behavior_tmp;
    [SerializeField] private TMP_Text power_range_tmp;
    [SerializeField] private Image illust;
    CardData card_data;


    public void Setup(skillcard_code code) 
    {
        card_data = card_data_SO.CardData_dict[code];

        name_tmp.text = card_data.Name;
        cost_tmp.text = card_data.Cost.ToString();

        string behaviorText = "";
        switch (card_data.BehaviorType)
        {
            case CardBehaviorType.attack:
                behaviorText = "공격";
                break;
            case CardBehaviorType.defend:
                behaviorText = "방어";
                break;
            case CardBehaviorType.dodge:
                behaviorText = "회피";
                break;
            case CardBehaviorType.etc:
                behaviorText = "기타";
                break;
        }
        behavior_tmp.text = behaviorText;

        int minpower = card_data.MinPowerOfLevel[card_data.Level - 1];
        int maxpower = card_data.MaxPowerOfLevel[card_data.Level - 1];
        power_range_tmp.text = string.Format("{0} - {1}", minpower, maxpower);

        illust.sprite = card_data.sprite;
    }
}
