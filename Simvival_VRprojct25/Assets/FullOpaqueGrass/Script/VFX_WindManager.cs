using UnityEngine;
using System;

namespace MyGame.Wind
{
    [ExecuteAlways]
    public class WindManager : MonoBehaviour
    {
        [Header("Wind Settings")]
        public Vector2 windDirection = new Vector2(1, 0);
        public float windSize = 1.0f;
        public float windStrength = 1.0f;
        public float windSpeed = 1.0f;

        public static event Action<Vector2> OnWindDirectionChanged;

        private Vector2 lastWindDirection;
        private float lastWindSize;
        private float lastWindStrength;
        private float lastWindSpeed;

        void Update()
        {
            if (windDirection != lastWindDirection || windSize != lastWindSize || windStrength != lastWindStrength || windSpeed != lastWindSpeed)
            {
                Shader.SetGlobalVector("_WindDirection", new Vector4(windDirection.x, windDirection.y, 0, 0));
                Shader.SetGlobalFloat("_WindSize", windSize);
                Shader.SetGlobalFloat("_WindStrength", windStrength);
                Shader.SetGlobalFloat("_WindSpeed", windSpeed);

                if (windDirection != lastWindDirection)
                {
                    OnWindDirectionChanged?.Invoke(windDirection);
                }

                lastWindDirection = windDirection;
                lastWindSize = windSize;
                lastWindStrength = windStrength;
                lastWindSpeed = windSpeed;
            }
        }

        void OnValidate()
        {
            Shader.SetGlobalVector("_WindDirection", new Vector4(windDirection.x, windDirection.y, 0, 0));
            Shader.SetGlobalFloat("_WindSize", windSize);
            Shader.SetGlobalFloat("_WindStrength", windStrength);
            Shader.SetGlobalFloat("_WindSpeed", windSpeed);

            OnWindDirectionChanged?.Invoke(windDirection);
        }
    }
}
