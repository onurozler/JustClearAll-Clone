using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    // Input Manager controls Player and Interaction in UI or Game

    public class InputManager : MonoBehaviour
    {
        public Canvas UI;

        public delegate void ClickBlock();
        public static event ClickBlock onBlockClicked;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
