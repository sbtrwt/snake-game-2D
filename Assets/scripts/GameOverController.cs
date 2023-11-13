using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Snake2D.UI
{
    public class GameOverController : MonoBehaviour
    {
        public Button singleButton;
        public Button lobbyButton;
        private void Awake()
        {
            singleButton.onClick.AddListener(OnClickSingle);
            lobbyButton.onClick.AddListener(OnClickLobby);
        }
        private void OnClickSingle()
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }
        private void OnClickLobby()
        {
            SceneManager.LoadScene(0);
        }
    }
}