using Assets.Scripts.Classes;
using UnityEngine;

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

        // Private values
        private Player _player;
        private BlockNumbers _bNumbers;

        // Game State to be accessible by other managers
        public static GameState GameStatus;

        private void Awake()
        {
            _player = DataManager.LoadData();
            _bNumbers = new BlockNumbers(_player.getTileCube);
            _levelManager.Init(_gameArea);
            _inputManager.Init(_bNumbers,_gameArea);

        }

        private void Start()
        {
            GameStatus = GameState.PLAYING;
            _levelManager.GenerateStage(_bNumbers,_player.getStage);

        }
                
    }
}
