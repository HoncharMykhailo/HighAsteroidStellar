using UnityEngine;

[CreateAssetMenu(fileName = "NewSystemData", menuName = "Space System")]
public class NewSystemData : ScriptableObject
{
    public string systemName;          // Name of the system
    public float emptyAsteroidChance;  // Chance for empty asteroids
    public int oreType1;               // First ore type
    public int oreType2;               // Second ore type
    public float oreType1Chance;       // Spawn chance for first ore
    public float oreType2Chance;       // Spawn chance for second ore
}
