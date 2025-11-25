using System;

[Serializable]
public class GameState
{
    public double Score { get; private set; }
    public double ClickPower { get; private set; }
    public double PassiveIncomePerSecond { get; private set; }

    public GameState(double score, double clickPower, double passiveIncome)
    {
        Score = score;
        ClickPower = clickPower;
        PassiveIncomePerSecond = passiveIncome;
    }

    public void AddClick()
    {
        Score += ClickPower;
    }

    public void AddPassive(float deltaTime)
    {
        Score += PassiveIncomePerSecond * deltaTime;
    }

    public bool TrySpend(double cost)
    {
        if (Score < cost) return false;

        Score -= cost;
        return true;
    }

    public void UpgradeClick(double amount)
    {
        ClickPower += amount;
    }

    public void UpgradePassive(double amount)
    {
        PassiveIncomePerSecond += amount;
    }
}
