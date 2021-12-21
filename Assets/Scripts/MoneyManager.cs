using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    // UI Elements; MoneyGroup
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI moneyCardText;

    [SerializeField] private float moneyCardCountdown;

    [HideInInspector] public int money;

    private void Awake()
    {
        Instance = this;
        money = 0;
    }

    // Adds money to the total money, and displays it on the Money Card text for a brief time.
    public void AddMoney(int value) {
        money += value;
        moneyText.SetText("$" + money);
        moneyCardText.SetText("+ $" + value);
        moneyCardText.gameObject.SetActive(true);
        Invoke("DisableMoneyCard", moneyCardCountdown); // Calls DisableMoneyCard(), moneyCardCountdown seconds from now.
        return;
    }

    private void DisableMoneyCard() {
        moneyCardText.gameObject.SetActive(false);
        return;
    }
}
