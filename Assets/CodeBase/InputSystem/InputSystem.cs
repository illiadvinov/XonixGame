using System;
using System.Collections;
using CodeBase.Coroutine;
using UnityEngine;
using Zenject;

namespace CodeBase.InputSystem
{
    public class InputSystem
    {
        private readonly ICoroutineRunner coroutineRunner;
        public Vector2Int Direction { get; private set; }
        public event Action OnDirectionChanged;
        public event Action OnWrongInput;
        private float swipeRange = 50;
        private Vector2 startTouchPosition;
        private Vector2 endTouchPosition;
        private Vector2 currentTouchPosition;
        private bool stopTouch;
        private UnityEngine.Coroutine coroutine;

        [Inject]
        public InputSystem(ICoroutineRunner coroutineRunner)
        {
            this.coroutineRunner = coroutineRunner;
        }

        public void StartInput()
        {
            Direction = Vector2Int.left;
            coroutine = coroutineRunner.StartCoroutine(EndlessInput());
        }

        public void StopInput()
        {
            coroutineRunner.StopCoroutine(coroutine);
        }

        private IEnumerator EndlessInput()
        {
            while (true)
            {
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    startTouchPosition = Input.GetTouch(0).position;
                }

                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    currentTouchPosition = Input.GetTouch(0).position;
                    Vector2 Distance = currentTouchPosition - startTouchPosition;
                    if (!stopTouch)
                    {
                        if (IsLeftSwipe(Distance))
                        {
                            if (Direction == Vector2Int.left)
                            {
                                OnWrongInput?.Invoke();
                            }
                            else
                            {
                                Direction = Vector2Int.left;
                                OnDirectionChanged?.Invoke();
                            }

                            stopTouch = true;
                        }
                        else if (IsRightSwipe(Distance))
                        {
                            if (Direction == Vector2Int.right)
                            {
                                OnWrongInput?.Invoke();
                            }
                            else
                            {
                                Direction = Vector2Int.right;

                                OnDirectionChanged?.Invoke();
                            }

                            stopTouch = true;
                        }
                        else if (IsDownSwipe(Distance))
                        {
                            if (Direction == Vector2Int.down)
                            {
                                OnWrongInput?.Invoke();
                            }
                            else
                            {
                                Direction = Vector2Int.down;
                                stopTouch = true;
                                OnDirectionChanged?.Invoke();
                            }

                            stopTouch = true;
                        }
                        else if (IsUpSwipe(Distance))
                        {
                            if (Direction == Vector2Int.up)
                            {
                                OnWrongInput?.Invoke();
                            }
                            else
                            {
                                Direction = Vector2Int.up;
                                stopTouch = true;
                                OnDirectionChanged?.Invoke();
                            }

                            stopTouch = true;
                        }
                    }
                }

                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    stopTouch = false;
                    endTouchPosition = Input.GetTouch(0).position;
                }

                yield return null;
            }
        }

        private bool IsUpSwipe(Vector2 distance) =>
            distance.y > swipeRange;

        private bool IsDownSwipe(Vector2 distance) =>
            distance.y < -swipeRange;

        private bool IsRightSwipe(Vector2 distance) =>
            distance.x > swipeRange;

        private bool IsLeftSwipe(Vector2 distance) =>
            distance.x < -swipeRange;

        // Update is called once per frame
        void Update()
        {
        }
    }
}