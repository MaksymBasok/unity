using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ClickUpgradeButtonView : MonoBehaviour
{
    [SerializeField] private GameStateController game;
    [SerializeField] private double defaultCost = 10;
    [SerializeField] private double increase = 1;
    [SerializeField] private TMP_Text priceText;
    private Button button;

    private double cost;
    private const string CostKey = "ClickUpgradeCost";

    private void Awake()
    {
        button = GetComponent<Button>();

        cost = PlayerPrefs.HasKey(CostKey)
            ? PlayerPrefs.GetFloat(CostKey)
            : defaultCost;

        button.onClick.AddListener(() =>
        {
            if (game == null || game.State == null) return;

            if (game.State.TrySpend(cost))
            {
                game.State.UpgradeClick(increase);

                cost *= 1.15;

                PlayerPrefs.SetFloat(CostKey, (float)cost);
                PlayerPrefs.Save();
            }
        });
    }

    private void Update()
    {
        if (game == null || game.State == null) return;

        priceText.text = $"Cost: {cost:0}";
        button.interactable = game.State.Score >= cost;
    }
}
