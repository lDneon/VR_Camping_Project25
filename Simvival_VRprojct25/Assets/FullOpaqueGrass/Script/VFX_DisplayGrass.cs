using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GrassDisplay
{
    [ExecuteInEditMode]
    public class MeshSpawner : MonoBehaviour
    {
        [Header("Spawning Settings")]
        public GameObject prefab;
        public int density = 100;
        [Range(0f, 180f)]
        public float maxSlopeAngle = 45f;

        [Header("Scale Settings")]
        public float randomScaleMin = 0.8f;
        public float randomScaleMax = 1.2f;

        public void Generate()
        {
            // Supprime tous les enfants nommés "GeneratedInstances" avant de générer de nouveaux objets.
            List<Transform> toDelete = new List<Transform>();
            foreach (Transform child in transform)
            {
                if (child.gameObject.name == "GeneratedInstances")
                {
                    toDelete.Add(child);
                }
            }
            foreach (Transform child in toDelete)
            {
#if UNITY_EDITOR
                Undo.DestroyObjectImmediate(child.gameObject);
#else
                Destroy(child.gameObject);
#endif
            }

            if (prefab == null)
            {
                Debug.LogWarning("Prefab non assigné.");
                return;
            }

            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null)
            {
                Debug.LogWarning("Aucun MeshFilter valide trouvé sur cet objet.");
                return;
            }

            Mesh mesh = meshFilter.sharedMesh;

            // Création d'un nouveau container nommé "GeneratedInstances"
            GameObject generatedContainer = new GameObject("GeneratedInstances");
            generatedContainer.transform.SetParent(transform);
            generatedContainer.transform.localPosition = Vector3.zero;
            generatedContainer.transform.localRotation = Quaternion.identity;

            GenerateInstances(mesh, generatedContainer);
        }

        private void GenerateInstances(Mesh mesh, GameObject container)
        {
            int spawned = 0;
            int attempts = 0;
            int maxAttempts = density * 10; // marge pour respecter les contraintes d'angle

            while (spawned < density && attempts < maxAttempts)
            {
                attempts++;
                Vector3 localPoint, localNormal;
                RandomPointOnMesh(mesh, out localPoint, out localNormal);

                // Passage en espace monde
                Vector3 worldPos = transform.TransformPoint(localPoint);
                Vector3 worldNormal = transform.TransformDirection(localNormal);
                float angle = Vector3.Angle(worldNormal, Vector3.up);

                // Vérifie la contrainte d'angle si maxSlopeAngle est défini (< 180°)
                if (maxSlopeAngle < 180f && angle > maxSlopeAngle)
                    continue;

                // Calcule la rotation pour aligner le prefab avec la normale du mesh
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, worldNormal);
                // Applique une rotation aléatoire sur l'axe Y après alignement avec la normale
                rotation *= Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                float randomScale = Random.Range(randomScaleMin, randomScaleMax);
                float slopeFactor = (maxSlopeAngle < 180f) ? Mathf.Lerp(1f, 0.5f, angle / maxSlopeAngle) : 1f;
                float finalScale = randomScale * slopeFactor;

                // Instancie le prefab comme enfant du container
#if UNITY_EDITOR
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, container.transform);
                Undo.RegisterCreatedObjectUndo(instance, "Spawn Instance");
#else
                GameObject instance = Instantiate(prefab, container.transform);
#endif
                instance.transform.position = worldPos;
                instance.transform.rotation = rotation;
                instance.transform.localScale = Vector3.one * finalScale;

                spawned++;
            }

            if (spawned == 0)
            {
                Debug.LogWarning("Aucune instance n'a été générée : vérifie les contraintes d'angle et les normales du mesh.");
            }
        }

        /// <summary>
        /// Génère un point aléatoire sur la surface d'un mesh (pondéré par l'aire des triangles)
        /// et calcule sa normale interpolée via les coordonnées barycentriques.
        /// </summary>
        private void RandomPointOnMesh(Mesh mesh, out Vector3 outPoint, out Vector3 outNormal)
        {
            int[] triangles = mesh.triangles;
            Vector3[] vertices = mesh.vertices;
            Vector3[] normals = mesh.normals;
            int triangleCount = triangles.Length / 3;

            float[] areas = new float[triangleCount];
            float totalArea = 0f;
            for (int i = 0; i < triangleCount; i++)
            {
                int index0 = triangles[i * 3];
                int index1 = triangles[i * 3 + 1];
                int index2 = triangles[i * 3 + 2];
                Vector3 a = vertices[index0];
                Vector3 b = vertices[index1];
                Vector3 c = vertices[index2];
                float area = Vector3.Cross(b - a, c - a).magnitude * 0.5f;
                areas[i] = area;
                totalArea += area;
            }

            float randomArea = Random.Range(0f, totalArea);
            float cumulative = 0f;
            int selectedTriangle = 0;
            for (int i = 0; i < triangleCount; i++)
            {
                cumulative += areas[i];
                if (randomArea <= cumulative)
                {
                    selectedTriangle = i;
                    break;
                }
            }

            int i0 = triangles[selectedTriangle * 3];
            int i1 = triangles[selectedTriangle * 3 + 1];
            int i2 = triangles[selectedTriangle * 3 + 2];

            Vector3 v0 = vertices[i0];
            Vector3 v1 = vertices[i1];
            Vector3 v2 = vertices[i2];

            float r1 = Random.Range(0f, 1f);
            float r2 = Random.Range(0f, 1f);
            if (r1 + r2 > 1f)
            {
                r1 = 1f - r1;
                r2 = 1f - r2;
            }
            float r0 = 1f - r1 - r2;
            outPoint = v0 * r0 + v1 * r1 + v2 * r2;

            if (normals != null && normals.Length > 0)
            {
                Vector3 n0 = normals[i0];
                Vector3 n1 = normals[i1];
                Vector3 n2 = normals[i2];
                outNormal = (n0 * r0 + n1 * r1 + n2 * r2).normalized;
            }
            else
            {
                outNormal = Vector3.Cross(v1 - v0, v2 - v0).normalized;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MeshSpawner))]
    public class MeshSpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Affiche l'inspecteur par défaut et le bouton Generate
            DrawDefaultInspector();
            MeshSpawner spawner = (MeshSpawner)target;
            if (GUILayout.Button("Generate"))
            {
                spawner.Generate();
            }
        }
    }
#endif
}
