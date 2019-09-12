using Assets.Scripts.Classes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector2 = System.Numerics.Vector2;

namespace Assets.Scripts.Main_Menu
{
    
    public class MenuManager : MonoBehaviour
    {
        public GameObject ContinuePopUp;
        private Player _player;

        // Check if the Player opens the game first time
        public void Awake()
        {
            // Portrait mode for testing on computer
#if UNITY_STANDALONE
            Screen.SetResolution(564, 960, false);
            Screen.fullScreen = false;
#elif UNITY_ANDROID
            Screen.SetResolution(750, 1334, true);
#endif


            // If player opens the game first time, create data for him.
            _player = DataManager.LoadData();
            if (_player == null)
            {
                print("There is no existing data. First data is creating...");
                string result = DataManager.SaveData(new Player(1, 0,new TileCube()));
                print(result);
            }
        }

        // Check If player wants to continue
        public void ContinueGame()
        {
            _player = DataManager.LoadData();
            if (_player.PositionOfNumbers.Count > 0)
            {
                ContinuePopUp.SetActive(true);
            }
            else
            {
                StartGameAgain();
            }
        }

        public void StartGameAgain()
        {
            DataManager.SaveData(new Player(1, 0, new TileCube()));
            LoadGameScene();
        }

        // Load Game Scene to Start Main Game
        public void LoadGameScene()
        {
            SceneManager.LoadScene("Game");
        }

    }
}
