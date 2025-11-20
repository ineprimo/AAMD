using UnityEngine;

public class StandarScaler
{
    private float[] mean;
    private float[] std;
    public StandarScaler(string serieliced)
    {
        string[] lines = serieliced.Split("\n");
        string[] meanStr = lines[0].Split(",");
        string[] stdStr = lines[1].Split(",");
        mean = new float[meanStr.Length];
        std = new float[stdStr.Length];
        for (int i = 0; i < meanStr.Length; i++)
        {
            mean[i] = float.Parse(meanStr[i], System.Globalization.CultureInfo.InvariantCulture);
        }

        for (int i = 0; i < stdStr.Length; i++)
        {
            std[i] = float.Parse(stdStr[i], System.Globalization.CultureInfo.InvariantCulture);
            std[i] = Mathf.Sqrt(std[i]);
        }
    }

    /// <summary>
    /// Aplica una normalización - media entre desviación tipica.
    /// </summary>
    /// <param name="a_input"></param>
    /// <returns></returns>
    public float[] Transform(float[] a_input)
    {
        float[] scaled = new float[a_input.Length];
        //TODO implementar

        return a_input;
    }
}