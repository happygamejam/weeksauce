using UnityEngine;

public class Ballroom : Room
{
    public override Room Generate(DungeonParameters parameters)
    {
        Debug.Log("Generating a ballroom");
        var room = Instantiate(this, new Vector3(0, 0, 0), Quaternion.identity);
        var provider = room.GetComponent<DungeonParametersProvider>();
        if (provider == null)
        {
            Debug.LogError("Failed to set parameters on Ballroom. Room prefabs must have a DungeonParametersProvider component.");
            return room;
        }
        provider.SetParameters(parameters);

        return room;
    }
}
