
using Assets.Scripts.Classes;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Game
{
    // Game Manager is responsible for controlling Input Manager, Level Manager and Data Management

    public class GameManager : MonoBehaviour
    {
        // Manager References
        [SerializeField]
        private LevelManager _levelManager;

        [SerializeField]
        private InputManager _inputManager;

        // Game Area Reference
        [SerializeField]
        private GameObject _gameArea;

        // UI References
        [SerializeField]
        private GameObject _canvas;

        // Private values
        private Player _player;
        private BlockNumbers _bNumbers;

        // Game State and Mission to be accessible by other managers
        public static GameState GameStatus;
        public static Mission Mission;
        public static int Score;
        public static int Target;
        public static int Stage;

        // Loading Player's data and Initialize managers
        private void Awake()
        {
            _player = DataManager.LoadData();
            _bNumbers = new BlockNumbers(_player.getTileCube);
            _levelManager.Init(_gameArea);
            _inputManager.Init(_bNumbers,_gameArea,_canvas);
            
        }

        // Loading UI and putting Stage Status
        private void Start()
        {
            GameStatus = GameState.PLAYING;
            Stage = _player.Stage;
            Score = _player.Score;

            if(LoadedGame.isLoaded)
                _levelManager.GenerateStageFromData(_bNumbers, _player);

            else
                _levelManager.GenerateStage(_bNumbers);
            
            LoadUiElements();
        }

        // Loading UI
        public void LoadUiElements()
        {
            GameObject score = _canvas.transform.GetChild(0).GetChild(0).gameObject;
            GameObject target = _canvas.transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
            GameObject stage = _canvas.transform.GetChild(1).GetChild(1).GetChild(0).gameObject;

            score.GetComponent<Text>().text = Score.ToString();
            stage.GetComponent<Text>().text = "Stage "+ _player.Stage;
            target.GetComponent<Text>().text = TargetSelector().ToString();

            Target = TargetSelector();
        }

        // Detect Target depending on Stage
        public int TargetSelector()
        {
            int target;
            if (_player.Stage < 3)
            {
                target = _player.Stage * 1000;
            }
            else
            {
                target = (_player.Stage * 500) + ((_player.Stage-1) * 1000);
            }

            return target;
        }

        // Update player data by increasing stage, meaning player passed stage.
        public void UpdatePlayerData()
        {
            LoadedGame.isLoaded = false;
            _player.Stage = Stage + 1;
            _player.Score = Score;
            DataManager.SaveData(_player);
        }

        // Clear player data, meaning player wants to play from beginning.
        public void ClearPlayerData()
        {
            LoadedGame.isLoaded = false;
            _player = new Player(1, 0, new TileCube());
            DataManager.SaveData(_player);
        }

        // Save player data, by holding GameObject positions.
        // Vector classes are not serializable that's why positions are kept string
        public void SavePlayerData()
        {
            var numbers = _bNumbers.GetNumbers();
            _player.PositionOfNumbers.Clear();
            foreach (var number in numbers)
            {
                string numberVector2Position = number.Key.x + "," + number.Key.y;
                string numberGameObjectPosition =
                    number.Value.transform.localPosition.x + "," + number.Value.transform.localPosition.y;

                _player.PositionOfNumbers.Add(numberVector2Position, numberGameObjectPosition);
            }

            _player.Stage = Stage;
            _player.Score = Score;
            DataManager.SaveData(_player);
        }

        // When Player closes app, save the data.
        void OnApplicationQuit()
        {
            print("Player exited, Saving Last Stage of Player...");
            SavePlayerData();
        }
    }
}
