using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.Player
{
    public class PlayerHealth
    {
        public int Health { get; private set; }
        public event Action OnHealthDecreased;
        private readonly InputSystem.InputSystem inputSystem;
        private readonly PlayerMovement playerMovement;
        private readonly TMP_Text livesLeftText;

        [Inject]
        public PlayerHealth(InputSystem.InputSystem inputSystem,
            PlayerMovement playerMovement,
            [Inject(Id = "LivesLeft")] TMP_Text livesLeftText)
        {
            this.inputSystem = inputSystem;
            this.playerMovement = playerMovement;
            this.livesLeftText = livesLeftText;
            inputSystem.OnWrongInput += DecreaseHealth;
        }

        public void DecreaseHealth()
        {
            Health -= 1;
            livesLeftText.text = Health.ToString();
            playerMovement.WasHit = false;

            if (Health <= 0)
            {
                playerMovement.WasHit = true;
                OnHealthDecreased?.Invoke();
            }
        }

        public void ResetHealth()
        {
            Health = 3;
            livesLeftText.text = Health.ToString();
        }
    }
}