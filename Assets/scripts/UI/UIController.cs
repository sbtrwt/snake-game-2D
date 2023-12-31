using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Snake2D.Sound;

namespace Snake2D.UI
{
    public class UIController : MonoBehaviour
    {
        public static UIController Instance { get; private set; }
        public GameObject gameOverController;
        [SerializeField] private GameObject resumeController;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button pauseButton;
        [SerializeField] private Button soundButton;
        private void Awake()
        {
            if (Instance == null) Instance = this;
        }
        private void Start()
        {
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
}