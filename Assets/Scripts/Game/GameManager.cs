using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    // Game Manager is responsible for controlling Input Manager, Level Manager and Game Mechanics

    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private LevelManager _levelManager;

        // Start is called before the first frame update
        void Start()
        {
            
        }

    }
}
