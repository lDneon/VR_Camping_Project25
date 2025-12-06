using UnityEngine;
using MyGame.Wind;  // Assure-toi d'ajouter cette ligne pour que le script reconnaisse WindManager.

namespace MyGame.VFX
{
    [ExecuteAlways]
    public class VFXController : MonoBehaviour
    {
        [Header("Modifiable Parameters")]
        [SerializeField] private Color particleColor = Color.white;
        [SerializeField, Range(0f, 4f)] private float intensity = 1f;
        [SerializeField] private Vector3 windDirection = Vector3.zero;

        private ParticleSystem[] particleSystems;
        private float[] defaultRateOverTimeValues;

        private void Awake()
        {
            FindParticles();
            ApplySettings();
            WindManager.OnWindDirectionChanged += OnWindDirectionChanged;  // Maintenant reconnu grâce au 'using'
        }

        private void OnDestroy()
        {
            WindManager.OnWindDirectionChanged -= OnWindDirectionChanged;
        }

        private void OnValidate()
        {
            ApplySettings();
        }

        private void FindParticles()
        {
            particleSystems = GetComponentsInChildren<ParticleSystem>();
            if (particleSystems == null)
                return;
            defaultRateOverTimeValues = new float[particleSystems.Length];

            for (int i = 0; i < particleSystems.Length; i++)
            {
                var emission = particleSystems[i].emission;
                defaultRateOverTimeValues[i] = emission.rateOverTime.constant;
            }
        }

        private void ApplySettings()
        {
            if (particleSystems == null || particleSystems.Length == 0)
            {
                FindParticles();
            }

            for (int i = 0; i < particleSystems.Length; i++)
            {
                ParticleSystem ps = particleSystems[i];
                var main = ps.main;
                var emission = ps.emission;
                var velocityOverLifetime = ps.velocityOverLifetime;

                main.startColor = particleColor;

                float baseRate = defaultRateOverTimeValues[i];
                if (emission.rateOverTime.mode == ParticleSystemCurveMode.Constant)
                {
                    emission.rateOverTime = new ParticleSystem.MinMaxCurve(baseRate * intensity);
                }
                else
                {
                    emission.rateOverTime = new ParticleSystem.MinMaxCurve(baseRate * intensity, baseRate * intensity);
                }

                if (!velocityOverLifetime.enabled)
                {
                    velocityOverLifetime.enabled = true;
                }

                velocityOverLifetime.xMultiplier = windDirection.x;
                velocityOverLifetime.zMultiplier = windDirection.z;
            }
        }

        private void OnWindDirectionChanged(Vector2 newWindDirection)
        {
            Debug.Log("VFXController received new wind direction: " + newWindDirection);
            windDirection.x = newWindDirection.x;
            windDirection.z = newWindDirection.y;
            ApplySettings();
        }

        public void SetParticleColor(Color newColor)
        {
            particleColor = newColor;
            ApplySettings();
        }

        public void SetIntensity(float newIntensity)
        {
            intensity = Mathf.Clamp(newIntensity, 0f, 4f);
            ApplySettings();
        }

        public void SetWindDirection(Vector3 newWindDirection)
        {
            windDirection = newWindDirection;
            ApplySettings();
        }

        public Color GetParticleColor()
        {
            return particleColor;
        }

        public float GetIntensity()
        {
            return intensity;
        }

        public Vector3 GetWindDirection()
        {
            return windDirection;
        }
    }
}
