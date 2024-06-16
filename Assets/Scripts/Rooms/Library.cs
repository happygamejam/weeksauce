using UnityEngine;

public class Library : Room
{
    public override Room Generate(DungeonParameters parameters)
    {
        Debug.Log("Generating a side room");
        var room = Instantiate(this, new Vector3(0, 0, 0), Quaternion.identity);
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
