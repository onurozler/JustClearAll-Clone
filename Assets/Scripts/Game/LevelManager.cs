using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Assets.Scripts.Classes;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
    // Level Manager is responsible for creating levels and loading previous game.

    public class LevelManager : MonoBehaviour
    {
        // Game Area reference
        public GameObject GameArea;

        // Private values
        private Player _player;
        private Dictionary<Vector2Int, GameObject> _cubeButtons;

        // Loading save data to generate Levels
        public void Awake()
        {
            _player = DataManager.LoadData();
            _cubeButtons = new Dictionary<Vector2Int, GameObject>();
            GenerateLevel(_player.getTileCube);
        }

        public void Start()
        {

        }

        public void GenerateLevel(TileCube tileCube)
        {
            
            GameArea.GetComponent<GridLayoutGroup>().enabled = true;

            GameObject tileBackground = Resources.Load("Prefabs/TileBackground") as GameObject;
            GameObject tileBlock = Resources.Load("Prefabs/TileBlock") as GameObject;

            // Generate Tiles 8x8 Array with Stage
            int[,] cubes = tileCube.GenerateTilesWithStage(1);

            // Match this array with dynamically created Buttons
            for (int i = 0; i < cubes.GetLength(0); i++)
            {
                for (int j = 0; j < cubes.GetLength(1); j++)
                {
                    GameObject cube;
                    if (cubes[i, j] != 0)
                    {
                        int number = cubes[i, j];
                        cube = Instantiate(tileBlock, GameArea.transform);
                        cube.transform.GetChild(0).GetComponent<Text>().text = number.ToString();
                        cube.transform.GetComponent<Image>().color = BlockNumberGenerator.GetColor(number);
                    }

                    else
                        cube = Instantiate(tileBackground, GameArea.transform);

                    _cubeButtons.Add(new Vector2Int(i, j), cube);
                    
                }
            }
        }
    }
}
