using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class DungeonListController
{
    VisualTreeAsset dungeonEntryTemplate;

    // UI Elements
    private ListView dungeonListView;
    private Label dungeonName;
    private Label difficulty;
    private Label rooms;
    private Button playButton;

    private List<DungeonParameters> dungeonList;
    private DungeonParameters selected;

    public void Initialize(VisualElement root, VisualTreeAsset entryTemplate)
    {
        FetchDungeonList();

        dungeonEntryTemplate = entryTemplate;

        dungeonListView = root.Q<ListView>("dungeon-list");
        dungeonName = root.Q<Label>("dungeon-name");
        difficulty = root.Q<Label>("dungeon-difficulty");
        rooms = root.Q<Label>("dungeon-rooms");
        playButton = root.Q<Button>("play-button");
        playButton.clicked += OnPlayClicked;

        FillList();

        dungeonListView.selectionChanged += OnDungeonSelected;
    }

    private void FetchDungeonList()
    {
        dungeonList = new List<DungeonParameters>();
        dungeonList.AddRange(Resources.LoadAll<DungeonParameters>("Dungeons"));
        Debug.Log("Found " + dungeonList.Count + " dungeons");
    }

    private void FillList()
    {
        dungeonListView.makeItem = () =>
        {
            var entry = dungeonEntryTemplate.Instantiate();

            var entryLogic = new DungeonListEntryController();
            entry.userData = entryLogic;
            entryLogic.SetVisualElement(entry);

            return entry;
        };

        // Set up bind function for a specific list entry
        dungeonListView.bindItem = (item, index) =>
        {
            (item.userData as DungeonListEntryController)?.SetDungeonParameters(dungeonList[index]);
        };

        dungeonListView.fixedItemHeight = 45;

        dungeonListView.itemsSource = dungeonList;
    }

    private void OnDungeonSelected(IEnumerable<object> selected) {
        var dungeon = dungeonListView.selectedItem as DungeonParameters;

        if (dungeon == null) {
            dungeonName.text = "Select a dungeon";
            difficulty.text = "";
            rooms.text = "";
            return;
        }

        dungeonName.text = dungeon.dungeonName;
        /* difficulty.text = "Difficulty: " + dungeon.difficulty; */
        rooms.text = dungeon.roomCount.ToString();
    }

    private void OnPlayClicked() {
        if (selected == null) {
            Debug.LogError("No dungeon selected.");
        }

        DungeonManager.SetActiveDungeon(selected);
        SceneManager.LoadScene("Dungeon");
    }
}
