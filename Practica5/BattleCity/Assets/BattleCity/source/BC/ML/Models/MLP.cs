using System.Collections.Generic;
using UnityEngine;

public class MLPParameters
{
    List<float[,]> coeficients;
    List<float[]> intercepts;

    public MLPParameters(int numLayers)
    {
        coeficients = new List<float[,]>();
        intercepts = new List<float[]>();
        for (int i = 0; i < numLayers - 1; i++)
        {
            coeficients.Add(null);
        }
        for (int i = 0; i < numLayers - 1; i++)
        {
            intercepts.Add(null);
        }
    }

    public void CreateCoeficient(int i, int rows, int cols)
    {
        coeficients[i] = new float[rows, cols];
    }

    public void SetCoeficiente(int i, int row, int col, float v)
    {
        coeficients[i][row, col] = v;
    }

    public List<float[,]> GetCoeff()
    {
        return coeficients;
    }
    public void CreateIntercept(int i, int row)
    {
        intercepts[i] = new float[row];
    }

    public void SetIntercept(int i, int row, float v)
    {
        intercepts[i][row] = v;
    }
    public List<float[]> GetInter()
    {
        return intercepts;
    }
}

public class MLPModel
{
    MLPParameters mlpParameters;
    public MLPModel(MLPParameters p)
    {
        mlpParameters = p;
    }

    /// <summary>
    /// Parameters required for model input. By default it will be perception, kart position and time, 
    /// but depending on the data cleaning and data acquisition modificiations made by each one, the input will need more parameters.
    /// </summary>
    /// <param name="p">The Agent perception</param>
    /// <returns>The action label</returns>
    public float[] FeedForward(float[] input)
    {
        //TODO: implement feedworward.
        //the size of the output layer depends on what actions you have performed in the game.
        //By default it is 7 (number of possible actions) but some actions may not have been performed and therefore the model has assumed that they do not exist.
        return new float[5];
    }

    /// <summary>
    /// Calculo de la sigmoidal
    /// </summary>
    /// <param name="z"></param>
    /// <returns></returns>
    private float sigmoid(float z)
    {
        //TODO implementar
        return 0f;
    }


    /// <summary>
    /// CAlculo de la soft max, se le pasa el vector de la ulrima capa oculta y devuelve el mismo vector, pero procesado
    /// aplicando softmax a cada uno de los elementos
    /// </summary>
    /// <param name="zArr"></param>
    /// <returns></returns>
    public float[] SoftMax(float[] zArr)
    {
        //TODO implementar
        return zArr;
    }

    /// <summary>
    /// Elige el output de mayor nivel
    /// </summary>
    /// <param name="output"></param>
    /// <returns></returns>
    public int Predict(float[] output)
    {
        float max;
        int index = GetIndexMaxValue(output, out max);
        return index;
    }

    /// <summary>
    /// Obtiene el índice de mayor valor.
    /// </summary>
    /// <param name="output"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public int GetIndexMaxValue(float[] output, out float max)
    {
        max = output[0];
        int index = 0;
        //TODO impleemntar.
        return index;
    }
}
