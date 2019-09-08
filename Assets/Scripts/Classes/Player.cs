using System;

namespace Assets.Scripts.Classes
{
    // Player Class to keep player's data and using data for loading game

    [Serializable]
    public class Player
    {
        // Properties of Player
        private int _stage;
        private int _score;
        private TileCube _tileCube;

        // Constructor
        public Player(int stage, int score, TileCube tileCube)
        {
            this._stage = stage;
            this._score = score;
            this._tileCube = tileCube;
        }

        // Getter & Setter

        public int setStage
        {
            set { _stage = value; }
        }

        public int getStage
        {
            get {return _stage; }
        }

        public int setScore
        {
            set { _score = value; }
        }

        public int getScore
        {
            get { return _score; }
        }
        public TileCube getTileCube
        {
            get { return _tileCube; }
        }
    }


}
