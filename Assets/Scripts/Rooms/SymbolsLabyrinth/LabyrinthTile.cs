using System;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthTile : MonoBehaviour
{
    private TileGenerator _master;
    private int _level = 0;

    private List<int> _validLevels = new List<int>();
    private int _checkpointLevel = -1;
    
    public void Setup(TileGenerator newMaster, int newCheckpointLevel)
    {
        _master = newMaster;
        newMaster.UpdateTileEvent += UpdateLevel;
        _checkpointLevel = newCheckpointLevel;
    }

    public void AddValidLevel(int newValidLevel)
    {
        _validLevels.Add(newValidLevel);
    }

    public void UpdateLevel(int newLevel)
    {
        _level = newLevel;
    }
    
    

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(_level);
        foreach( var x in _validLevels) {
            Debug.Log( x.ToString());
        }

       
        if ( !_validLevels.Contains(_level))
        {
            Debug.Log("entered");
            Destroy(gameObject);
            return;
            //TODO : KILL
        }

        if (_level == _checkpointLevel)
        {
            _master.UpdateTileLevel(_checkpointLevel + 1);
        }
    }
}
