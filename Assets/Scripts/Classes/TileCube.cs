using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Classes
{
    [Serializable]
    public class TileCube
    {
        private const int ROW = 8;
        private const int COLUMN = 8;

        private int[,] _tiles;

        public TileCube()
        {
            _tiles = new int[ROW,COLUMN];
        }

        public int GetValue(Vector2Int index)
        {
            return _tiles[index.x,index.y];
        }

        public void SetValue(Vector2Int index, int value)
        {
            _tiles[index.x,index.y] = value;
        }

        public int[,] GetCubes()
        {
            return _tiles;
        }

        public int[,] GenerateTilesWithStage(int stage)
        {
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COLUMN; j++)
                {
                    _tiles[i,j] = UnityEngine.Random.Range(1, stage+3);
                }
            }
            // Selecting Target Block
            int targetX = UnityEngine.Random.Range(0, ROW);
            int targetY = UnityEngine.Random.Range(0, COLUMN);

            _tiles[targetX, targetY] = stage + 3;

            return _tiles;
        }
        public List<Vector2Int> GetSelectedBlockTiles(Vector2Int index)
        {
            int[,] temp = _tiles.Clone() as int[,];
            List<Vector2Int> returnPositionOfSelectedCubes = new List<Vector2Int>();  
            BlockSelector.FloodFill(temp,index.x,index.y);

            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COLUMN; j++)
                {
                    if (temp[i,j] == -1)
                        returnPositionOfSelectedCubes.Add(new Vector2Int(i,j));
                }
            }

            return returnPositionOfSelectedCubes;
        }

        public void DeleteSelectedBlockTiles(Vector2Int index)
        {
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COLUMN; j++)
                {
                    if (i == index.x && j == index.y)
                        _tiles[i, j] = 0;
                }
            }
            RepositionTiles();
        }

        public void RepositionTiles()
        {
            for (int j = 0; j < COLUMN; j++)
            {
                for (int i = 0; i < ROW - 1; i++)
                {
                    if (_tiles[i, j] != 0 && _tiles[i+1,j] == 0)
                    {
                        _tiles[i+1, j] = _tiles[i, j];
                        _tiles[i, j] = 0;
                    }
                }
            }
        }
    }
}
