using Assets.Scripts.Classes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Main_Menu
{
    
    public class MenuManager : MonoBehaviour
    {
        // Check if the Player opens the game first time
        void Awake()
        {
            // If player opens the game first time, create data for him.
            Player player = DataManager.LoadData();
            if (player == null)
            {
                print("There is no existing data. First data is creating...");
                string result = DataManager.SaveData(new Player(1, 0,new TileCube()));
                print(result);
            }
        }

        // Load Game Scene to Start Main Game
        public void LoadGameScene()
        {
            SceneManager.LoadScene("Game");
        }

    }
}
