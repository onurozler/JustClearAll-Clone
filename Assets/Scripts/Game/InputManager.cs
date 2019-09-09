
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.Game
{
    // Input Manager controls Player Interaction and Actions on UI and Game

    public class InputManager : MonoBehaviour
    {
        private BlockNumbers _blockNumbers;
        private GameObject _gameArea;

        public void Init(BlockNumbers blockNumbers, GameObject area)
        {
            _blockNumbers = blockNumbers;
            _gameArea = area;
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
                            if(AreTheyInSameGroup(clicked))
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
                _blockNumbers.RefreshElements(positionClicked);
                Destroy(element);
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    print(_blockNumbers._tileCube.GetCubes()[i,j] + " x: " + i + " y: "+ j);
                }
            }
            
        }

        // ************************************** UI Interactions *************************************************

        public void PauseGame()
        {
            GameManager.GameStatus = GameState.PAUSING;
        }

        public void ContinueGame()
        {
            GameManager.GameStatus = GameState.PLAYING;
        }
    }
}