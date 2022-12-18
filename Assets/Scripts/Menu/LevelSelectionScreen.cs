using m039.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class LevelSelectionScreen : MonoBehaviour
    {
        #region Inspector

        [SerializeField] float _TransitionDuration = 0.7f;

        [SerializeField] RectTransform _LevelsGridFirst;

        [SerializeField] RectTransform _LevelsGridSecond;

        [SerializeField] LevelButton _LevelButtonPrefab;

        [SerializeField] RectTransform _PageIndicatorGroup;

        [SerializeField] PageIndicator _PageIndicatorPrefab;

        [SerializeField] NextButton _NextButton;

        [SerializeField] NextButton _PreviousButton;

        #endregion

        const int Rows = 3;

        const int Columns = 3;

        const int CellsCount = Rows * Columns;

        int _page = 0;

        bool _isAnimating;

        RectTransform[] _levelGrids;

        void Awake()
        {
            _levelGrids = new[]
            {
                _LevelsGridFirst,
                _LevelsGridSecond
            };
        }

        void Start()
        {
            var availableLevel = LevelSelectionManager.Instance.FindAvailableLevel();
            MoveToPage((int)Mathf.Ceil((float)availableLevel / CellsCount) - 1, true);

            // Init prev and next buttons.
            _PreviousButton.button.onClick.AddListener(OnPreviusButtonClicked);
            _NextButton.button.onClick.AddListener(OnNextButtonClicked);
        }

        void MoveToPage(int page, bool force)
        {
            // Init page indicator group.
            _PageIndicatorGroup.transform.DestroyAllChildren();

            var count = GetMaxPages();
            for (int i = 0; i < count; i++)
            {
                var pageIndicator = PageIndicator.Create(_PageIndicatorPrefab, _PageIndicatorGroup);
                pageIndicator.SetFilled(page == i);
            }

            // Update next and previous buttons.
            _PreviousButton.SetIconVisibility(page > 0);
            _NextButton.SetIconVisibility(page < GetMaxPages() - 1);

            // Init level grids.
            if (force)
            {
                _levelGrids.ForEach(lg =>
                {
                    lg.gameObject.SetActive(lg == _LevelsGridFirst);
                    lg.GetComponent<CanvasGroup>().alpha = 1f;
                    lg.anchoredPosition = Vector2.zero;
                });
                ConstructPage(_LevelsGridFirst, page);
            } else
            {
                IEnumerator animate(RectTransform levelsGrid, int page, bool appearOrDisappear, bool leftOrRight)
                {
                    _isAnimating = true;

                    levelsGrid.gameObject.SetActive(true);
                    ConstructPage(levelsGrid, page);

                    var canvasGroup = levelsGrid.GetComponent<CanvasGroup>();
                    float startAlpha;
                    float endAlpha;
                    float startXPosition;
                    float endXPosition;
                    var width = levelsGrid.sizeDelta.x;

                    if (appearOrDisappear)
                    {
                        startAlpha = 0f;
                        endAlpha = 1f;

                        if (leftOrRight)
                        {
                            startXPosition = -width;
                            endXPosition = 0;
                        } else
                        {
                            startXPosition = width;
                            endXPosition = 0;
                        }
                    } else
                    {
                        startAlpha = 1f;
                        endAlpha = 0f;

                        if (leftOrRight)
                        {
                            startXPosition = 0;
                            endXPosition = width;
                        }
                        else
                        {
                            startXPosition = 0;
                            endXPosition = -width;
                        }
                    }

                    var d = 0f;

                    while (d < _TransitionDuration)
                    {
                        var t = d / _TransitionDuration;
                        canvasGroup.alpha = EasingFunction.EaseInOutCubic(startAlpha, endAlpha, t);
                        d += Time.deltaTime;

                        var p = levelsGrid.anchoredPosition;
                        p.x = EasingFunction.EaseInOutCubic(startXPosition, endXPosition, t);
                        levelsGrid.anchoredPosition = p;

                        yield return null;
                    }

                    _isAnimating = false;
                    levelsGrid.gameObject.SetActive(appearOrDisappear);
                }

                var leftOrRight = page < _page;

                StopAllCoroutines();
                StartCoroutine(animate(_LevelsGridFirst, _page, false, leftOrRight));
                StartCoroutine(animate(_LevelsGridSecond, page, true, leftOrRight));
            }

            _page = page;
        }

        void ConstructPage(RectTransform levelsGrid, int page)
        {
            // Init levels grid.
            levelsGrid.transform.DestroyAllChildren();

            var startLevel = page * CellsCount;
            var count = Mathf.Min(startLevel + CellsCount, LevelSelectionManager.Instance.MaxLevels);

            for (int i = startLevel; i < count; i++)
            {
                var level = i + 1;
                var levelButton = LevelButton.Create(_LevelButtonPrefab, levelsGrid, level);
                levelButton.SetCheckbox(LevelSelectionManager.Instance.IsLevelCompleted(level));
                levelButton.ShowLock(!LevelSelectionManager.Instance.IsLevelAvailable(level));
                levelButton.onButtonClicked += OnLevelButtonClicked;
            }
        }

        int GetMaxPages()
        {
            return (int) Mathf.Ceil((float)LevelSelectionManager.Instance.MaxLevels / CellsCount);
        }

        void OnDestroy()
        {
            _PreviousButton.button.onClick.RemoveListener(OnPreviusButtonClicked);
            _NextButton.button.onClick.RemoveListener(OnNextButtonClicked);
        }

        void OnLevelButtonClicked(LevelButton levelButton)
        {
            LevelSelectionManager.Instance.OpenScene(levelButton.Level);
        }

        void OnNextButtonClicked()
        {
            if (_isAnimating)
                return;

            if (_page < GetMaxPages() - 1)
            {
                MoveToPage(_page + 1, false);
            }
        }

        void OnPreviusButtonClicked()
        {
            if (_isAnimating)
                return;

            if (_page > 0)
            {
                MoveToPage(_page - 1, false);
            }
        }
    }
}
