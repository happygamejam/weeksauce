using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rooms.RochesColorees;
using UnityEngine;
using Random = UnityEngine.Random;

public class TuileGenerator : MonoBehaviour
{

    char[,] tuiles = new char[10, 8];

    int[] currentPosition = { 0, 0 };
    

    private int _previousIndex;
    private int _removedIndex = -1;
 
    private Step[] _case1 =
    {
        new Step('\u2193',0, new []{1, 0}, 1),
        new Step('\u2192',0,new []{0, 1}, 2),
        new Step('\u2191',0, new []{-1, 0}, 3),
        new Step('\u2190',0,new []{0, -1}, 2)
    };
    
    private Step[] _case2 = 
    {
        new Step('\u2192',1,new []{0, 1}, 3),
        new Step('\u2191',1,new []{-1, 0}, 2),
        new Step('\u2190',1,new []{0, -1}, 3),
        new Step('\u2192',1,new []{0, 1}, 1)
    };
    
    private Step[] _case3 = 
    {
        new Step('\u2191',2,new []{-1, 0}, 3),
        new Step('\u2191',2,new []{-1, 0}, 2),
        new Step('\u2192',2,new []{0, 1}, 3),
        new Step('\u2190',2,new []{0, -1}, 2)
    };
    
    private Step[] _case4 = 
    {
        new Step('\u2192',3,new []{0, 1}, 1),
        new Step('\u2190',3,new []{0, -1}, 1),
        new Step('\u2193',3,new []{1, 0}, 1),
        new Step('\u2191',3,new []{-1, 0}, 1),
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
        _regle[0] = _case1;
        _regle[1] = _case2;
        _regle[2] = _case3;
        _regle[3] = _case4;
       
        
        //Set empty tiles
        for (int i = 0; i < tuiles.GetLength(0); i++)
        {
            for(int j = 0; j < tuiles.GetLength(1); j++)
            {
                tuiles[i, j] = ' ';
            }
        }

        
        currentPosition[0] = tuiles.GetLength(0) - 1;
        currentPosition[1] = Random.Range(0, tuiles.GetLength(1));
        int[] startPosition = currentPosition; 
        

        //Déterminer ici premier icone précédent
        _previousIndex = Random.Range(0,4);
        
        while (currentPosition[0] > 0)
        {
            Print2DArray(tuiles);
            if (!CalculateNewchemin())
            {
               break;
            }
            
        }
        tuiles[startPosition[0], startPosition[1]] = 's';
        tuiles[currentPosition[0], currentPosition[1]] = 'F';
        Print2DArray(tuiles);
        
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
                    
                    if (futurePosition[0] >= 0 && tuiles[futurePosition[0], futurePosition[1]] != ' ')
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
                if (futurePosition[0] < 0 || tuiles[futurePosition[0], futurePosition[1]] == ' ')
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
    
    private bool CalculateNewchemin()
    {
        Step? checkStep = EvaluateChemin();

        if (checkStep == null)
        {
            return false;
        }

        Step stepTrouve = (Step)checkStep;

        if (_previousIndex == stepTrouve.Case)
        {
            _removedIndex = stepTrouve.Case;
        }
        
        _previousIndex = stepTrouve.Case;
        
        tuiles[currentPosition[0], currentPosition[1]] = stepTrouve.Symbole;
        for (int i = 0; i < stepTrouve.NbSteps; i++)
        {
            currentPosition = new[]
                { currentPosition[0] + stepTrouve.StepDirection[0], currentPosition[1] + stepTrouve.StepDirection[1] };
            if (currentPosition[0] == 0)
            {
                return true;
            }

            if (stepTrouve.StepDirection[0] == 0)
            {
                // tuiles[currentPosition[0], currentPosition[1]] = '—';
            }
            else
            {
                // tuiles[currentPosition[0], currentPosition[1]] = '|';
            }
        }
        
        
        // int deplacementY = Random.Range(-4 + bias, 4 + bias);
        // bias -= Sign(deplacementY);
        // while (deplacementY != 0)
        // {
        //     currentPosition[1] += Sign(deplacementY);
        //     if (currentPosition[1] < 0)
        //     {
        //         currentPosition[1] = 0;
        //         break;
        //     }
        //     if (currentPosition[1] >= tuiles.GetLength(1)) {
        //         currentPosition[1] = tuiles.GetLength(1) - 1;
        //     }
        //     tuiles[currentPosition[0], currentPosition[1]] = '�';
        //     deplacementY -= Sign(deplacementY);
        // }
        // tuiles[currentPosition[0], currentPosition[1]] = '+';
        //
        //
        // int deplacementX = Random.Range(1, 4);
        //
        // while (deplacementX > 0) {
        //     currentPosition[0] -= 1;
        //     if (currentPosition[0] < 0) {
        //         currentPosition[0] = 0;
        //         break;
        //     }
        //     tuiles[currentPosition[0], currentPosition[1]] = '|';
        //     deplacementX--;
        // }
        // tuiles[currentPosition[0], currentPosition[1]] = '+';
        return true;
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

   

}
