using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace VFX_Variant
{
    [ExecuteAlways]
    public class TargetPosition5Points : MonoBehaviour
    {
        public Transform target;

        private int[] shaderIDs;
        private Queue<(float time, Vector3 position)> history = new Queue<(float, Vector3)>();

        private const int NumPoints = 5;
        private const float MaxHistory = 0.5f; // garde une marge
        private readonly float[] timeOffsets = new float[] { 0f, 0.1f, 0.2f, 0.3f, 0.4f };

        void Start()
        {
            shaderIDs = new int[NumPoints];
            for (int i = 0; i < NumPoints; i++)
            {
                shaderIDs[i] = Shader.PropertyToID($"_TargetTurbulencePose{i + 1}");
            }

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
                UpdateShader(Time.realtimeSinceStartup);
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

            history.Enqueue((currentTime, target.position));

            // Nettoyer l'historique trop vieux
            while (history.Count > 0 && currentTime - history.Peek().time > MaxHistory)
            {
                history.Dequeue();
            }

            Vector3[] poses = new Vector3[NumPoints];

            for (int i = 0; i < NumPoints; i++)
            {
                float targetTime = currentTime - timeOffsets[i];
                poses[i] = GetClosestPosition(targetTime);
                Shader.SetGlobalVector(shaderIDs[i], poses[i]);
            }
        }

        Vector3 GetClosestPosition(float targetTime)
        {
            Vector3 closestPos = target != null ? target.position : Vector3.zero;
            float closestDiff = float.MaxValue;

            foreach (var (time, pos) in history)
            {
                float diff = Mathf.Abs(time - targetTime);
                if (diff < closestDiff)
                {
                    closestDiff = diff;
                    closestPos = pos;
                }
            }

            return closestPos;
        }
    }
}
