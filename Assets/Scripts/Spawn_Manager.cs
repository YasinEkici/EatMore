using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Manager : MonoBehaviour
{
    // Prefab for the entry portal
    [SerializeField] private GameObject portalPrefab;

    // Prefab for the exit portal
    [SerializeField] private GameObject exitPortalPrefab;

    // Time interval between portal spawns
    [SerializeField] private float spawnPortalWaitTime = 5.0f;

    // Minimum distance required between entry and exit portals
    [SerializeField] private float minDistanceBetweenPortals = 10.0f;

    // Lifetime of portals before they are destroyed
    [SerializeField] private float portalLifetime = 10.20f;

    // Container object to organize spawned portals
    [SerializeField] GameObject portalContainer;

    // Container object to organize spawned bait
    [SerializeField] GameObject baitContainer;

    // Time interval between bait spawns
    [SerializeField] float spawnBaitWaitTime = 1.0f;

    // Array of different bait types
    public GameObject[] Baits;

    // Flag to control the spawning process
    bool stopSpawning = false;
    public AudioSource Spawn_Manager_Source;
    [SerializeField] private AudioClip baitSpawningclip;
    [SerializeField] private AudioClip portalSpawningclip;

    void Start()
    {
        // Start spawning bait and portals
        Spawn_Manager_Source = GetComponent<AudioSource>();
        if(Spawn_Manager_Source ==null){
            Debug.LogError("Audio Source boş");
        }
        StartCoroutine(SpawnBaits());
        StartCoroutine(SpawnPortals());
    }

    void Update()
    {
        // Currently no functionality needed in Update
    }
        private void OnEnable()
    {
        Spawn_Manager_Source.volume = PlayerPrefs.GetFloat("soundVolume", 0.5f);
        // EVENT'E ABONE OLUYORUZ
        SoundManager.OnSoundVolumeChanged += HandleOnSoundVolumeChanged;
    }

    private void OnDisable()
    {
        // EVENT'DEN ÇIKIYORUZ (Memory leak veya null ref engellemek için)
        SoundManager.OnSoundVolumeChanged -= HandleOnSoundVolumeChanged;
    }
        private void HandleOnSoundVolumeChanged(float newVolume)
    {
        // SoundManager'dan gelen yeni volume değeri, bu nesnenin AudioSource'u güncellesin
        Spawn_Manager_Source.volume = newVolume;
    }
    // Coroutine to spawn bait at regular intervals
    IEnumerator SpawnBaits()
    {
        while (!stopSpawning)
        {
            // Generate a random position within the defined bounds
            Vector3 spawnPosition = new Vector3(Random.Range(-34.4f, 34.4f), 1, Random.Range(-19.3f, 19.3f));

            // Check if the position is valid
            bool isPositionValid = IsBaitSpawnPositionValid(spawnPosition, 2.0f);

            // Randomly select a bait type
            int idMaker = Random.Range(0, 3);

            // If position is valid, spawn the bait
            if (isPositionValid)
            {
                GameObject newBait = Instantiate(Baits[idMaker], spawnPosition, Quaternion.identity);
                if (idMaker == 0){
                Spawn_Manager_Source.clip = baitSpawningclip;
                Spawn_Manager_Source.Play();
                }
                newBait.transform.parent = baitContainer.transform; // Parent the bait to the bait container
                yield return new WaitForSeconds(spawnBaitWaitTime); // Wait for the defined time before spawning the next bait
            }
        }
    }

    // Coroutine to spawn portals at regular intervals
    IEnumerator SpawnPortals()
    {
        while (!stopSpawning)
        {
            // Generate a random position for the entry portal
            Vector3 entrySpawnPosition = new Vector3(Random.Range(-34.4f, 34.4f), 0.1f, Random.Range(-19.3f, 19.3f));

            // Generate a valid position for the exit portal
            Vector3 exitSpawnPosition;
            do
            {
                exitSpawnPosition = new Vector3(Random.Range(-34.4f, 34.4f), 0.1f, Random.Range(-19.3f, 19.3f));
            } while (Vector3.Distance(entrySpawnPosition, exitSpawnPosition) < minDistanceBetweenPortals); // Ensure minimum distance

            // Spawn entry and exit portals
            GameObject entryPortal = Instantiate(portalPrefab, entrySpawnPosition, Quaternion.identity);
            Spawn_Manager_Source.clip = portalSpawningclip;
            Spawn_Manager_Source.Play();
            entryPortal.transform.parent = portalContainer.transform; // Parent the portal to the portal container
            GameObject exitPortal = Instantiate(exitPortalPrefab, exitSpawnPosition, Quaternion.identity);
            exitPortal.transform.parent = portalContainer.transform;

            // Link the entry portal to the exit portal
            Portal portalScript = entryPortal.GetComponent<Portal>();
            if (portalScript != null)
            {
                portalScript.SetExitPoint(exitPortal.transform);
            }

            // Destroy portals after their lifetime ends
            Destroy(entryPortal, portalLifetime);
            Destroy(exitPortal, portalLifetime);

            yield return new WaitForSeconds(spawnPortalWaitTime); // Wait for the defined time before spawning the next portals
        }
    }

    // Helper function to check if the spawn position for bait is valid
    private bool IsBaitSpawnPositionValid(Vector3 position, float minDistance)
    {
        // Check all existing bait positions in the bait container
        foreach (Transform bait in baitContainer.transform)
        {
            if (Vector3.Distance(position, bait.position) < minDistance)
            {
                return false; // Position is invalid if too close to an existing bait
            }
        }

        return true; // Position is valid
    }

    // Function to stop all spawning processes
    public void StopSpawning()
    {
        stopSpawning = true;
    }
}
