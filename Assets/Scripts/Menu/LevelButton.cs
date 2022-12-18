using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public System.Action<LevelButton> onButtonClicked;

        TMPro.TMP_Text _number;

        Transform _checkboxFilled;

        Transform _checkboxHollowed;

        Transform _levelGroup;

        Transform _lockGroup;

        Button _button;

        void Awake()
        {
            _number = transform.Find("LevelGroup/Number").GetComponent<TMPro.TMP_Text>();
            _checkboxFilled = transform.Find("LevelGroup/Checkbox/Filled");
            _checkboxHollowed = transform.Find("LevelGroup/Checkbox/Hollowed");
            _levelGroup = transform.Find("LevelGroup");
            _lockGroup = transform.Find("LockGroup");
            _button = GetComponent<Button>();

            _button.onClick.AddListener(OnButtonClicked);
        }

        void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        void Start()
        {
            _number.text = Level.ToString();
        }

        public void SetCheckbox(bool enabled)
        {
            _checkboxFilled.gameObject.SetActive(enabled);
            _checkboxHollowed.gameObject.SetActive(!enabled);
        }

        public void ShowLock(bool on)
        {
            _levelGroup.gameObject.SetActive(!on);
            _lockGroup.gameObject.SetActive(on);
        }

        void OnButtonClicked()
        {
            if (!_lockGroup.gameObject.activeSelf)
            {
                onButtonClicked?.Invoke(this);
            }
        }
    }
}
