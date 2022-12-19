using m039.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class MainMenuManager : MonoBehaviour
    {
        enum AppearSide
        {
            ToRight, ToLeft
        }

        #region Inspector

        [SerializeField] RectTransform _MainScreen;

        [SerializeField] RectTransform _LevelSelectionScreen;

        [SerializeField] float _AnimationDuration = 1f;

        #endregion

        RectTransform _currentScreen;

        RectTransform[] _screens;

        public static bool ShowLevelSelectionScreen;

        void Awake()
        {
            _screens = new []
            {
                _MainScreen,
                _LevelSelectionScreen
            };

            ShowScreen(DebugConfig.Instance.showLevelSelectionScreen || ShowLevelSelectionScreen ? _LevelSelectionScreen : _MainScreen, true);
        }

        public void OnPlayClicked()
        {
            ShowScreen(_LevelSelectionScreen, false, AppearSide.ToLeft);
        }

        public void OnHelpClicked()
        {

        }

        public void OnMenuClicked()
        {
            ShowScreen(_MainScreen, false, AppearSide.ToRight);
        }

        void ShowScreen(RectTransform screen, bool force, AppearSide sides = AppearSide.ToLeft)
        {
            if (force)
            {
                _screens.ForEach(s =>
                {
                    s.gameObject.SetActive(screen == s);
                    s.GetComponent<CanvasGroup>().alpha = 1f;
                });
                _currentScreen = screen;
            } else
            {
                IEnumerator animate(RectTransform screen, bool appearOrDisappear)
                {
                    screen.gameObject.SetActive(true);
                    var canvasGroup = screen.GetComponent<CanvasGroup>();
                    float startAlpha;
                    float endAlpha;
                    float startXPosition;
                    float endXPosition;
                    var size = screen.sizeDelta;
                    var d = 0f;

                    if (appearOrDisappear)
                    {
                        startAlpha = 0f;
                        endAlpha = 1f;

                        if (sides == AppearSide.ToRight)
                        {
                            startXPosition = -size.x;
                            endXPosition = 0f;
                        } else
                        {
                            startXPosition = size.x;
                            endXPosition = 0f;
                        }

                    } else
                    {
                        startAlpha = 1f;
                        endAlpha = 0f;

                        if (sides == AppearSide.ToRight)
                        {
                            startXPosition = 0f;
                            endXPosition = size.x;
                        }
                        else
                        {
                            startXPosition = 0f;
                            endXPosition = -size.x;
                        }
                    }

                    while (d < _AnimationDuration)
                    {
                        var t = d / _AnimationDuration;

                        canvasGroup.alpha = EasingFunction.EaseInOutCubic(startAlpha, endAlpha, t);

                        var p = screen.anchoredPosition;
                        p.x = EasingFunction.EaseInOutCubic(startXPosition, endXPosition, t);
                        screen.anchoredPosition = p;

                        d += Time.deltaTime;
                        yield return null;
                    }

                    screen.gameObject.SetActive(appearOrDisappear);
                }

                StopAllCoroutines();
                _screens.ForEach(s => StartCoroutine(animate(s, s == screen)));
            }
        }
    }
}
