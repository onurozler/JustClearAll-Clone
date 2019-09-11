
using UnityEngine;
using Assets.Scripts.Classes;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
    // Level Manager is responsible for creating levels and loading previous game.

    public class LevelManager : MonoBehaviour
    {

        // Background and Block loaded from Prefabs
        private GameObject _tileBackground;
        private GameObject _tileBlock;
        private GameObject _gameArea;


        public void Init(GameObject area)
        {
            _gameArea = area;
            _tileBlock = Resources.Load("Prefabs/TileBlock") as GameObject;
        }

        // Generate Level Loading from data
        public void GenerateStage(BlockNumbers bNumbers, int stage)
        {
            bNumbers.Clear();
            GameManager.Score = 0;

            _gameArea.GetComponent<GridLayoutGroup>().enabled = true;

            // Generate Tiles 8x8 Array with Stage
            int[,] cubes = bNumbers.GenerateElementsWithStage(stage);

            // Match Block Numbers and TileCubes by using dynamically created Buttons
            for (int i = 0; i < cubes.GetLength(0); i++)
            {
                for (int j = 0; j < cubes.GetLength(1); j++)
                {
                    if (cubes[i, j] != 0)
                    {
                        int number = cubes[i, j];
                        GameObject cube = Instantiate(_tileBlock, _gameArea.transform);
                        bNumbers.SetCubeProperties(cube, number);
                        bNumbers.AddElement(new Vector2Int(i, j), cube);
                    }
                }
            }

        }

        public void GenerateNewStage(BlockNumbers bNumbers)
        {

        }
    }
}
