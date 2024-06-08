using UnityEngine;

public class SideRoom : Room
{
    public override GameObject Generate(DungeonParameters parameters)
    {
        Debug.Log("Generating a side room");
        var room = Instantiate(gameObject, new Vector3(0, 0, 0), Quaternion.identity);
        var provider = room.GetComponent<DungeonParametersProvider>();
        if (provider == null)
        {
            Debug.LogError("Failed to set parameters on SideRoom. Room prefabs must have a DungeonParametersProvider component.");
            return room;
        }
        provider.SetParameters(parameters);

        return room;
    }
}
