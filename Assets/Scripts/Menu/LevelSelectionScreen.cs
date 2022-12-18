using m039.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class LevelSelectionScreen : MonoBehaviour
    {
        #region Inspector

        [SerializeField] RectTransform _LevelsGrid;

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

        // Start is called before the first frame update
        void Start()
        {
            ConstructPage(0);

            // Init prev and next buttons.

            _PreviousButton.button.onClick.AddListener(OnPreviusButtonClicked);
            _NextButton.button.onClick.AddListener(OnNextButtonClicked);
        }

        void ConstructPage(int page)
        {
            _page = page;

            // Init levels grid.
            _LevelsGrid.transform.DestroyAllChildren();

            var startLevel = _page * CellsCount;
            var count = Mathf.Min(startLevel + CellsCount, LevelSelectionManager.Instance.MaxLevels);

            for (int i = startLevel; i < count; i++)
            {
                LevelButton.Create(_LevelButtonPrefab, _LevelsGrid, i + 1);
            }

            // Init page indicator group.
            _PageIndicatorGroup.transform.DestroyAllChildren();

            count = GetMaxPages();
            for (int i = 0; i < count; i++)
            {
                var pageIndicator = PageIndicator.Create(_PageIndicatorPrefab, _PageIndicatorGroup);
                pageIndicator.SetFilled(page == i);
            }

            // Update next and previous buttons.
            _PreviousButton.SetIconVisibility(page > 0);
            _NextButton.SetIconVisibility(page < GetMaxPages() - 1);
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

        void OnNextButtonClicked()
        {
            if (_page < GetMaxPages() - 1)
            {
                ConstructPage(_page + 1);
            }
        }

        void OnPreviusButtonClicked()
        {
            if (_page > 0)
            {
                ConstructPage(_page - 1);
            }
        }
    }
}
