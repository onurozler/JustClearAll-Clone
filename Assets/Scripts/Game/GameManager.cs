using System.Collections.Generic;
using Assets.Scripts.Classes;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = System.Numerics.Vector2;
using Vector3 = System.Numerics.Vector3;

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

            if(_player.PositionOfNumbers.Count>0)
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
                target = _player.Stage * 500;
            }

            return target;
        }

        public void UpdatePlayerData()
        {
            _player.Stage = Stage + 1;
            DataManager.SaveData(_player);
        }

        public void ClearPlayerData()
        {
            _player = new Player(1, 0, new TileCube());
            DataManager.SaveData(_player);
        }

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

        void OnApplicationQuit()
        {
            print("Player exited, Saving Last Stage of Player...");
            SavePlayerData();
        }
    }
}
