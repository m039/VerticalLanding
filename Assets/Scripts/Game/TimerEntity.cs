using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VL
{
    public class TimerEntity : MonoBehaviour
    {
        #region Inspector

        [SerializeField] int _Seconds = 60;

        #endregion

        public System.Action onTrigger;

        float _seconds;

        bool _isRunning;

        public void StartTimer()
        {
            _seconds = _Seconds;
            _isRunning = true;

            TimerIndicator.Instance?.Appear();
        }

        public void StopTimer()
        {
            _isRunning = false;
            TimerIndicator.Instance?.Disappear();
        }

        void TriggerTimer()
        {
            onTrigger?.Invoke();
            _isRunning = false;
        }

        void Update()
        {
            UpdateTimer();
        }

        void UpdateTimer()
        {
            if (!_isRunning)
                return;

            _seconds = Mathf.Max(_seconds - Time.deltaTime, 0f);
            TimerIndicator.Instance?.SetNumber((int)_seconds);

            if (_seconds <= 0f)
            {
                TriggerTimer();
            }
        }
    }
}
