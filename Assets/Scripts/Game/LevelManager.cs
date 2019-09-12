
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
        public void GenerateStage(BlockNumbers bNumbers)
        {
            bNumbers.Clear();
            _gameArea.GetComponent<GridLayoutGroup>().enabled = true;

            // Generate Tiles 8x8 Array with Stage
            int[,] cubes = bNumbers.GenerateElementsWithStage(GameManager.Stage);

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

        public void GenerateStageFromData(BlockNumbers bNumbers, Player p)
        {
            bNumbers.Clear();
            _gameArea.GetComponent<GridLayoutGroup>().enabled = false;

            var cubes = bNumbers.getTileCube.GetCubes();
            var positionOfNumbers = p.PositionOfNumbers;

            for (int i = 0; i < cubes.GetLength(0); i++)
            {
                for (int j = 0; j < cubes.GetLength(1); j++)
                {
                    if (cubes[i, j] != 0)
                    {
                        foreach (var savedPositions in positionOfNumbers)
                        {
                            // x=0, y=1 indexes
                            string[] vector2Position = savedPositions.Key.Split(',');

                            if (int.Parse(vector2Position[0]) == i && int.Parse(vector2Position[1]) == j)
                            {
                                int number = cubes[i, j];

                                GameObject cube = Instantiate(_tileBlock, _gameArea.transform);
                                cube.GetComponent<RectTransform>().sizeDelta = new Vector2(93.5f,93.5f);

                                bNumbers.SetCubeProperties(cube, number);
                                bNumbers.AddElement(new Vector2Int(i, j), cube);

                                string[] vector3Position = savedPositions.Value.Split(',');
                                int cubeX = int.Parse(vector3Position[0]);
                                int cubeY = int.Parse(vector3Position[1]);


                                cube.transform.localPosition = new Vector3(cubeX,  (_gameArea.GetComponent<RectTransform>().rect.height/2) - i*20);


                            }
                        }
                    }
                }
            }

        }

    }
}
