using UnityEngine;
using UnityEngine.InputSystem;

namespace SF
{
    public class RocketFlame : MonoBehaviour
    {
        #region Inspector

        public float gap = 0.2f;

        public float outlineWidth = 0.05f;

        public float maxHeight = 1f;

        public Color flameColor = Color.red;

        #endregion

        Mesh _mesh;

        Vector3[] _vertices;

        Color[] _colors;

        MeshFilter _meshFilter;


        private void OnValidate()
        {
            UpdateVertices();
            UpdateColors();
        }

        private void Awake()
        {
            CreateMesh();
        }

        void UpdateColors()
        {
            if (!Application.isPlaying || _mesh == null)
                return;

            InitColors();
            _mesh.colors = _colors;
        }

        void UpdateVertices()
        {
            if (!Application.isPlaying || _mesh == null)
                return;

            InitVertices();
            _mesh.vertices = _vertices;
        }

        void InitVertices()
        {
            // Outline.

            var alpha = Mathf.Atan(gap / maxHeight);
            var cosAlpha = Mathf.Cos(alpha);
            var z = maxHeight * Mathf.Sqrt(1 - cosAlpha * cosAlpha);
            var a = outlineWidth / z * maxHeight;
            var b = outlineWidth / z * gap;

            if (maxHeight < outlineWidth)
            {
                a = b = 0;
            }

            _vertices[0] = new Vector3(gap, 0, 0);
            _vertices[1] = new Vector3(gap + b, 0, 0);
            _vertices[2] = new Vector3(0, -maxHeight - a, 0);
            _vertices[3] = new Vector3(0, -maxHeight, 0);
            _vertices[4] = new Vector3(-gap, 0, 0);
            _vertices[5] = new Vector3(-gap - b, 0, 0);

            // Flame.

            _vertices[6] = new Vector3(gap, 0, 0);
            _vertices[7] = new Vector3(0, -maxHeight, 0);
            _vertices[8] = new Vector3(-gap, 0, 0);
        }

        void InitColors()
        {
            _colors[0] = Color.black;
            _colors[1] = Color.black;
            _colors[2] = Color.black;
            _colors[3] = Color.black;
            _colors[4] = Color.black;
            _colors[5] = Color.black;
            _colors[6] = flameColor;
            _colors[7] = flameColor;
            _colors[8] = flameColor;
        }

        void CreateMesh()
        {
            _mesh = new Mesh();

            _vertices = new Vector3[9];
            InitVertices();

            _mesh.vertices = _vertices;

            _colors = new Color[9];
            InitColors();

            _mesh.colors = _colors;

            _mesh.normals = new Vector3[9]
            {
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
                -Vector3.forward,
            };

            _mesh.triangles = new int[15]
            {
                0, 1, 2,
                0, 2, 3,
                4, 3, 2,
                5, 4, 2,
                6, 7, 8
            };

            _meshFilter = GetComponent<MeshFilter>();
            _meshFilter.mesh = _mesh;
        }

        public void StartFlame()
        {

        }

        public void StopFlame()
        {

        }

        void Update()
        {
            if (Keyboard.current.wKey.isPressed)
            {
                StartFlame();
            } else
            {
                StopFlame();
            }
        }
    }
}
