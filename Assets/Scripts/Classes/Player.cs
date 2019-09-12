using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace Assets.Scripts.Classes
{
    // Player Class to keep player's data and using data for loading game

    [Serializable]
    public class Player
    {
        // Properties of Player
        private int _stage;
        private int _score;
        private TileCube _tileCubes;
        public Dictionary<string, string> PositionOfNumbers { get; set; }

        // Constructor
        public Player(int stage, int score, TileCube tileCube)
        {
            this._stage = stage;
            this._score = score;
            this._tileCubes = tileCube;
            this.PositionOfNumbers = new Dictionary<string, string>();
        }

        // Getter & Setter

        public int Stage
        {
            get { return _stage; }
            set { _stage = value; }
        }

        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public TileCube getTileCube
        {
            get { return _tileCubes; }
        }
        public TileCube setTileCube
        {
            set { _tileCubes = value; }
        }
    }
}
