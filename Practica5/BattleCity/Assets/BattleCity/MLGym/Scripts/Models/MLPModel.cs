using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace MLGym
{
    public class MLPModel
    {
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


            public void CreateIntercept(int i, int row)
            {
                intercepts[i] = new float[row];
            }

            public void SetIntercept(int i, int row, float v)
            {
                intercepts[i][row] = v;
            }

            public List<float[,]> getCoeficients()
            {
                return coeficients;
            }

            public List<float[]> getIntercepts()
            {
                return intercepts;
            }
        }



        private List<float[,]> thetas;

        MLPParameters mlpParameters;
        public MLPModel(MLPParameters p)
        {
            mlpParameters = p;

            List<float[,]> coeficients = mlpParameters.getCoeficients();
            List<float[]> intecepts = mlpParameters.getIntercepts();

            thetas = new List<float[,]>();

            // Añado el bias 
            for (int idx = 0; idx < coeficients.Count; idx++)
            {
                float[,] coeficient = coeficients[idx];
                int rows = coeficient.GetLength(0) + 1;
                int cols = coeficient.GetLength(1);

                thetas.Add(new float[rows, cols]);
                float[,] theta = thetas[idx];

                for (int j = 0; j < cols; j++)
                    theta[0, j] = intecepts[idx][j];

                for (int i = 1; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                        theta[i, j] = coeficient[i - 1, j];
            }

            // Traspongo las thetas
            //for (int idx = 0; idx < thetas.Count; idx++)
            //{
            //    float[,] theta = thetas[idx];
            //    int rows = theta.GetLength(0);
            //    int cols = theta.GetLength(1);

            //    float[,] trasposedTheta = thetas[idx] = new float[cols, rows];

            //    for (int i = 0; i < cols; i++)
            //        for (int j = 0; j < rows; j++)
            //            trasposedTheta[i, j] = theta[j, i]; 
            //}
        }

        /// <summary>
        /// Parameters required for model input. By default it will be perception, kart position and time, 
        /// but depending on the data cleaning and data acquisition modificiations made by each one, the input will need more parameters.
        /// </summary>
        /// <param name="p">The Agent perception</param>
        /// <returns>The action label</returns>
        public float[] FeedForward(Perception p, Transform transform, Parameters parameters)
        {
            float[] input = parameters.ConvertToFloatArray();

            // Inicializa la primera activación a partir de X  
            float[] a = cleanData(input, p);

            // Propagación hacia adelante a través de todas las capas
            for (int i = 0; i < thetas.Count; i++)
            {
                // Agrega el sesgo a la activación actual 
                a = addBias(a);

                // Calcula z para la capa
                float[] z = dot(a, thetas[i]);

                // Calcula la activación para la siguiente capa usando la función sigmoidal
                a = sigmoid(z);
            }

            return a;
        }

        private float[] cleanData(float[] input, Perception perception)
        {
            float[] cleanedInput = new float[6];

            // rays
            for (int i = 1; i < perception.distance.Length; i++)
            {
                if (input[i] == -1)
                    cleanedInput[i - 1] = perception.distance[i];
                else cleanedInput[i - 1] = input[i];
            }

            // x
            cleanedInput[4] = input[5];

            // z
            cleanedInput[5] = input[7];

            return cleanedInput;
        }

        private float[] addBias(float[] vector)
        {
            float[] newArr = new float[vector.Length + 1];
            newArr[0] = 1.0f;
            Array.Copy(vector, 0, newArr, 1, vector.Length);
            return newArr;
        }

        private float[] dot(float[] vector, float[,] matrix)
        {
            int vectorSize = vector.Length;
            int matrixRows = matrix.GetLength(0);
            int matrixCols = matrix.GetLength(1);

            if (vectorSize != matrixRows)
                throw new InvalidOperationException("Las dimensiones no son compatibles para el producto escalar.");

            float[] result = new float[matrixCols];

            for (int i = 0; i < matrixCols; i++)
            {
                float sum = 0.0f;
                for (int j = 0; j < vectorSize; j++)
                {
                    sum += vector[j] * matrix[j, i]; // Multiplicación para el producto escalar
                }
                result[i] = sum;
            }

            return result;
        }

        static float[] sigmoid(float[] vector)
        {
            float[] result = new float[vector.Length];

            for (int i = 0; i < vector.Length; i++)
            {
                double sigmoid = 1.0 / (1.0 + Math.Exp(-vector[i]));
                result[i] = (float)sigmoid;
            }


            return result;
        }

        

        public int Predict(float[] output)
        {
            float max;
            int index = GetIndexMaxValue(output, out max);
            return index;
        }

        public int GetIndexMaxValue(float[] output, out float max)
        {
            max = output[0];
            max = output[0];
            int index = 0;
            for (int i = 1; i < output.Length; i++)
            {
                if (output[i] > max)
                {
                    max = output[i];
                    index = i;
                }
            }
            return index;
        }

        public static string TrimpBrackers(string val)
        {
            val = val.Trim();
            val = val.Substring(1);
            val = val.Substring(0, val.Length - 1);
            return val;
        }

        public static int[] SplitWithColumInt(string val)
        {
            val = val.Trim();
            string[] values = val.Split(",");
            int[] result = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
                if (values[i].StartsWith("'"))
                    values[i] = values[i].Substring(1);
                if (values[i].EndsWith("'"))
                    values[i] = values[i].Substring(0, values[i].Length - 1);
                result[i] = int.Parse(values[i]);
            }
            return result;
        }

        public static float[] SplitWithColumFloat(string val)
        {
            // Usar el formato 'en-US' (inglés de Estados Unidos) para el punto decimal
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

            val = val.Trim();
            string[] values = val.Split(", ");
            float[] result = new float[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                result[i] = float.Parse(values[i], culture);
            }
            return result;
        }

        public static MLPParameters LoadParameters(string file, string delimeter)
        {
            string[] lines = file.Split(delimeter);
            int num_layers = 0;
            MLPParameters mlpParameters = null;
            int currentParameter = -1;
            int[] currentDimension = null;
            bool coefficient = false;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                line = line.Trim();
                if (line != "")
                {
                    string[] nameValue = line.Split(":");
                    string name = nameValue[0];
                    string val = nameValue[1];
                    if (name == "num_layers")
                    {
                        num_layers = int.Parse(val);
                        mlpParameters = new MLPParameters(num_layers);
                    }
                    else
                    {
                        if (num_layers <= 0)
                            Debug.LogError("Format error: First line must be num_layers");
                        else
                        {
                            if (name == "parameter")
                                currentParameter = int.Parse(val);
                            else if (name == "dims")
                            {
                                val = TrimpBrackers(val);
                                currentDimension = SplitWithColumInt(val);
                            }
                            else if (name == "name")
                            {
                                if (val.StartsWith("coefficient"))
                                {
                                    coefficient = true;
                                    int index = currentParameter / 2;
                                    mlpParameters.CreateCoeficient(currentParameter, currentDimension[0], currentDimension[1]);
                                }
                                else
                                {
                                    coefficient = false;
                                    mlpParameters.CreateIntercept(currentParameter, currentDimension[1]);
                                }

                            }
                            else if (name == "values")
                            {
                                val = TrimpBrackers(val);
                                float[] parameters = SplitWithColumFloat(val);

                                for (int index = 0; index < parameters.Length; index++)
                                {
                                    if (coefficient)
                                    {
                                        int row = index / currentDimension[1];
                                        int col = index % currentDimension[1];
                                        mlpParameters.SetCoeficiente(currentParameter, row, col, parameters[index]);
                                    }
                                    else
                                    {
                                        mlpParameters.SetIntercept(currentParameter, index, parameters[index]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return mlpParameters;
        }
    }
}

