using Assets.Scripts.Classes;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
    // Game Manager is responsible for controlling Input Manager, Level Manager and Game Mechanics

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

        private void Awake()
        {
            _player = DataManager.LoadData();
            _bNumbers = new BlockNumbers(_player.getTileCube);
            _levelManager.Init(_gameArea);
            _inputManager.Init(_bNumbers,_gameArea,_canvas);
            
        }

        private void Start()
        {
            GameStatus = GameState.PLAYING;
            _levelManager.GenerateStage(_bNumbers,_player.getStage);
            LoadUiElements();

        }

        public void LoadUiElements()
        {
            GameObject score = _canvas.transform.GetChild(0).GetChild(0).gameObject;
            GameObject target = _canvas.transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
            GameObject stage = _canvas.transform.GetChild(1).GetChild(1).GetChild(0).gameObject;

            score.GetComponent<Text>().text = Score.ToString();
            stage.GetComponent<Text>().text = "Stage "+ _player.getStage;
            target.GetComponent<Text>().text = TargetSelector().ToString();
        }

        public int TargetSelector()
        {
            int target;
            if (_player.getStage < 3)
            {
                target = _player.getStage * 1000;
            }
            else
            {
                target = _player.getStage * 500;
            }

            return target;
        }
                
    }
}
