using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetProgressButtonView : MonoBehaviour
{
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("ClickerSave_v1");
        PlayerPrefs.DeleteKey("ClickUpgradeCost");
        PlayerPrefs.DeleteKey("PassiveUpgradeCost");
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
