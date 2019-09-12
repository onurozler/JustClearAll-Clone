
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
            ShowSelectedBlocks(selectedElements.Count, CalculatePoint(numberOfElement, selectedElements.Count));

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
            _blockNumbers.RepositionColumns();
            _clickCheck.Clear();



            for (int i = 0; i < 8; i++)
            {
                print(_blockNumbers.getTileCube.GetCubes()[i, 0] + " " + _blockNumbers.getTileCube.GetCubes()[i, 1] + " " + _blockNumbers.getTileCube.GetCubes()[i, 2] + " " + _blockNumbers.getTileCube.GetCubes()[i, 3] + " " + _blockNumbers.getTileCube.GetCubes()[i, 4] + " " + _blockNumbers.getTileCube.GetCubes()[i, 5] + " " + _blockNumbers.getTileCube.GetCubes()[i, 6] + " " + _blockNumbers.getTileCube.GetCubes()[i, 7]);
            }



            // Update Score

            int numberOfElement = int.Parse(selectedElements[0].transform.GetChild(0).GetComponent<Text>().text);
            int addedScore = CalculatePoint(numberOfElement, selectedElements.Count);
            ScoreUpdate(addedScore);

            if (!_blockNumbers.CanMove())
                FinishedStage(_blockNumbers.GetLength());

            
        }


        // ************************************** UI Interactions *************************************************

        // Pause or Continue game depending on input from buttons
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

        // Show selected blocks on top and its points that player can get
        public void ShowSelectedBlocks(int count,int point)
        {
            Text selectedBlocksText = _canvas.transform.GetChild(1).GetChild(0).GetComponent<Text>();
            selectedBlocksText.text = count + " Tiles + " + point;
        }

        // Update Scores and Score Label on UI
        public void ScoreUpdate(int score)
        {
            GameManager.Score += score;

            Text scoreText = _canvas.transform.GetChild(0).GetChild(0).GetComponent<Text>();
            scoreText.text = GameManager.Score.ToString();
        }

        // Returns to Menu
        public void ToMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }

        // Load Game Scene
        public void LoadGameScene()
        {
            SceneManager.LoadScene("Game");
        }

        // Showing Successfull or fail pop up according to last score
        public void ShowLastPopUp(int extra)
        {
            GameObject LastPopUp;
            int score = 0;
            score += extra > 0 ? extra : -extra;
            string scoreOp = extra > 0 ? " + " : " - ";

            if (GameManager.Mission == Mission.SUCCESSFULL)
            {
                LastPopUp = _canvas.transform.GetChild(3).gameObject;
                LastPopUp.transform.GetChild(2).GetComponent<Text>().text = "SCORE = " + (GameManager.Score + score) + scoreOp + score;
                LastPopUp.transform.GetChild(3).GetComponent<Text>().text = "TOTAL = " + GameManager.Score.ToString();
            }
            else
            {
                LastPopUp = _canvas.transform.GetChild(4).gameObject;
                LastPopUp.transform.GetChild(2).GetComponent<Text>().text = "SCORE = " + (GameManager.Score + score) + scoreOp + score;
                LastPopUp.transform.GetChild(3).GetComponent<Text>().text = "TOTAL = " + GameManager.Score.ToString();
            }
            LastPopUp.SetActive(true);
        }


        // ************************************** Calculations *************************************************
        // Calculate number point depending on count
        public int CalculatePoint(int number,int count)
        {
            int total = number * 5 * count;
            if (count>4 && count < 10)
            {
                total *= 2;
            }
            else if (count > 9 && count < 15)
            {
                total *= 3;
            }
            else if(count > 14)
            {
                total *= 4;
            }

            return total;
        }

        // Calculate Extra point, if there is no number, then player can get extra points. But if there
        // is left player get punishment points.
        public int CalculateExtraPoint(int count)
        {
            int total = 0;
            if (count == 0)
                total = 1000 + ((GameManager.Stage - 1) * 500);

            int penaltyPoint = 100 + ((GameManager.Stage - 1) * 50);

            penaltyPoint *= count;

            total -= penaltyPoint;

            return total;
        }

        // Calculate total Score and decide if Player is Successfull or not.
        public void FinishedStage(int leftElements)
        {
            GameManager.GameStatus = GameState.PAUSING;
            int extra = CalculateExtraPoint(leftElements);
            GameManager.Score += extra;

            GameManager.Mission = GameManager.Score > GameManager.Target ? Mission.SUCCESSFULL : Mission.FAIL;

            ShowLastPopUp(extra);
        }
    }
}