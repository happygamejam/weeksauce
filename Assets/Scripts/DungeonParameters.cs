using UnityEngine;

[CreateAssetMenu(fileName = "DungeonParameters", menuName = "Scriptable Objects/DungeonParameters")]
public class DungeonParameters : ScriptableObject
{
    public int seed;

    // Number of rooms to generate
    // Excluding the beginning and end rooms
    public int roomCount;

    public bool preventDuplicates;
}
