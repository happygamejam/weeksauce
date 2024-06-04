using UnityEngine;

public class EmptyRoom : Room
{
    public EmptyRoom(): base(25) { }

    public override GameObject Generate()
    {
        Debug.Log("Generating an empty room");
        return Instantiate(gameObject, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
