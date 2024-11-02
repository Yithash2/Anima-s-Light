using System.Collections.Generic;
using UnityEngine;

public class ParticleFollower : MonoBehaviour
{
    public ParticleSystem _particleSystem;  // Reference to the particle system
    public GameObject followerPrefab;       // The prefab to follow each particle
    private ParticleSystem.Particle[] particles; // Array to hold the particles
    private List<GameObject> followers;           // List to hold each follower GameObject

    void Start()
    {
        // Initialize the followers list
        followers = new List<GameObject>();
        particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
    }

    void Update()
    {
        // Get the current active particles in the system
        int particleCount = _particleSystem.GetParticles(particles);

        // Adjust the followers list to match the particle count
        while (followers.Count < particleCount)
        {
            // Add more followers if there are more particles
            GameObject follower = Instantiate(followerPrefab, Vector3.zero, Quaternion.identity, transform);
            followers.Add(follower);
        }
        while (followers.Count > particleCount)
        {
            // Remove extra followers if there are fewer particles
            Destroy(followers[followers.Count - 1]);
            followers.RemoveAt(followers.Count - 1);
        }

        // Update each follower position to match each particle's position
        for (int i = 0; i < particleCount; i++)
        {
            Vector3 updatedPosition = (_particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World)
                ? particles[i].position
                : _particleSystem.transform.TransformPoint(particles[i].position);
            
            followers[i].transform.position = updatedPosition;
        }
    }
}

