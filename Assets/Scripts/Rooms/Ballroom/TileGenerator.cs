using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rooms.RochesColorees;
using Rooms.SymbolsLabyrinth;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private int sizeX = 8;
    [SerializeField] private int sizeY = 8;
    [SerializeField] private int minSteps = 5;
    [SerializeField] private int maxSteps= 8;
    private char[,] _tiles;

    [SerializeField] private GameObject _tileParent;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _theDuckingFloor;
    
    int[] _currentPosition = { 0, 0 };
    [SerializeField] private Material[] symbols;

    private int _previousIndex;
    private int _removedIndex = -1;

    [SerializeField] private PropSpawner _propSpawner;
    private int _propVariationValue = 0;
    
    private Step[] _tile1 =
    {
        new Step(0,'\u2193',0, new []{1, 0}, 1),
        new Step(0,'\u2192',0,new []{0, 1}, 2),
        new Step(0,'\u2191',0, new []{-1, 0}, 3),
        new Step(0,'\u2190',0,new []{0, -1}, 2),
        new Step(0,'\u2191',0,new []{-1, 0}, 2)
    };
    
    private Step[] _tile2 = 
    {
        new Step(1,'\u2192',1,new []{0, 1}, 3),
        new Step(1,'\u2191',1,new []{-1, 0}, 2),
        new Step(1,'\u2190',1,new []{0, -1}, 3),
        new Step(1,'\u2192',1,new []{0, 1}, 1),
        new Step(1,'\u2190',1,new []{0, -1}, 1)
    };
    
    private Step[] _tile3 = 
    {
        new Step(2, '\u2191',2,new []{-1, 0}, 4),
        new Step(2, '\u2193',2,new []{1, 0}, 1),
        new Step(2, '\u2192',2,new []{0, 1}, 3),
        new Step(2, '\u2190',2,new []{0, -1}, 2),
        new Step(2,'\u2192',2,new []{0, 1}, 1)
    };
    
    private Step[] _tile4 = 
    {
        new Step(3,'\u2192',3,new []{0, 1}, 1),
        new Step(3,'\u2190',3,new []{0, -1}, 1),
        new Step(3,'\u2193',3,new []{1, 0}, 1),
        new Step(3,'\u2191',3,new []{-1, 0}, 4),
        new Step(3,'\u2191',3,new []{-1, 0}, 4)
    };

    // private Step[] _tile1 =
    // {
    //     new Step('1',0, new []{1, 0}, 1),
    //     new Step('1',0,new []{0, 1}, 2),
    //     new Step('1',0, new []{-1, 0}, 2),
    //     new Step('1',0,new []{0, -1}, 2)
    // };
    //
    // private Step[] _tile2 = 
    // {
    //     new Step('2',1,new []{0, 1}, 2),
    //     new Step('2',1,new []{0, -1}, 3),
    //     new Step('2',1,new []{0, -1}, 4),
    //     new Step('2',1,new []{0, 1}, 1)
    // };
    //
    // private Step[] _tile3 = 
    // {
    //     new Step('3',2,new []{-1, 0}, 1),
    //     new Step('3',2,new []{-1, 0}, 2),
    //     new Step('3',2,new []{0, 1}, 3),
    //     new Step('3',2,new []{0, -1}, 2)
    // };
    //
    // private Step[] _tile4 = 
    // {
    //     new Step('4',3,new []{0, 1}, 1),
    //     new Step('4',3,new []{0, -1}, 1),
    //     new Step('4',3,new []{1, 0}, 1),
    //     new Step('4',3,new []{-1, 0}, 1),
    // };
    
    private Step[][] _rule = new Step[4][];

    [SerializeField] private PianoSpawner _pianoSpawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _propVariationValue = _propSpawner.GetPropVariationValue() % 3;
        _tiles = new char[sizeX, sizeY];
        _rule[0] = _tile1;
        _rule[1] = _tile2;
        _rule[2] = _tile3;
        _rule[3] = _tile4;

        List<Step> stepsToGenerate = new List<Step>();
        int[] startingPosition = new []{0, 0};
        int maxRetry = 30;
        
        while (maxRetry > 0)
        {
            maxRetry--;
            stepsToGenerate = new List<Step>();
            //Set empty tiles
            for (int i = 0; i < _tiles.GetLength(0); i++)
            {
                for(int j = 0; j < _tiles.GetLength(1); j++)
                {
                    _tiles[i, j] = '*';
                }
            }

        
            _currentPosition[0] = _tiles.GetLength(0) - 1;
            _currentPosition[1] = Random.Range(0, _tiles.GetLength(1));
            int[] startPosition = _currentPosition; 
        

            //Déterminer ici premier icone précédent
            _previousIndex = 4;
            bool found = true;
            int currentStep = 0;
            while (_currentPosition[0] > 0)
            {
                currentStep++;
                if (currentStep > maxSteps || !CalculateNewPath(stepsToGenerate))
                {
                    found = false;   
                    break;
                }
            }

            
            if (currentStep >= minSteps && found)
            {
                startingPosition = startPosition;
                _pianoSpawner.SpawnLibrary(startingPosition[1]);
                _tiles[startPosition[0], startPosition[1]] = 's';
                _tiles[_currentPosition[0], _currentPosition[1]] = 'F';
                Print2DArray(_tiles);
                break;
            }
        }

        if (maxRetry > 0)
        {
            InstanciateNewPath(startingPosition, stepsToGenerate);
        }
        else
        {
            Debug.Log("failed to instantiate maze");
        }
    }

    private Step? EvaluatePathNoIntersect()
    {
        List<int> options = new List<int> {0, 1, 2, 3};

        if (_removedIndex != -1)
        {
            options.Remove(_removedIndex);
            _removedIndex = -1;
        }
        
        while (true)
        {
            if (options.Count == 0)
            {
                return null;
            }
            int currentTile = options[Random.Range(0, options.Count)];
            Step currentStep = _rule[currentTile][_previousIndex];

            int[] futurePosition =
            {
                _currentPosition[0],
                _currentPosition[1]
            };

            try
            {
                bool found = true;
                for (int i = 0; i < currentStep.NbSteps; i++)
                {
                    futurePosition = new[]
                    {
                        futurePosition[0] + currentStep.StepDirection[0], futurePosition[1] + currentStep.StepDirection[1]
                    };
                    
                    if (futurePosition[0] >= 0 && _tiles[futurePosition[0], futurePosition[1]] != '*')
                                               // && tuiles[futurePosition[0], futurePosition[1]] != '-'
                                               // && tuiles[futurePosition[0], futurePosition[1]] != '|')
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return currentStep;
                }
            }
            catch (IndexOutOfRangeException) {}
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
            options.Remove(currentTile);
        }
    }
    
    private Step? EvaluatePath()
    {
        List<int> options = new List<int> {0, 1, 2, 3};
        if (_removedIndex != -1)
        {
            options.Remove(_removedIndex);
            _removedIndex = -1;
        }
        
        while (true)
        {
            if (options.Count == 0)
            {
                return null;
            }
            
            int currentTile = options[Random.Range(0, options.Count)];
            Step currentStep = _rule[currentTile][_previousIndex];
            int[] futurePosition =
            {
                _currentPosition[0] + currentStep.StepDirection[0] * currentStep.NbSteps,
                _currentPosition[1] + currentStep.StepDirection[1] * currentStep.NbSteps
            };


            try
            {
                if (futurePosition[0] < 0 || _tiles[futurePosition[0], futurePosition[1]] == '*')
                                          // || tuiles[futurePosition[0], futurePosition[1]] == '-'
                                          // || tuiles[futurePosition[0], futurePosition[1]] == '|')
                {
                    return currentStep;
                }
            }
            catch (IndexOutOfRangeException) {}
            catch (Exception e)
            {
                Debug.LogException(e, this);
            }
            options.Remove(currentTile);
        }
    }
    
    private bool CalculateNewPath(List<Step> stepsToGenerate)
    {
        Step? checkStep = EvaluatePath();

        if (checkStep == null)
        {
            return false;
        }

        Step stepFound = (Step)checkStep;
        stepsToGenerate.Add(stepFound);
        
        if (_previousIndex == stepFound.Tile)
        {
            _removedIndex = stepFound.Tile;
        }

        switch (_propVariationValue)
        {
            case 0 : 
                _previousIndex = stepFound.Tile;
                break;
            case 1 :
                switch (stepFound.StepDirection[0])
                {
                    //up
                    case -1 when stepFound.StepDirection[1] == 0:
                        _previousIndex = 0;
                        break;
                    //right
                    case 0 when stepFound.StepDirection[1] == 1:
                        _previousIndex = 1;
                        break;
                    //down
                    case 1 when stepFound.StepDirection[1] == 0:
                        _previousIndex = 2;
                        break;
                    //left
                    case 0 when stepFound.StepDirection[1] == -1:
                        _previousIndex = 3;
                        break;
                }
                break;
            case 2:
                _previousIndex = stepFound.NbSteps;
                break;
        }
       
        
        _tiles[_currentPosition[0], _currentPosition[1]] = stepFound.Symbol;
        for (int i = 0; i < stepFound.NbSteps; i++)
        {
            _currentPosition = new[]
                { _currentPosition[0] + stepFound.StepDirection[0], _currentPosition[1] + stepFound.StepDirection[1] };
            if (_currentPosition[0] == 0)
            {
                return true;
            }

            if (_tiles[_currentPosition[0], _currentPosition[1]] != '*')
            {
                continue;
            }
            
            if (stepFound.StepDirection[0] == 0)
            {
                _tiles[_currentPosition[0], _currentPosition[1]] = '—';
            }
            else
            {
                _tiles[_currentPosition[0], _currentPosition[1]] = '|';
            }
        }
        
        return true;
    }

    private void InstanciateNewPath(int[] initialPosition, List<Step> steps)
    {
        GameObject[,] tileInstances = new GameObject[sizeX, sizeY];

        int[] currentPosition = new[] { initialPosition[0], initialPosition[1] };
        bool rotated = false;
        for (int i = 0; i < _tiles.GetLength(0); i++)
        {
            for (int j = 0; j < _tiles.GetLength(1); j++)
            {
                GameObject tileInstance = Instantiate(_tilePrefab, _tileParent.transform);
                if(rotated)  tileInstance.transform.Rotate(Vector3.up, 90);
                rotated = !rotated;
                
                tileInstance.transform.localPosition = new Vector3(i * 4, 0, j * 4);
                tileInstance.GetComponent<BallroomTile>().Setup(this, _theDuckingFloor, -1);
                tileInstance.transform.GetChild(0).GetComponent<MeshRenderer>().SetMaterials(new List<Material>(){symbols[Random.Range(0, 4)]});
                tileInstances[i, j] = tileInstance;
    
            }

            rotated = !rotated;
        }

        tileInstances[currentPosition[0], currentPosition[1]].GetComponent<BallroomTile>().AddValidLevel(0);

        int currentLevel = 0;
        foreach (Step currentStep in steps)
        {
            tileInstances[currentPosition[0], currentPosition[1]].GetComponent<BallroomTile>().Setup(this, _theDuckingFloor, currentLevel);
            tileInstances[currentPosition[0], currentPosition[1]].transform.GetChild(0).GetComponent<MeshRenderer>().SetMaterials(new List<Material>(){symbols[currentStep.Id]});
            currentLevel++;
            for (int j = 0; j < currentStep.NbSteps; j++)
            {
                currentPosition[0] += currentStep.StepDirection[0];
                currentPosition[1] += currentStep.StepDirection[1];

                try
                {
                    tileInstances[currentPosition[0], currentPosition[1]].GetComponent<BallroomTile>().AddValidLevel(currentLevel);
                }
                catch (IndexOutOfRangeException) { }
                catch (Exception e)
                {
                    Debug.LogException(e, this);
                    break;
                }
            }
            
        }
    }

    
    private static void Print2DArray<T>(T[,] matrix)
    {
        string test = "";
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            test += "  ";
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                test += matrix[i, j] + "\t";
            }
            test += "\n";
        }
        Debug.Log(test);
    }

    public delegate void UpdateTileLevelDelegate(int newLevel);

    public event UpdateTileLevelDelegate UpdateTileEvent;
    public void UpdateTileLevel(int newLevel)
    {
        UpdateTileEvent(newLevel);
    }
}
