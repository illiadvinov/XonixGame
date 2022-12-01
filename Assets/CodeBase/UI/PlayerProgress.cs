using System;
using CodeBase.StateMachine;
using CodeBase.StateMachine.States;
using TMPro;
using Zenject;

namespace CodeBase.UI
{
    public class PlayerProgress
    {
        private readonly GameStateMachine gameStateMachine;
        private readonly TMP_Text paintedPercentageText;
        private int needPercentage = 60;

        [Inject]
        public PlayerProgress(GameStateMachine gameStateMachine,
            [Inject(Id = "PaintedPercentage")] TMP_Text paintedPercentageText)
        {
            this.gameStateMachine = gameStateMachine;
            this.paintedPercentageText = paintedPercentageText;
        }

        public void SetPaintProgress(int progress)
        {
            paintedPercentageText.text = $"{progress.ToString()}%";
            if (progress > needPercentage)
                gameStateMachine.Enter<LevelState>();
        }
    }
}