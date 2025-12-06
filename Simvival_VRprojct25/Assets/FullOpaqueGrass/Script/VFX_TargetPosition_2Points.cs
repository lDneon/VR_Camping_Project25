using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace VFX
{
    [ExecuteAlways]
    public class TargetPosition : MonoBehaviour
    {
        public Transform target;

        [Tooltip("Durée du trail (en secondes) pour la position passée")]
        public float trailDuration = 0.2f;

        private int shaderPropertyID_Current;
        private int shaderPropertyID_Past;

        private Queue<(float time, Vector3 position)> positionHistory = new Queue<(float, Vector3)>();

        void Start()
        {
            shaderPropertyID_Current = Shader.PropertyToID("_TargetTurbulencePose1");
            shaderPropertyID_Past = Shader.PropertyToID("_TargetTurbulencePose2");

#if UNITY_EDITOR
            EditorApplication.update += UpdateInEditor;
#endif
        }

        void OnDisable()
        {
#if UNITY_EDITOR
            EditorApplication.update -= UpdateInEditor;
#endif
        }

        void FixedUpdate()
        {
            if (Application.isPlaying)
            {
                UpdateShader((float)EditorApplication.timeSinceStartup);
            }
        }

#if UNITY_EDITOR
        void UpdateInEditor()
        {
            if (!Application.isPlaying)
            {
                UpdateShader((float)EditorApplication.timeSinceStartup);
            }
        }
#endif

        void UpdateShader(float currentTime)
        {
            if (target == null) return;

            Vector3 currentPosition = target.position;
            positionHistory.Enqueue((currentTime, currentPosition));

            Vector3 pastPosition = currentPosition;

            while (positionHistory.Count > 0)
            {
                var (time, position) = positionHistory.Peek();
                float age = currentTime - time;

                if (age > trailDuration)
                {
                    positionHistory.Dequeue();
                }
                else
                {
                    pastPosition = position;
                    break;
                }
            }

            Shader.SetGlobalVector(shaderPropertyID_Current, currentPosition);
            Shader.SetGlobalVector(shaderPropertyID_Past, pastPosition);
        }
    }
}
