using UnityEngine;

public class EnemyCarSpawnManager : MonoBehaviour
{
    ScoreManager scoreInstance;
    [SerializeField] Transform[] spawnPlaces;
    [SerializeField] Transform carSpawnParent;

    public enum GameLevel
    {
        Easy,
        Medium,
        DarkSouls,
    }

    public GameLevel gameLevel;

    [System.Serializable]
    public class EnemyCar
    {
        public GameObject[] level1;
        public GameObject[] level2;
        public GameObject[] level3;
    }
    [SerializeField] EnemyCar enemyCar;

    [SerializeField] int levelIndex = 1;
    [SerializeField] int carCount = 0;

    private void Start()
    {
        scoreInstance = ScoreManager.instance;
    }

    private void OnEnable()
    {
        AI_CarHealth.CarAdded += AddCar;
        AI_CarHealth.AIDestroy += SpawnCars;
    }

    private void OnDisable()
    {
        AI_CarHealth.CarAdded -= AddCar;
        AI_CarHealth.AIDestroy -= SpawnCars;
    }

    void SpawnCars(Transform t)
    {
        carCount--;
        if (scoreInstance.level < 1)
            gameLevel = GameLevel.Easy;

        else if (scoreInstance.level > 1)
            gameLevel = GameLevel.Medium;

        SpawnFunction(gameLevel);
    }

    #region Spawn Function
    void SpawnFunction (GameLevel _level)
    {
        Debug.Log($"Car spawned {gameLevel}");
        if (_level == GameLevel.Easy)
        {
            int spawnType = Random.Range(0, enemyCar.level1.Length);
            int spawnPlaceCount = Random.Range(0, spawnPlaces.Length);

            GameObject objectToSpawn = enemyCar.level1[spawnType];
            Instantiate(objectToSpawn, spawnPlaces[spawnPlaceCount].position, Quaternion.identity, carSpawnParent);
        }

        else if (_level == GameLevel.Medium)
        {
            int spawnType = Random.Range(0, enemyCar.level2.Length);
            int spawnPlaceCount = Random.Range(0, spawnPlaces.Length);

            GameObject objectToSpawn = enemyCar.level2[spawnType];
            Instantiate(objectToSpawn, spawnPlaces[spawnPlaceCount].position, Quaternion.identity, carSpawnParent);
        }

        else if (_level == GameLevel.DarkSouls)
        {
            int spawnType = Random.Range(0, enemyCar.level3.Length);
            int spawnPlaceCount = Random.Range(0, spawnPlaces.Length);

            GameObject objectToSpawn = enemyCar.level3[spawnType];
            Instantiate(objectToSpawn, spawnPlaces[spawnPlaceCount].position, Quaternion.identity, carSpawnParent);
        }
    }
    #endregion

    void AddCar() => carCount++;
}
