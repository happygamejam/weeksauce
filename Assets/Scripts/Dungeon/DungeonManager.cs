using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance { get; private set; }

    private DungeonParameters activeDungeon;
    public static DungeonParameters ActiveDungeon => instance.activeDungeon;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void SetActiveDungeon(DungeonParameters parameters) {
       instance.activeDungeon = parameters; 
    }
}
