using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SF
{
    public class LevelSelectionManager
    {
        static LevelSelectionManager _sInstance;

        public static LevelSelectionManager Instance {
            get {
                if (_sInstance == null)
                    _sInstance = new LevelSelectionManager();

                return _sInstance;
            }
        }

        public int MaxLevels = 22;

        public int FindAvailableLevel()
        {
            return 6;
        }

        public bool IsLevelCompleted(int level)
        {
            return level >= 1 && level <= 5;
        }

        public bool IsLevelAvailable(int level)
        {
            return level >= 1 && level <= 6;
        }

        public void OpenScene(int level)
        {
            SceneManager.LoadScene("Level_1");
        }
    }
}
