using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    GameObject gameOverUI;
    [SerializeField] GameObject curPointsUI;
    [SerializeField] GameObject areYouSureUI;
    Text curPointsTxt;
    static bool buttonSoundToggle = false;
    [SerializeField] IntRef curPoints;

    void OnEnable()
    {
        gameOverUI = gameObject.transform.GetChild(0).transform.GetChild(0).gameObject;
        curPointsTxt = curPointsUI.GetComponent<Text>();

        Enforcer.Instance.GameStateChanged += UpdateUIBasedOnState;
        Enforcer.Instance.HumanHasDied += UpdatePointsUI;
    }

    void OnDisable()
    {
        Enforcer.Instance.GameStateChanged -= UpdateUIBasedOnState;
        Enforcer.Instance.HumanHasDied -= UpdatePointsUI;
    }

    void UpdateUIBasedOnState(IGameState state)
    {
        if (state == Enforcer.gameOverState)
        {
            EnableGameOverUI();
        }
    }

    void EnableGameOverUI()
    {
        gameOverUI.SetActive(true);
        curPointsUI.SetActive(false);
    }

    void UpdatePointsUI()
    {
        curPointsTxt.text = curPoints.value.ToString();
    }

    //BUTTON FUNCTIONS:

    public void DeleteAllDataClicked()
    {
        GameObject.Find("AreYouSureParent").transform.GetChild(0).gameObject.SetActive(true);
    }

    public void DeleteAllData()
    {
        Saver.DeleteAllData();
        print("data deleted");
    }

    public void SkinButtonClicked()
    {
        SceneManager.LoadScene(3);
    }

    public void StartGameClicked()
    {
        Enforcer.Instance.StartGame();
    }

    public void MainMenuClicked()
    {
        Enforcer.Instance.GoToMainMenu();
    }

    public void RetryClicked()
    {
        Enforcer.Instance.RestartGame();
    }

    public void PlayButtonDownSound()
    {
        if (buttonSoundToggle)
            SoundMan.instance.PlaySound(0, 1.0f);
        else
            SoundMan.instance.PlaySound(1, 1.0f);
        buttonSoundToggle = !buttonSoundToggle;
    }

    public void Jiggle(Transform tran)
    {
        tran.DOComplete();
        tran.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f), 0.2f, 5, 0.5f);
    }

    public void SettingsClicked()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
            SceneManager.LoadScene(2);
        else
            SceneManager.LoadScene(0);
    }
}
