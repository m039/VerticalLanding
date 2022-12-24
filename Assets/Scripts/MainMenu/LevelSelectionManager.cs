using m039.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VL
{
    public class LevelSelectionManager : SingletonScriptableObject<LevelSelectionManager>
    {
        const string LevelFmt = "Level_{0}";

        #region Inspector

        [SerializeField] int _MaxLevels = 3;

        #endregion

        #region SingletonScriptableObject

        protected override bool UseResourceFolder => true;

        protected override string PathToResource => "LevelSelectionManager";

        #endregion

        public int MaxLevels => _MaxLevels;

        public int FindAvailableLevel()
        {
            for (int i = MaxLevels; i >= 1; i--)
            {
                if (IsLevelAvailable(i))
                    return i;
            }

            return 1;
        }

        public bool IsLevelCompleted(int level)
        {
            return PlayerPrefs.GetInt(GetLevelSceneName(level) + "_completed", 0) == 1;
        }

        public void SetLevelCompleted(int level, bool upload = true)
        {
            PlayerPrefs.SetInt(GetLevelSceneName(level) + "_completed", 1);

            if (upload)
            {
                var completedLevels = new List<int>();
                for (int i = 1; i < MaxLevels; i++)
                {
                    if (IsLevelCompleted(i))
                    {
                        completedLevels.Add(i);
                    }
                }

                YandexGamesManager.Instance.UploadGameData(completedLevels.ToArray());
            }
        }

        public bool IsLevelAvailable(int level)
        {
            if (DebugConfig.Instance.allLevelsAvailable)
                return true;

            if (level == 1)
                return true;

            return IsLevelCompleted(level - 1);
        }

        public void OpenLevelScene(int level)
        {
            SceneManager.LoadScene(GetLevelSceneName(level));
        }

        public int GetCurrentLevel()
        {
            var scene = SceneManager.GetActiveScene();
            var undesrcore = scene.name.LastIndexOf('_');
            if (undesrcore == -1)
                return -1;

            var number = scene.name.Substring(undesrcore + 1);
            if (int.TryParse(number, out int result))
            {
                return result;
            }

            return -1;
        }

        string GetLevelSceneName(int level)
        {
            return string.Format(LevelFmt, level);
        }
    }
}
