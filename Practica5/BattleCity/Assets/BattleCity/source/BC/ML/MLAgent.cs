using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.XR.Oculus.Input;
using UnityEngine;
using UnityEngine.Rendering;






public class MLAgent : MonoBehaviour
{
    public enum ModelType { MLP = 0 }
    public TextAsset text;
    public ModelType model;
    public bool agentEnable;
    public TextAsset _standarScaler;
    [Tooltip("Eliminamos los elementos de la entrada, que no nos sirven")]
    public int[] indicesToRemove;
    public bool runTest;
    public TextAsset csvTest;
    public float accuracyTarget;
    public float accuracyThreshold;
    public float aceptThreshold;
    public List<OHE_Elements> oHE_Elements;
    public Record recorder;



    private MLPParameters mlpParameters;
    private MLPModel mlpModel;
    private PlayerPerception perception;
    private TankMove tankMove;
    private TankFire tankFire;
    private StandarScaler standarScaler;
    private OneHotEncoding oneHotEncoding;
    private float _time;


    // Start is called before the first frame update
    void Start()
    {
        if (agentEnable)
        {
            string file = text.text;
            if (model == ModelType.MLP)
            {
                mlpParameters = LoadParameters(file);
                mlpModel = new MLPModel(mlpParameters);
            }
            _time = 0f;
            Debug.Log("Parameters loaded " + mlpParameters);
            perception = GetComponent<PlayerPerception>();
            tankMove = GetComponent<TankMove>();
            tankFire = GetComponent<TankFire>();
            standarScaler = new StandarScaler(_standarScaler.text);
            oneHotEncoding = new OneHotEncoding(oHE_Elements);
            recorder.ResetInGame();
            if (runTest)
            {
                Tuple<List<MLGym.Parameters>, List<int>> tuple = Record.ReadFromCsv(csvTest.text, true);
                List<MLGym.Parameters> parameters = tuple.Item1;
                List<int> labels = tuple.Item2;
                int goals = 0;
                for (int i = 0; i < parameters.Count; i++)
                {
                    float[] input = parameters[i].ConvertToFloatArray();
                    float[] outputs = RunFeedForward(input);
                    if (i == 0)
                        Debug.Log(outputs[0] + "," + outputs[1] + "," + outputs[2]);
                    int action = this.mlpModel.Predict(outputs);
                    if (action == labels[i])
                        goals++;
                }
                float acc = goals / ((float)parameters.Count);
                float diff = Mathf.Abs(acc - accuracyTarget);
                Debug.Log("Accuracy " + acc + " Accuracy espected " + accuracyTarget + " goalds " + goals + " Examples " + parameters.Count + " Difference " + diff);
                if (diff < aceptThreshold)
                {
                    Debug.Log("Test Complete!");
                }
                else
                {
                    Debug.LogError("Error: Accuracy is not the same. Accuracy in C# " + acc + " accuracy in sklearn " + acc);
                }

            }
        }
    }


    private void Update()
    {
        GameLogic gameLogic = GameLogic.Get();
        if (gameLogic.gameMode == GameLogic.GAME_MODE.IA_DRIVEN)
        {
            _time += Time.deltaTime;
            PerceptionBase.ACTION_TYPE actions = PerceptionBase.ACTION_TYPE.MOVE_UP;
            if (_time > 1f) // Tiempo para evitar la condición de parada inicial del agente.
            {
                actions = AgentInput();
            }
            //Guarda las acciones realizadas por si las queremos usar en le contexto.
            perception.AddAction(Record.ConvertInputToLabel(actions));

            switch (actions)
            {
                case TankPerception.ACTION_TYPE.MOVE_UP:
                    Debug.Log("MOVE_UP");
                    this.tankMove.Move(Vector2.up);
                    break;
                case TankPerception.ACTION_TYPE.MOVE_DOWN:
                    Debug.Log("MOVE_DOWN");
                    this.tankMove.Move(Vector2.down);
                    break;
                case TankPerception.ACTION_TYPE.MOVE_LEFT:
                    Debug.Log("MOVE_LEFT");
                    this.tankMove.Move(Vector2.left);
                    break;
                case TankPerception.ACTION_TYPE.MOVE_RIGHT:
                    Debug.Log("MOVE_RIGHT");
                    this.tankMove.Move(Vector2.right);
                    break;
                default:
                    Debug.Log("None");
                    this.tankMove.Move(Vector2.zero);
                    break;
            }
            //El agente por defecto siempre dispara, podeis intentar aprender un comportamiento de disparo.
            this.tankFire.Fire();
        }
    }


    /// <summary>
    /// Motodo que debe llamar al modelo MLP leer de los parametros de perception y hacer las conversiones necesarias para poder ejecutar el
    /// método Runfeedforward que ejecuta la red neuronal
    /// </summary>
    /// <returns></returns>
    public PerceptionBase.ACTION_TYPE AgentInput()
    {
        int action = -1;
        switch (model)
        {
            case ModelType.MLP:
                action = 0;
                //TODO leer de los parámetros de la percepción.
                //Debe respetar el mismo orden que los datos.
                //TODO Llamar a RunFeedForward
                //guardar la toma de decisiones y despues validar si son correctas.
                recorder.AIRecord(action);
                break;
        }
        PerceptionBase.ACTION_TYPE input = Record.ConvertLabelToInput(action);
        return input;
    }

    /// <summary>
    /// Ejecuta el modelo Feedforward.
    /// </summary>
    /// <param name="modelInput"></param>
    /// <returns></returns>
    public float[] RunFeedForward(float[] modelInput)
    {
        //permite eliminar columnas de la percepción si las habeis eliminado en el modelo.
        modelInput = modelInput.Where((value, index) => !indicesToRemove.Contains(index)).ToArray();
        //TODO Hacer las transformaciónes necesarias para ejecutar el modelo

        //Guardamos el model input con las trasformaciones para poder ejecutarlo desde paython y comporbar si funciona.
        recorder.AIRecord(modelInput);
        float[] outputs = this.mlpModel.FeedForward(modelInput);

        return outputs;
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
        val = val.Trim();
        string[] values = val.Split(",");
        float[] result = new float[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            result[i] = float.Parse(values[i], System.Globalization.CultureInfo.InvariantCulture);
        }
        return result;
    }

    public static MLPParameters LoadParameters(string file)
    {
        string[] lines = file.Split("\n");
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
