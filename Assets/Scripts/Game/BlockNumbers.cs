
using System.Collections.Generic;
using Assets.Scripts.Classes;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Assets.Scripts.Game
{
    public class BlockNumbers
    {
        
        private Dictionary<Vector2Int, GameObject> _cubeButtons;
        public TileCube _tileCube;

        public BlockNumbers(TileCube tileCube)
        {
            _cubeButtons= new Dictionary<Vector2Int, GameObject>();
            this._tileCube = tileCube;
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

        public void RefreshElements(Vector2Int position)
        {
             _tileCube.DeleteSelectedBlockTiles(position);
        }

        public bool CanMove()
        {
            foreach (var position in _cubeButtons.Keys)
            {
                if (GetSelectedBlockNumbers(position) != null)
                    return true;
            }

            return false;
        }

        public void RepositionElements()
        {
            var newAndOldPositions = _tileCube.RepositionTiles();
            var tempDict = new Dictionary<Vector2Int, GameObject>();

            /*
            var columnUpdate = _tileCube.RepositionColumnTiles();
            foreach (var position in columnUpdate)
            {
                for (int i = 0; i < newAndOldPositions.Count; i++)
                {
                    
                }
            }*/

            foreach (var position in newAndOldPositions)
            {
                // No need to check if its old position and new position is same.
                if (position.Key == position.Value)
                    continue;

                var element = _cubeButtons.Where(x => x.Key == position.Key)?.SingleOrDefault();
                _cubeButtons.Remove(element.Value.Key);
                tempDict.Add(position.Value,element.Value.Value);
            }

            foreach (var temp in tempDict)
            {
                _cubeButtons.Add(temp.Key,temp.Value);
            }
        }


        public int[,] GenerateElementsWithStage(int stage)
        {
            return _tileCube.GenerateTilesWithStage(stage);
        }

        public Vector2Int GetPositionByElement(GameObject element)
        {
            Vector2Int position = _cubeButtons.FirstOrDefault(s => s.Value == element).Key;
            return position;
        }


        public List<GameObject> GetSelectedBlockNumbers(Vector2Int position)
        {
            List<GameObject> selectedBlockNumbers = new List<GameObject>();
            List<Vector2Int> positionOfSelectedNumbers = _tileCube.GetSelectedBlockTiles(position);

            if (positionOfSelectedNumbers.Count < 2)
                return null;

            foreach (var positionSelected in positionOfSelectedNumbers)
            {
                selectedBlockNumbers.Add(_cubeButtons[positionSelected]);
            }

            return selectedBlockNumbers;

        }

        public void IncreaseNumber(Vector2Int position)
        {
            GameObject increasedNumber = _cubeButtons[position];
            int number = int.Parse(increasedNumber.transform.GetChild(0).GetComponent<Text>().text);
            SetCubeProperties(increasedNumber,number+1);

            _tileCube.SetValue(position,number+1);
        }


        public void SetCubeProperties(GameObject cube, int number)
        {
            cube.transform.GetChild(0).GetComponent<Text>().text = number.ToString();
            cube.transform.GetComponent<Image>().color = SetColor(number);
        }

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
