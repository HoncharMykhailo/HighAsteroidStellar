using System.Collections;
using UnityEngine;

public class AsteroidFieldGenerator : MonoBehaviour
{
    public GameObject[] asteroidPrefabs; // Array of asteroid meshes
    public Material[] asteroidMaterials; // Array of materials
    public Material goldMaterial;
    public Material gemMaterial;
    public Material hydroMaterial;
    public Material platinumMaterial;

    public int asteroidCount = 100; // Number of asteroids
    public float fieldRadius = 100f; // Field radius
    public float minCenterClearance = 20f; // Minimum distance from center
    public float minScale = 0.5f, maxScale = 3f; // Scale range
    public float baseAsteroidHealth = 50f;
    public float asteroidHealthMultiplier = 1f;
    public GameObject player;

    private int specialType1;
    private int specialType2;

    void Start()
    {
        // Randomly pick two special asteroid types (excluding 0)
        specialType1 = Random.Range(1, 5);
        do
        {
            specialType2 = Random.Range(1, 5);
        } while (specialType2 == specialType1);

        GenerateAsteroidField();
    }

    void GenerateAsteroidField()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            Vector3 position = GetRandomPosition();

            GameObject asteroidPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
            GameObject asteroid = Instantiate(asteroidPrefab, position, Random.rotation);
            float scale = Random.Range(minScale, maxScale);
            asteroid.transform.localScale = Vector3.one * scale;

            Rigidbody rb = asteroid.GetComponent<Rigidbody>() ?? asteroid.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.mass = scale;

            AsteroidCTRL asteroidCtrl = asteroid.GetComponent<AsteroidCTRL>() ?? asteroid.AddComponent<AsteroidCTRL>();
            asteroidCtrl.InitializeHealth(baseAsteroidHealth, scale * asteroidHealthMultiplier);

            // Assign asteroid type (mostly type 0, but some are special)
            int type = (Random.value < 0.75f) ? 0 : (Random.value < 0.5f ? specialType1 : specialType2);
            asteroidCtrl.type = type;
            asteroidCtrl.player = player;

            MeshRenderer renderer = asteroid.GetComponent<MeshRenderer>();
            if (renderer && asteroidMaterials.Length > 0)
            {
                renderer.material = GetMaterialForType(type);
            }

            asteroid.transform.parent = transform;
        }
    }

public void GenerateAsteroidFieldWithSystem(NewSystemData systemData)
{
    foreach (Transform child in transform)
    {
        Destroy(child.gameObject); // Clear previous asteroids
    }

    for (int i = 0; i < asteroidCount; i++)
    {
        Vector3 position = GetRandomPosition();
        GameObject asteroidPrefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
        GameObject asteroid = Instantiate(asteroidPrefab, position, Random.rotation);
        float scale = Random.Range(minScale, maxScale);
        asteroid.transform.localScale = Vector3.one * scale;

        AsteroidCTRL asteroidCtrl = asteroid.GetComponent<AsteroidCTRL>() ?? asteroid.AddComponent<AsteroidCTRL>();
        asteroidCtrl.InitializeHealth(baseAsteroidHealth, scale * asteroidHealthMultiplier);

        // Determine asteroid type based on system data
        float randValue = Random.value;
        if (randValue < systemData.emptyAsteroidChance)
        {
            asteroidCtrl.type = 0; // Empty asteroid
        }
        else if (randValue < systemData.emptyAsteroidChance + systemData.oreType1Chance)
        {
            asteroidCtrl.type = systemData.oreType1;
        }
        else
        {
            asteroidCtrl.type = systemData.oreType2;
        }

        MeshRenderer renderer = asteroid.GetComponent<MeshRenderer>();
        if (renderer && asteroidMaterials.Length > 0)
        {
            renderer.material = GetMaterialForType(asteroidCtrl.type);
        }

        asteroidCtrl.player = player;
        asteroid.transform.parent = transform;
    }
}


    Vector3 GetRandomPosition()
    {
        Vector3 pos;
        do
        {
            pos = new Vector3(
                Random.Range(-fieldRadius, fieldRadius),
                Random.Range(-fieldRadius, fieldRadius),
                Random.Range(-fieldRadius, fieldRadius)
            );
        } while (pos.magnitude < minCenterClearance);

        return pos + transform.position;
    }

    Material GetMaterialForType(int type)
    {
        switch (type)
        {
            case 0:
                return asteroidMaterials[Random.Range(0, asteroidMaterials.Length)];
            case 1:
                return goldMaterial;
            case 2:
                return gemMaterial;
            case 3:
                return hydroMaterial;
            case 4:
                return platinumMaterial;
            default:
                return asteroidMaterials[0];
        }
    }
}
