using UnityEngine;

// Provides dungeon parameters its children.
// This should be on the prefab root and will be set automatically by DungeonGenerator.
public class DungeonParametersProvider : MonoBehaviour
{
	[SerializeField]
	private DungeonParameters parameters;

	public DungeonParameters Parameters => parameters;

	public void SetParameters(DungeonParameters parameters)
	{
		this.parameters = parameters;
	}
}
