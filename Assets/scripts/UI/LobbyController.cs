using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public Button singleButton;
    public Button coOpButton;
    public Button exitButton;

    private void Awake()
    {
        singleButton.onClick.AddListener(OnClickSingle);
        coOpButton.onClick.AddListener(OnClickCoOp);
        exitButton.onClick.AddListener(OnClickExit);
    }

    private void OnClickSingle()
    {
        SceneManager.LoadScene(1);
    }
    private void OnClickCoOp()
    {
        SceneManager.LoadScene(2);
    }
    private void OnClickExit()
    {
        Application.Quit();
    }
}
