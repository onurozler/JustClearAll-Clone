using System;
using System.Collections.Generic;
using System.Linq;
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

        public int GetEmptyColumnsCount()
        {
            int number = 0;
            for (int j = 0; j < COLUMN - 1; j++)
            {
                if (_tiles[7, j] == 0)
                {
                    number++;
                }
            }

            return number;
        }

        public int[,] GenerateTilesWithStage(int stage)
        {
            // Very Basic Probability System that keeping low numbers probability is high to generate
            // Finding Ratio of numbers
            Dictionary<int,int> probabilityOfNumbers = new Dictionary<int, int>();
            int percentage = 100;
            int highChance = 40;

            for (int i = 1; i < stage + 2; i++)
            {
                highChance /= i;
                int percentageOfI = UnityEngine.Random.Range(highChance, percentage);
                percentage -= percentageOfI;
                
                probabilityOfNumbers.Add(i,percentageOfI);
            }
            probabilityOfNumbers.Add(stage + 2, percentage);


            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COLUMN; j++)
                {
                    int random = UnityEngine.Random.Range(0, 50);
                    foreach (var probability in probabilityOfNumbers.Reverse())
                    {
                        if (random < probability.Value)
                        {
                            _tiles[i, j] = probability.Key;
                            break;
                        }
                        else
                        {
                            _tiles[i, j] = 1;
                        }
                    }
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
        }


        public Dictionary<Vector2Int,Vector2Int> RepositionTiles()
        {
            // Holding values in Queue
            Queue<int> columnQueue = new Queue<int>();

            // Holding new and old positions to update Numbers too
            List<Vector2Int> oldPosition = new List<Vector2Int>();
            List<Vector2Int> newPosition = new List<Vector2Int>();
            
            // Reposition Tiles from top to down 
            for (int j = 0; j < COLUMN; j++)
            {
                for (int i = 0; i < ROW ; i++)
                {
                    if (_tiles[i, j] != 0)
                    {
                        columnQueue.Enqueue(_tiles[i,j]);
                        oldPosition.Add(new Vector2Int(i,j));
                        _tiles[i, j] = 0;
                    }
                }

                for (int k = ROW - columnQueue.Count; k < ROW; k++)
                {
                    _tiles[k, j] = columnQueue.Dequeue();
                    newPosition.Add(new Vector2Int(k,j));
                }
            }

            var returnDict = oldPosition.Zip(newPosition, (k, v) => new {Key = k, Value = v}).
                ToDictionary(x=> x.Key, x=>x.Value);

            return returnDict;
        }

        public Dictionary<Vector2Int, Vector2Int> RepositionColumnTiles()
        {
            // Holding new and old positions to update Numbers too
            Dictionary<Vector2Int,Vector2Int> oldAndNewPosition = new Dictionary<Vector2Int, Vector2Int>();

                for (int j = 0; j < COLUMN - 1; j++)
                {
                    if (_tiles[7, j] == 0)
                    {
                        for (int i = 0; i < ROW; i++)
                        {
                            if (_tiles[i, j + 1] != 0)
                            {
                                _tiles[i, j] = _tiles[i, j + 1];
                                _tiles[i, j + 1] = 0;
                                oldAndNewPosition.Add(new Vector2Int(i, j + 1), new Vector2Int(i, j));
                            }
                        }
                    }
                }

            if(oldAndNewPosition.Count>0)
                return oldAndNewPosition;

           return null;

        }
    }
}
