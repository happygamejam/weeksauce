using UnityEngine;

public class EmptyRoom : Room
{
    public override GameObject Generate(DungeonParameters parameters)
    {
        Debug.Log("Generating an empty room");
        room = Instantiate(gameObject, new Vector3(0, 0, 0), Quaternion.identity);
        var provider = room.GetComponent<DungeonParametersProvider>();
        if (provider == null)
        {
            Debug.LogError("Failed to set parameters on EmptyRoom. Room prefabs must have a DungeonParametersProvider component.");
            return room;
        }
        provider.SetParameters(parameters);

        return room;
    }
}
