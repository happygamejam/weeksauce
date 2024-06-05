using UnityEngine;

public class TuileGenerator : MonoBehaviour
{

    char[,] tuiles = new char[12, 8];

    int[] currentPosition = { 0, 0 };
    int bias = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set empty tiles
        for (int i = 0; i < tuiles.GetLength(0); i++)
        {
            for(int j = 0; j < tuiles.GetLength(1); j++)
            {
                tuiles[i, j] = ' ';
            }
        }

        currentPosition[0] = 11;
        currentPosition[1] = Random.Range(0, tuiles.GetLength(1));
        tuiles[currentPosition[0], currentPosition[1]] = 's';

        while (currentPosition[0] > 0)
        {
            CalculateNewchemin();
        }
        tuiles[currentPosition[0], currentPosition[1]] = 'F';

        Print2DArray(tuiles);
        
    }


    private void CalculateNewchemin()
    {

        int deplacementY = Random.Range(-4 + bias, 4 + bias);
        bias -= Sign(deplacementY);
        while (deplacementY != 0)
        {
            currentPosition[1] += Sign(deplacementY);
            if (currentPosition[1] < 0)
            {
                currentPosition[1] = 0;
                break;
            }
            if (currentPosition[1] >= tuiles.GetLength(1)) {
                currentPosition[1] = tuiles.GetLength(1) - 1;
            }
            tuiles[currentPosition[0], currentPosition[1]] = '—';
            deplacementY -= Sign(deplacementY);
        }
        tuiles[currentPosition[0], currentPosition[1]] = '+';


        int deplacementX = Random.Range(1, 4);

        while (deplacementX > 0) {
            currentPosition[0] -= 1;
            if (currentPosition[0] < 0) {
                currentPosition[0] = 0;
                break;
            }
            tuiles[currentPosition[0], currentPosition[1]] = '|';
            deplacementX--;
        }
        tuiles[currentPosition[0], currentPosition[1]] = '+';
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

    public static int Sign(int i)
    {
        return (i < 0) ? -1 : 1;
    }

}
