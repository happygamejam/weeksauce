using System;
using System.Collections.Generic;
using UnityEngine;

public class PierreColoree : MonoBehaviour
{
    private TuileGenerator _master;
    private int _level = 0;

    private List<int> _validLevels = new List<int>();
    private int _checkpointLevel = -1;
    
    public void Setup(TuileGenerator newMaster, int newCheckpointLevel)
    {
        _master = newMaster;
        newMaster.UpdateRocheEvent += UpdateLevel;
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
        if (!_validLevels.Contains(_level))
        {
            Destroy(this);
            return;
            //TODO : KILL
        }

        if (_level == _checkpointLevel)
        {
            _master.UpdateRocheLevel(_checkpointLevel + 1);
        }
    }
}
