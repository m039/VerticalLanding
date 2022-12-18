using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class LevelButton : MonoBehaviour
    {
        public int Level { get; private set; }

        public static LevelButton Create(LevelButton prefab, Transform parent, int level)
        {
            var levelButton = Instantiate(prefab, parent);
            levelButton.Level = level;
            return levelButton;
        }

        TMPro.TMP_Text _number;

        Transform _checkboxFilled;

        Transform _checkboxHollowed;

        Transform _levelGroup;

        Transform _lockGroup;

        private void Awake()
        {
            _number = transform.Find("LevelGroup/Number").GetComponent<TMPro.TMP_Text>();
            _checkboxFilled = transform.Find("LevelGroup/Checkbox/Filled");
            _checkboxHollowed = transform.Find("LevelGroup/Checkbox/Hollowed");
            _levelGroup = transform.Find("LevelGroup");
            _lockGroup = transform.Find("LockGroup");
        }

        void Start()
        {
            _number.text = Level.ToString();

            SetCheckbox(LevelSelectionManager.Instance.IsLevelCompleted(Level));
            ShowLock(!LevelSelectionManager.Instance.IsLevelAvailable(Level));
        }

        void SetCheckbox(bool enabled)
        {
            _checkboxFilled.gameObject.SetActive(enabled);
            _checkboxHollowed.gameObject.SetActive(!enabled);
        }

        void ShowLock(bool on)
        {
            _levelGroup.gameObject.SetActive(!on);
            _lockGroup.gameObject.SetActive(on);
        }
    }
}
