using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsToSpawn;
    [SerializeField] private Transform[] spawnZones; 
    private void Start()
    {
        SpawnPrefabsInZones();
    }

    private void SpawnPrefabsInZones()
    {
        foreach (Transform spawnZone in spawnZones)
        {
            Vector3 randomPosition = GetRandomPositionInZone(spawnZone);
            GameObject randomPrefab = prefabsToSpawn[Random.Range(0, prefabsToSpawn.Length)];
            Instantiate(randomPrefab, randomPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPositionInZone(Transform zone)
    {
        Vector3 zoneScale = zone.localScale;
        float randomX = Random.Range(-zoneScale.x / 2, zoneScale.x / 2);
        float randomZ = Random.Range(-zoneScale.z / 2, zoneScale.z / 2);
        return zone.position + new Vector3(randomX, 0, randomZ);
    }
}
