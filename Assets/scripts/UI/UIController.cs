using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static GameObject gameOverController;
    [SerializeField] private GameObject resumeController;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button soundButton;
    private void Awake()
    {
        Debug.Log("UI script in awake event");
    }
    private void Start()
    {
        Debug.Log("UI script in start event");
        if (resumeButton != null)
            resumeButton.onClick.AddListener(OnClickResume);
        if (pauseButton != null)
            pauseButton.onClick.AddListener(OnClickPause);
        if (soundButton != null)
            soundButton.onClick.AddListener(OnClickSound);
    }
    private void OnClickResume()
    {
        resumeController.SetActive(false);
        Time.timeScale = 1;
    }
    private void OnClickPause()
    {
        resumeController.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnClickSound()
    {
        SoundManager.Instance.ToggleMusic();
    }
}
