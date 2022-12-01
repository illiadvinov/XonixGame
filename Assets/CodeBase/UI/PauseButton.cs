using System;
using UnityEngine;

namespace CodeBase.UI
{
    public class PauseButton : MonoBehaviour
    {
        private bool isClicked = false;
        private float cacheTimeScale;

        private void Start() =>
            cacheTimeScale = Time.timeScale;

        public void Pause()
        {
            if (!isClicked)
            {
                Time.timeScale = 0;
                isClicked = true;
            }
            else if (isClicked)
            {
                Time.timeScale = cacheTimeScale;
                isClicked = false;
            }
        }
    }
}