using System;
using System.Collections;
using CodeBase.Coroutine;
using CodeBase.StateMachine;
using CodeBase.StateMachine.States;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI
{
    public class TimerController
    {
        public event Action OnTimerStop;
        private readonly ICoroutineRunner coroutineRunner;
        private readonly TMP_Text timerText;
        private float currentTime;
        private float startingTime = 60;
        private UnityEngine.Coroutine coroutine;

        [Inject]
        public TimerController(ICoroutineRunner coroutineRunner,
            [Inject(Id = "Timer")] TMP_Text timerText)
        {
            this.coroutineRunner = coroutineRunner;
            this.timerText = timerText;
        }

        public void StartTimer()
        {
            currentTime = startingTime;
            coroutine = coroutineRunner.StartCoroutine(StartCountdown());
        }

        public void StopTimer()
        {
            coroutineRunner.StopCoroutine(coroutine);
        }

        private IEnumerator StartCountdown()
        {
            while (true)
            {
                if (currentTime <= 0)
                {
                    OnTimerStop?.Invoke();
                    break;
                }

                currentTime -= 1 * Time.deltaTime;
                timerText.text = currentTime.ToString("0");
                yield return null;
            }
        }
    }
}