using m039.Common;
using UnityEngine;

namespace VL
{
    public class GateColorData : SingletonScriptableObject<GateColorData>
    {
        public Color red = Color.white;

        public Color green = Color.white;

        public Color yellow = Color.white;

        #region SingletonScriptableObject

        protected override bool UseResourceFolder => true;

        protected override string PathToResource => "Data/GateColor";

        #endregion
    }
}
