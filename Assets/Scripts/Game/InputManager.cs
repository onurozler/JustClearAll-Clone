
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Assets.Scripts.Game
{
    // Input Manager controls Player Interaction and Actions on UI and Game

    public class InputManager : MonoBehaviour
    {
        private BlockNumbers _blockNumbers;
        private GameObject _gameArea;
        private GameObject _canvas;
        

        public void Init(BlockNumbers blockNumbers, GameObject area, GameObject canvas)
        {
            _blockNumbers = blockNumbers;
            _gameArea = area;
            _canvas = canvas;
        }

        private void Update()
        { 
            // If Player plays the game, enable him to click buttons
            if (GameManager.GameStatus == GameState.PLAYING)
            {
                // Detect which Button to Click
                if (Input.GetMouseButtonDown(0))
                {
                    // Disable Grid Layout Group to fall Buttons physically
                    if (_gameArea.GetComponent<GridLayoutGroup>().enabled)
                        _gameArea.GetComponent<GridLayoutGroup>().enabled = false;

                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                    if (hit.collider != null)
                    {
                        if (hit.collider.CompareTag("Clickable"))
                        {
                            GameObject clicked = hit.collider.gameObject;
                            //clicked.transform.Translate(Vector2.left * 0.7f);
                            if (AreTheyInSameGroup(clicked))
                                DoubleClickSelectedNumbers(clicked);
                            else
                                HighlightSelectedNumbers(clicked);
                        }
                    }
                }
            }
        }

        // ************************************** Game Actions *************************************************
        private List<GameObject> _clickCheck = new List<GameObject>();
        public bool AreTheyInSameGroup(GameObject item)
        {
            if (_clickCheck == null) return false;
            return _clickCheck.Contains(item);
        }

        public void HighlightSelectedNumbers(GameObject clicked)
        {
            Vector2Int positionClicked = _blockNumbers.GetPositionByElement(clicked);
            List<GameObject> selectedElements = _blockNumbers.GetSelectedBlockNumbers(positionClicked);
            

            if (selectedElements == null) return;

            int numberOfElement = int.Parse(selectedElements[0].transform.GetChild(0).GetComponent<Text>().text);
            ShowSelectedBlocks(numberOfElement, selectedElements.Count);

            _clickCheck.Clear();
            _clickCheck.AddRange(selectedElements);
        }

        public void DoubleClickSelectedNumbers(GameObject clicked)
        {
            Vector2Int positionClicked = _blockNumbers.GetPositionByElement(clicked);
            List<GameObject> selectedElements = _blockNumbers.GetSelectedBlockNumbers(positionClicked);

            _blockNumbers.IncreaseNumber(positionClicked);

            var deletedElements = selectedElements.Where(x => x != clicked);
            foreach (var element in deletedElements)
            {
                _blockNumbers.RefreshElements(_blockNumbers.GetPositionByElement(element));
                _blockNumbers.RemoveElement(element);
                Destroy(element);
            }

            _blockNumbers.RepositionElements();
            _clickCheck.Clear();


            if (!_blockNumbers.CanMove())
                FinishedStage();

            
            for (int i = 0; i < 8; i++)
            {
                print(_blockNumbers._tileCube.GetCubes()[i, 0] + " " + _blockNumbers._tileCube.GetCubes()[i, 1] + " " + _blockNumbers._tileCube.GetCubes()[i, 2] + " " +_blockNumbers._tileCube.GetCubes()[i, 3] + " " +_blockNumbers._tileCube.GetCubes()[i, 4] + " " +_blockNumbers._tileCube.GetCubes()[i, 5] + " " +_blockNumbers._tileCube.GetCubes()[i, 6] + " " + _blockNumbers._tileCube.GetCubes()[i, 7]);
            }
            
        }


        // ************************************** UI Interactions *************************************************

        public void PauseOrContinueGame(bool isPausing)
        {
            GameObject pausePanel = _canvas.transform.GetChild(2).gameObject;
            if (isPausing)
            {
                GameManager.GameStatus = GameState.PAUSING;
                pausePanel.SetActive(true);
            }
            else
            {
                GameManager.GameStatus = GameState.PLAYING;
                pausePanel.SetActive(false);
            }
        }

        public void ShowSelectedBlocks(int number,int count)
        {
            Text selectedBlocksText = _canvas.transform.GetChild(1).GetChild(0).GetComponent<Text>();
            selectedBlocksText.text = count + " Tiles" + " +" + (number * 5) * count;
        }

        public void ScoreUpdate(int score)
        {
            Text scoreText = _canvas.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            scoreText.text = score.ToString();
        }

        public void ToMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }

        public void FinishedStage()
        {
            GameManager.GameStatus = GameState.PAUSING;
        }
    }
}