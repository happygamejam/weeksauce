using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rooms.RochesColorees;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TuileGenerator : MonoBehaviour
{
    [SerializeField] private int tailleX = 8;
    [SerializeField] private int tailleY = 8;
    [SerializeField] private int minSteps = 4;
    [SerializeField] private int maxSteps= 7;
    private char[,] _tuiles;

    [SerializeField] private GameObject _tuileParent;
    [SerializeField] private GameObject _tuilePrefab;
    
    int[] currentPosition = { 0, 0 };
    [SerializeField] private Material[] symboles;

    private int _previousIndex;
    private int _removedIndex = -1;

    private Step[] _case1 =
    {
        new Step(0,'\u2193',0, new []{1, 0}, 1),
        new Step(0,'\u2192',0,new []{0, 1}, 2),
        new Step(0,'\u2191',0, new []{-1, 0}, 3),
        new Step(0,'\u2190',0,new []{0, -1}, 2)
    };
    
    private Step[] _case2 = 
    {
        new Step(1,'\u2192',1,new []{0, 1}, 3),
        new Step(1,'\u2191',1,new []{-1, 0}, 2),
        new Step(1,'\u2190',1,new []{0, -1}, 3),
        new Step(1,'\u2192',1,new []{0, 1}, 1)
    };
    
    private Step[] _case3 = 
    {
        new Step(2, '\u2191',2,new []{-1, 0}, 3),
        new Step(2, '\u2191',2,new []{-1, 0}, 2),
        new Step(2, '\u2192',2,new []{0, 1}, 3),
        new Step(2, '\u2190',2,new []{0, -1}, 2)
    };
    
    private Step[] _case4 = 
    {
        new Step(3,'\u2192',3,new []{0, 1}, 1),
        new Step(3,'\u2190',3,new []{0, -1}, 1),
        new Step(3,'\u2193',3,new []{1, 0}, 1),
        new Step(3,'\u2191',3,new []{-1, 0}, 1),
    };

    // private Step[] _case1 =
    // {
    //     new Step('1',0, new []{1, 0}, 1),
    //     new Step('1',0,new []{0, 1}, 2),
    //     new Step('1',0, new []{-1, 0}, 2),
    //     new Step('1',0,new []{0, -1}, 2)
    // };
    //
    // private Step[] _case2 = 
    // {
    //     new Step('2',1,new []{0, 1}, 2),
    //     new Step('2',1,new []{0, -1}, 3),
    //     new Step('2',1,new []{0, -1}, 4),
    //     new Step('2',1,new []{0, 1}, 1)
    // };
    //
    // private Step[] _case3 = 
    // {
    //     new Step('3',2,new []{-1, 0}, 1),
    //     new Step('3',2,new []{-1, 0}, 2),
    //     new Step('3',2,new []{0, 1}, 3),
    //     new Step('3',2,new []{0, -1}, 2)
    // };
    //
    // private Step[] _case4 = 
    // {
    //     new Step('4',3,new []{0, 1}, 1),
    //     new Step('4',3,new []{0, -1}, 1),
    //     new Step('4',3,new []{1, 0}, 1),
    //     new Step('4',3,new []{-1, 0}, 1),
    // };
    
    private Step[][] _regle = new Step[4][];
   
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _tuiles = new char[tailleX, tailleY];
        _regle[0] = _case1;
        _regle[1] = _case2;
        _regle[2] = _case3;
        _regle[3] = _case4;

        List<Step> stepsToGenerate = new List<Step>();
        int[] startingPosition = new []{0, 0};
        int maxRetry = 10;
        
        while (maxRetry > 0)
        {
            maxRetry--;
            stepsToGenerate = new List<Step>();
            //Set empty tiles
            for (int i = 0; i < _tuiles.GetLength(0); i++)
            {
                for(int j = 0; j < _tuiles.GetLength(1); j++)
                {
                    _tuiles[i, j] = ' ';
                }
            }

        
            currentPosition[0] = _tuiles.GetLength(0) - 1;
            currentPosition[1] = Random.Range(0, _tuiles.GetLength(1));
            int[] startPosition = currentPosition; 
        

            //Déterminer ici premier icone précédent
            _previousIndex = Random.Range(0,4);
            bool found = true;
            int currentStep = 0;
            while (currentPosition[0] > 0)
            {
                currentStep++;
                if (currentStep > maxSteps || !CalculateNewchemin(stepsToGenerate))
                {
                    found = false;   
                    break;
                }
            }

            
            if (currentStep >= minSteps && found)
            {
                startingPosition = startPosition;
                _tuiles[startPosition[0], startPosition[1]] = 's';
                _tuiles[currentPosition[0], currentPosition[1]] = 'F';
                Print2DArray(_tuiles);
                break;
            }
        }

        if (maxRetry > 0)
        {
            InstanciateNewChemin(startingPosition, stepsToGenerate);
        }
    }

    private Step? EvaluateCheminNoIntersect()
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
            int currentCase = options[Random.Range(0, options.Count)];
            Step currentStep = _regle[currentCase][_previousIndex];

            int[] futurePosition =
            {
                currentPosition[0],
                currentPosition[1]
            };

            try
            {
                bool trouve = true;
                for (int i = 0; i < currentStep.NbSteps; i++)
                {
                    futurePosition = new[]
                    {
                        futurePosition[0] + currentStep.StepDirection[0], futurePosition[1] + currentStep.StepDirection[1]
                    };
                    
                    if (futurePosition[0] >= 0 && _tuiles[futurePosition[0], futurePosition[1]] != ' ')
                                               // && tuiles[futurePosition[0], futurePosition[1]] != '-'
                                               // && tuiles[futurePosition[0], futurePosition[1]] != '|')
                    {
                        trouve = false;
                        break;
                    }
                }

                if (trouve)
                {
                    return currentStep;
                }
            }
            catch (Exception e)
            {
                print("oops");
            }
            options.Remove(currentCase);
        }
    }
    
    private Step? EvaluateChemin()
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
            
            int currentCase = options[Random.Range(0, options.Count)];
            Step currentStep = _regle[currentCase][_previousIndex];
            int[] futurePosition =
            {
                currentPosition[0] + currentStep.StepDirection[0] * currentStep.NbSteps,
                currentPosition[1] + currentStep.StepDirection[1] * currentStep.NbSteps
            };


            try
            {
                if (futurePosition[0] < 0 || _tuiles[futurePosition[0], futurePosition[1]] == ' ')
                                          // || tuiles[futurePosition[0], futurePosition[1]] == '-'
                                          // || tuiles[futurePosition[0], futurePosition[1]] == '|')
                {
                    return currentStep;
                }
            }
            catch (Exception e)
            {
                print("oops");
            }
            options.Remove(currentCase);
        }
    }
    
    private bool CalculateNewchemin(List<Step> stepsToGenerate)
    {
        Step? checkStep = EvaluateChemin();

        if (checkStep == null)
        {
            return false;
        }

        Step stepTrouve = (Step)checkStep;
        stepsToGenerate.Add(stepTrouve);
        
        if (_previousIndex == stepTrouve.Case)
        {
            _removedIndex = stepTrouve.Case;
        }
        
        _previousIndex = stepTrouve.Case;
        
        _tuiles[currentPosition[0], currentPosition[1]] = stepTrouve.Symbole;
        for (int i = 0; i < stepTrouve.NbSteps; i++)
        {
            currentPosition = new[]
                { currentPosition[0] + stepTrouve.StepDirection[0], currentPosition[1] + stepTrouve.StepDirection[1] };
            if (currentPosition[0] == 0)
            {
                return true;
            }

            if (_tuiles[currentPosition[0], currentPosition[1]] != ' ')
            {
                continue;
            }
            
            if (stepTrouve.StepDirection[0] == 0)
            {
                _tuiles[currentPosition[0], currentPosition[1]] = '—';
            }
            else
            {
                _tuiles[currentPosition[0], currentPosition[1]] = '|';
            }
        }
        
        return true;
    }

    private void InstanciateNewChemin(int[] initialPosition, List<Step> steps)
    {
        GameObject[,] tuilesInstance = new GameObject[tailleX, tailleY];

        int[] currentPosition = new[] { initialPosition[0], initialPosition[1] };

        for (int i = 0; i < _tuiles.GetLength(0); i++)
        {
            for (int j = 0; j < _tuiles.GetLength(1); j++)
            {
                GameObject tuileInstance = Instantiate(_tuilePrefab, new Vector3(i * 2, 0, j * 2), Quaternion.identity);
                tuileInstance.transform.parent = _tuileParent.transform;
                tuileInstance.GetComponent<PierreColoree>().Setup(this, -1);
                tuilesInstance[i, j] = tuileInstance;

            }
        }

        tuilesInstance[currentPosition[0], currentPosition[1]].GetComponent<PierreColoree>().AddValidLevel(0);

        int currentLevel = 0;
        foreach (Step currentStep in steps)
        {
            tuilesInstance[currentPosition[0], currentPosition[1]].GetComponent<PierreColoree>().Setup(this, currentLevel);
            tuilesInstance[currentPosition[0], currentPosition[1]].transform.GetChild(0).GetComponent<MeshRenderer>().SetMaterials(new List<Material>(){symboles[currentStep.Id]});
            for (int j = 0; j < currentStep.NbSteps; j++)
            {
                currentPosition[0] += currentStep.StepDirection[0];
                currentPosition[1] += currentStep.StepDirection[1];

                try
                {
                    tuilesInstance[currentPosition[0], currentPosition[1]].GetComponent<PierreColoree>().AddValidLevel(currentLevel);
                }
                catch (Exception e)
                {
                    break;
                }
            }

            
            currentLevel++;
            // tuileInstance.GetComponent<PierreColoree>().Setup();

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

    public delegate void UpdateRocheLevelDelegate(int newLevel);

    public event UpdateRocheLevelDelegate UpdateRocheEvent;
    public void UpdateRocheLevel(int newLevel)
    {
        UpdateRocheEvent(newLevel);
    }

}
