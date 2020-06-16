using UnityEngine;


namespace UnityStandardAssets.Effects
{
    public class ExtinguishableParticleSystem : MonoBehaviour
    {
        public float multiplier = 1;

        private ParticleSystem[] m_Systems;


        private void Start()
        {
            m_Systems = GetComponentsInChildren<ParticleSystem>();
        }


        public void Extinguish()
        {
            foreach (ParticleSystem system in m_Systems)
            {
                ParticleSystem.EmissionModule emission = system.emission;
                emission.enabled = false;
            }
        }
    }
}
