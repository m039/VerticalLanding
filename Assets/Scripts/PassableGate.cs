using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SF
{
    public class PassableGate : MonoBehaviour
    {
        #region Inspector

        public GateColor gateColor;

        #endregion

        SpriteRenderer _gate;

        SpriteRenderer Gate
        {
            get
            {
                if (_gate == null)
                {
                    _gate = transform.Find("Gate")?.GetComponent<SpriteRenderer>();
                }

                return _gate;
            }
        }

        void OnValidate()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
