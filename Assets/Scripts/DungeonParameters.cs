using UnityEngine;

[CreateAssetMenu(fileName = "DungeonParameters", menuName = "Scriptable Objects/DungeonParameters")]
public class DungeonParameters : ScriptableObject
{
    // Seed for the random number generator
    // If negative, a random seed will be used
    public int seed;

    // Number of rooms to generate
    // Excluding the beginning and end rooms
    public int roomCount;

    public bool preventDuplicates;
}
