
using System;
using System.Collections.Generic;
using Assets.Scripts.Classes;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Assets.Scripts.Game
{
    // Block Number class is connection between 2D array and GameObjects represented in Scene

    public class BlockNumbers
    {
        
        private Dictionary<Vector2Int, GameObject> _cubeButtons;
        private TileCube _tileCube;

        public BlockNumbers(TileCube tileCube)
        {
            _cubeButtons= new Dictionary<Vector2Int, GameObject>();
            this._tileCube = tileCube;
        }

        // Getter & Setters

        public TileCube getTileCube
        {
            get { return _tileCube; }
        }

        public Dictionary<Vector2Int, GameObject> GetNumbers()
        {
            return _cubeButtons;
        }

        public void AddElement(Vector2Int position, GameObject element)
        {
            _cubeButtons.Add(position,element);
        }


        public void RemoveElement(GameObject element)
        {
            _cubeButtons.Remove(GetPositionByElement(element));
        }

        public void Clear()
        {
            _cubeButtons.Clear();
        }

        public int GetLength()
        {
            return _cubeButtons.Count();
        }

        // Refreshes array with deleted block tiles
        public void RefreshElements(Vector2Int position)
        {
             _tileCube.DeleteSelectedBlockTiles(position);
        }

        // Can Move is detecting if player can concatenate blocks
        public bool CanMove()
        {
            foreach (var position in _cubeButtons.Keys)
            {
                if (GetSelectedBlockNumbers(position) != null)
                    return true;
            }

            return false;
        }

        // After player move, this function Repositions GameObjects according to 2D array data.
        public void RepositionElements()
        {
            var newAndOldPositions = _tileCube.RepositionTiles();
            var tempDict = new Dictionary<Vector2Int, GameObject>();
            foreach (var position in newAndOldPositions)
            {
                // No need to check if its old position and new position is same.
                if (position.Key == position.Value)
                    continue;

                // Finding its old position, and delete it from original dictionary and added to temporary
                // with new position
                var element = _cubeButtons.Where(x => x.Key == position.Key)?.SingleOrDefault();
                _cubeButtons.Remove(element.Value.Key);
                tempDict.Add(position.Value,element.Value.Value);
            }

            // Updates Original Dictionary with new position values by using temp dict.
            foreach (var temp in tempDict)
            {
                _cubeButtons.Add(temp.Key,temp.Value);
            }
        }

        // After player move, this function concatenate columns and Reposition some GameObjects
        // according to 2D array data.

        public void RepositionColumns()
        {
            // Repeat the process depending on empty column count
            for (int i = 0; i <= _tileCube.GetEmptyColumnsCount(); i++)
            {
                var columnUpdate = _tileCube.RepositionColumnTiles();
                if (columnUpdate != null)
                {
                    var tempDict = new Dictionary<Vector2Int, GameObject>();
                    foreach (var position in columnUpdate)
                    {
                        // Deleting GameObject with old position and added to temp dict with new position.
                        var element = _cubeButtons.Where(x => x.Key == position.Key)?.SingleOrDefault();
                        _cubeButtons.Remove(element.Value.Key);
                        tempDict.Add(position.Value, element.Value.Value);
                    }

                    // Updates Original Dictionary with new position values by using temp dict.
                    foreach (var temp in tempDict)
                    {
                        temp.Value.transform.Translate(Vector2.left * 0.7f);
                        _cubeButtons.Add(temp.Key, temp.Value);
                    }
                }
            }
        }

        // Returns 2D array generation data.
        public int[,] GenerateElementsWithStage(int stage)
        {
            return _tileCube.GenerateTilesWithStage(stage);
        }

        // Returns GameObject by position.
        public Vector2Int GetPositionByElement(GameObject element)
        {
            Vector2Int position = _cubeButtons.FirstOrDefault(s => s.Value == element).Key;
            return position;
        }

        // Takes positions from 2D data, and determines selected GameObjects depending on Position.
        public List<GameObject> GetSelectedBlockNumbers(Vector2Int position)
        {
            List<GameObject> selectedBlockNumbers = new List<GameObject>();
            List<Vector2Int> positionOfSelectedNumbers = _tileCube.GetSelectedBlockTiles(position);

            // If selected blocks lower than 2, player can't move.
            if (positionOfSelectedNumbers.Count < 2)
                return null;

            foreach (var positionSelected in positionOfSelectedNumbers)
            {
                selectedBlockNumbers.Add(_cubeButtons[positionSelected]);
            }

            return selectedBlockNumbers;

        }

        // After player concatenate blocks, it increases number of selected block and change its color depending on number.
        public void IncreaseNumber(Vector2Int position)
        {
            GameObject increasedNumber = _cubeButtons[position];
            int number = int.Parse(increasedNumber.transform.GetChild(0).GetComponent<Text>().text);
            SetCubeProperties(increasedNumber,number+1);

            _tileCube.SetValue(position,number+1);
        }

        // Sets GameObject properties, its color and number to be shown in text.
        public void SetCubeProperties(GameObject cube, int number)
        {
            cube.transform.GetChild(0).GetComponent<Text>().text = number.ToString();
            cube.transform.GetComponent<Image>().color = SetColor(number);
        }

        // Sets Random colors
        private Color SetColor(int number)
        {
            float colorChooserB = number * 0.05f;
            float colorChooserG = -number * 0.03f;
            float colorChooserR = number * 0.08f;
            if (number % 2 == 0)
            {
                colorChooserB = number * 0.08f;
                colorChooserG = number * 0.05f;
                colorChooserR = -number * 0.03f;
            }

            return new Color(0.5f + colorChooserR, 0.5f + colorChooserG, 0.5f + colorChooserB);
        }

    }

}
