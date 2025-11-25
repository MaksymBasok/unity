using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ScoreView : MonoBehaviour
{
    [SerializeField] private GameStateController game;

    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (game == null || game.State == null)
            return;

        text.text = $"Cookies: {game.State.Score:0}";
    }
}
