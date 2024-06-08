using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager instance { get; private set; }

    private DungeonParameters activeDungeon;
    private DungeonParameters previousDungeon;
    public static DungeonParameters ActiveDungeon => instance.activeDungeon;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void StartDungeon(DungeonParameters parameters) {
        if (parameters == null) {
            Debug.LogError("Cannot start dungeon with null parameters.");
        }
        
        instance.activeDungeon = parameters;
        SceneManager.LoadScene("Dungeon");
    }

    // Restarts the current dungeon, or the previous if there are no active dungeons.
    public static void Restart() {
        DungeonParameters dungeon = instance.activeDungeon;
        if (dungeon == null) {
            Debug.Log("No active dungeon, restarting previous dungeon: " + instance.previousDungeon);
            dungeon = instance.previousDungeon;
        }

        if (dungeon == null) {
            Debug.LogError("No dungeons to restart.");
            return;
        }

        StartDungeon(dungeon);
    }

    public static void Quit() {
        instance.previousDungeon = null;
        instance.activeDungeon = null;
        SceneManager.LoadScene("PlayMenu");
    }

    public static void GameOver() {
        instance.previousDungeon = instance.activeDungeon;
        instance.activeDungeon = null;
        Debug.Log("Game over, returning to menu.");
        SceneManager.LoadScene("GameOver");
    }
}
