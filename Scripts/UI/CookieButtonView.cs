using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CookieButtonView : MonoBehaviour
{
    [SerializeField] private GameStateController game;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (game == null || game.State == null)
                return;

            game.State.AddClick();
        });
    }
}
