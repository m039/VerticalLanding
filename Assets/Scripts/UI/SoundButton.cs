using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class SoundButton : MonoBehaviour
    {
        Transform _on;

        Transform _off;

        void Awake()
        {
            _on = transform.Find("Icon/On");
            _off = transform.Find("Icon/Off");
        }

        void OnEnable()
        {
            SetEnabled(true);
        }

        void OnDisable()
        {
            SetEnabled(false);
        }

        void SetEnabled(bool enabled)
        {
            _on.gameObject.SetActive(enabled);
            _off.gameObject.SetActive(!enabled);
        }
    }
}
