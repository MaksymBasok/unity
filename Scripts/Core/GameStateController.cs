using System;
using UnityEngine;

[DisallowMultipleComponent]
public class GameStateController : MonoBehaviour
{
    private const string SaveKey = "ClickerSave_v1";

    [Header("Starting Values")]
    [SerializeField] private double startScore = 0;
    [SerializeField] private double startClickPower = 1;
    [SerializeField] private double startPassiveIncome = 0;

    [Header("Offline earnings")]
    [Range(0f, 1f)]
    [SerializeField]
    private float offlineEarningsPercent = 0.2f;

    public GameState State { get; private set; }

    [Serializable]
    private class SaveData
    {
        public double score;
        public double clickPower;
        public double passiveIncomePerSecond;
        public long lastSaveUnixUtc;
    }

    private void Awake()
    {
        LoadOrCreate();
    }

    private void Update()
    {
        if (State != null)
        {
            State.AddPassive(Time.deltaTime);
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Save();
        }
        else
        {
            LoadOrCreate();
        }
    }

    private void LoadOrCreate()
    {
        if (!PlayerPrefs.HasKey(SaveKey))
        {
            State = new GameState(startScore, startClickPower, startPassiveIncome);
            return;
        }

        var json = PlayerPrefs.GetString(SaveKey);
        var data = JsonUtility.FromJson<SaveData>(json);

        State = new GameState(
            data.score,
            data.clickPower,
            data.passiveIncomePerSecond);

        var lastTime = DateTimeOffset.FromUnixTimeSeconds(data.lastSaveUnixUtc).UtcDateTime;
        var now = DateTime.UtcNow;

        var offlineSeconds = (now - lastTime).TotalSeconds;

        if (offlineSeconds > 0 &&
            State.PassiveIncomePerSecond > 0 &&
            offlineEarningsPercent > 0f)
        {
            var effectiveSeconds = offlineSeconds * offlineEarningsPercent;
            var earned = State.PassiveIncomePerSecond * effectiveSeconds;

            State.AddPassive((float)effectiveSeconds);

            Debug.Log($"[OFFLINE EARNINGS] You earned {earned:0} cookies while offline!");
        }
    }

    private void Save()
    {
        if (State == null) return;

        var data = new SaveData
        {
            score = State.Score,
            clickPower = State.ClickPower,
            passiveIncomePerSecond = State.PassiveIncomePerSecond,
            lastSaveUnixUtc = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };

        var json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }
}
