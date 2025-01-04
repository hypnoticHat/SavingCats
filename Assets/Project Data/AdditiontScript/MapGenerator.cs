using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int totalMaps = 10;
    [SerializeField] private Transform mapParent; 
    [SerializeField] private GameObject[] animalPrefab;
    [SerializeField] private GameObject[] buffPrefab;
    [SerializeField] private GameObject[] chasingObjectPrefab;
    [SerializeField] private GameObject[] obstaclePrefab;
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private Vector3 mapSize = new Vector3(90, 0, 90);
    [SerializeField] private int baseAnimalCount = 5;
    [SerializeField] private int baseBuffCount = 3;
    [SerializeField] private int baseObstacleCount = 2;
    [SerializeField] private GameObject roadPrefab;

    private void Start()
    {
        GenerateMaps();
    }

    private void GenerateMaps()
    {
        for (int i = 0; i < totalMaps; i++)
        {
            Vector3 mapPosition = new Vector3(
                (i % 3) * (mapSize.x + 20),
                0,
                (i / 3) * (mapSize.z + 20)
            );

            GameObject map = new GameObject($"Map_Level_{i + 1}");
            map.transform.SetParent(mapParent);
            map.transform.position = mapPosition;


            SpawnRoad(map.transform, mapPosition);
            SpawnFloor(map.transform, mapPosition);
            GenerateMapContent(map.transform, i + 1);
        }
    }

    private void SpawnRoad(Transform mapTransform, Vector3 mapPosition)
    {

        GameObject road = Instantiate(roadPrefab, mapPosition, Quaternion.identity, mapTransform);
        Debug.Log("spawmed!");
        road.transform.localScale = new Vector3(mapSize.x, 1, mapSize.z);
    }

    private void SpawnFloor(Transform mapTransform, Vector3 mapPosition)
    {

        GameObject floor = Instantiate(floorPrefab, mapPosition, Quaternion.identity, mapTransform);

        floor.transform.localScale = new Vector3(mapSize.x/10, 1, mapSize.z/10);
    }


    private void GenerateMapContent(Transform mapTransform, int level)
    {
        int animalCount = baseAnimalCount + level * 2;
        int buffCount = Mathf.Max(baseBuffCount - level, 1);
        int obstacleCount = baseObstacleCount + level;

        // T?ng s? ð?i tý?ng c?n spawn
        int totalSpawnableObjects = animalCount + buffCount + obstacleCount;

        // Chia map thành các vùng nh?
        float cellWidth = mapSize.x / Mathf.Sqrt(totalSpawnableObjects);
        float cellHeight = mapSize.z / Mathf.Sqrt(totalSpawnableObjects);

        //  cat
        for (int i = 0; i < animalCount; i++)
        {
            Vector3 spawnPosition = GetRandomPositionInGrid(mapTransform, cellWidth, cellHeight, i);
            GameObject animal = animalPrefab[Random.Range(0, animalPrefab.Length)];
            Instantiate(animal, spawnPosition, Quaternion.identity, mapTransform);
        }

        //Buffs
        for (int i = 0; i < buffCount; i++)
        {
            Vector3 spawnPosition = GetRandomPositionInGrid(mapTransform, cellWidth, cellHeight, i + animalCount);
            GameObject buff = buffPrefab[Random.Range(0, buffPrefab.Length)];
            Instantiate(buff, spawnPosition, Quaternion.identity, mapTransform);
        }

        // Obstacles
        for (int i = 0; i < obstacleCount; i++)
        {
            Vector3 spawnPosition = GetRandomPositionInGrid(mapTransform, cellWidth, cellHeight, i + animalCount + buffCount);
            GameObject obstacle = obstaclePrefab[Random.Range(0, obstaclePrefab.Length)];
            Instantiate(obstacle, spawnPosition, Quaternion.identity, mapTransform);
        }

        // ChasingObject
        Transform chasingSpawnPoint = mapTransform.Find("ChasingSpawnPoint");
        if (chasingSpawnPoint != null)
        {
            GameObject chasingObject = chasingObjectPrefab[0];
            Instantiate(chasingObject, chasingSpawnPoint.position, chasingSpawnPoint.rotation, mapTransform);
        }
    }
    private Vector3 GetRandomPositionInGrid(Transform mapTransform, float cellWidth, float cellHeight, int index)
    {
        int gridRow = index / (int)Mathf.Sqrt(totalMaps);
        int gridCol = index % (int)Mathf.Sqrt(totalMaps);

        float minX = gridCol * cellWidth;
        float maxX = minX + cellWidth;
        float minZ = gridRow * cellHeight;
        float maxZ = minZ + cellHeight;

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        return new Vector3(randomX, mapTransform.position.y, randomZ);
    }


    private void OnDrawGizmos()
    {
        if (totalMaps <= 0 || mapSize == Vector3.zero) return;

  
        for (int i = 0; i < totalMaps; i++)
        {

            Vector3 mapPosition = new Vector3(
                (i % 3) * (mapSize.x + 20),
                0,                          
                (i / 3) * (mapSize.z + 20) 
            );

            Gizmos.color = Color.green; 
            Gizmos.DrawWireCube(mapPosition, mapSize);

 
            UnityEditor.Handles.Label(mapPosition + Vector3.up * 2, $"Map {i + 1}");
        }
    }
}
