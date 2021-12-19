using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    [SerializeField] private TextMeshProUGUI money;
    [SerializeField] private TextMeshProUGUI moneyCard;

    [HideInInspector] public int Money;

    private void Awake()
    {
        Instance = this;
    }
}
