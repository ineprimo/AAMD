using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static TankPerception;


public class Record : MonoBehaviour
{
    public bool recordMode;
    public PlayerPerception perception;
    public Transform player;
    public PlayerInputController keyboardInput;
    public float snapshotTime;
    public string csvOutput;
    public bool saveInGameDataToTest=false;

    private List<MLGym.Parameters> parameters;
    private List<PerceptionBase.ACTION_TYPE> actions;
    private float time;
    private float totalTime;

    private List<float[]> inGamePerceptions;
    private List<int> inGameActions;

    // Start is called before the first frame update
    void Start()
    {
        parameters = new List<MLGym.Parameters>();
        actions = new List<PerceptionBase.ACTION_TYPE>();
        time = 0f;
        totalTime = 0f;
        GameLogic.Get().gameFinishCallback += Save;
    }

    public void ResetInGame()
    {
        inGamePerceptions = new List<float[]>();
        inGameActions = new List<int>();
    }
    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;

        if (recordMode)
        {
            time += Time.deltaTime;
            if(time > snapshotTime)
            {
                time = time - snapshotTime;
                Debug.Log("RecordSnapshot "+ (actions.Count + 1));
                RecordSnapshot(totalTime);
            }
        }
    }

    /// <summary>
    /// Alacena una foto de la percepcion.
    /// </summary>
    /// <param name="t"></param>
    public void RecordSnapshot(float t)
    {
        MLGym.Parameters p = perception.Parameters;
        PerceptionBase.ACTION_TYPE input  = keyboardInput.GetLastInput();
        perception.AddAction(ConvertInputToLabel(input));
        parameters.Add(p);
        actions.Add(input);
    }


    /// <summary>
    /// Convierte una acción numerica al enumerado.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static PerceptionBase.ACTION_TYPE ConvertLabelToInput(int action)
    {
        return (PerceptionBase.ACTION_TYPE)action;
    }

    /// <summary>
    /// Convierte el enumerado a la acción numérica.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static int ConvertInputToLabel(PerceptionBase.ACTION_TYPE action)
    {
        int act = (int)action;
        return act;
    }

    private void OnDestroy()
    {
        GameLogic gl = GameLogic.Get();
        if(gl != null)
        {
            gl.gameFinishCallback -= Save;
        }
    }

    /// <summary>
    /// El método es llamado cuando se supera el nivel para guardar la partida y poder entrenar si estamso en recorded mode.
    /// </summary>
    /// <param name="win"></param>
    public void Save(bool win)
    {
        if (recordMode)
        {
            string csvFormat = ConvertToCSV(PlayerPerception.GetParameterNames(), parameters, actions);
            csvFormat += win ? "win" : "game over";
            string date = DateTime.Now.ToString();
            date = date.Replace("/", "_");
            date = date.Replace(":", "_");
            File.WriteAllText(csvOutput + "_" + date + ".csv", csvFormat);
            Debug.Log("File " + csvOutput + " save");
        }
    }


    /// <summary>
    /// Utilidad que permite convertir las acciones y la percepción en un CSV
    /// </summary>
    /// <param name="parametersName"></param>
    /// <param name="parameters"></param>
    /// <param name="actions"></param>
    /// <returns></returns>
    public static string ConvertToCSV(string[] parametersName, List<MLGym.Parameters> parameters, List<PerceptionBase.ACTION_TYPE> actions)
    {
        string csv = "";
        for(int i = 0; i < parametersName.Length; i++)
        {
            csv += parametersName[i] + ",";
        }
        csv += "time,";
        csv += "action\n";

        for (int i = 0; i < parameters.Count; i++)
        {
            MLGym.Parameters p = parameters[i];
            csv += p.ToString();
            csv += ",";
            int act = ConvertInputToLabel(actions[i]);
            csv += act.ToString() + "\n";
        }

        return csv;
    }

    //Permite ejecutar test para probar la implementación del MLP
    public static Tuple<List<MLGym.Parameters>, List<int>> ReadFromCsv(string csv, bool ignoreFirstLine)
    {
        Tuple<List<MLGym.Parameters>, List<int>> output;
        List<MLGym.Parameters> paremters = new List<MLGym.Parameters>();
        List<int> labels = new List<int>();
        string[] lines = csv.Split("\n");
        for (int i = ignoreFirstLine ? 1 : 0; i < lines.Length; i++)
        {
            if (lines[i].Trim() != "")
            {
                string line = lines[i];
                string[] fields = line.Split(",");
                MLGym.Parameters parameter = new MLGym.Parameters(fields.Length - 1,0f);
                for (int j = 0; j < fields.Length - 1; j++)
                {
                    float value = float.Parse(fields[j], System.Globalization.CultureInfo.InvariantCulture);
                    parameter[j] = value;
                }
                paremters.Add(parameter);
                string label = fields[fields.Length - 1].Trim();
                int lab = int.Parse(label);
                labels.Add(lab);
            }
        }
        output = new Tuple<List<MLGym.Parameters>, List<int>>(paremters, labels);
        return output;
    }

    ///Metodos que sirven para almacenar las acciones que tima el agente en modo AI_Driven para luego poder guardar en un fichero y testear si el resultados es correcto
    public void AIRecord(float[] modelInput, int action)
    {
        inGamePerceptions.Add(modelInput);
        inGameActions.Add(action);
    }

    public void AIRecord(float[] modelInput)
    {
        inGamePerceptions.Add(modelInput);
    }

    public void AIRecord( int action)
    {
        inGameActions.Add(action);
    }

    //Guarda la información en el fichero especificado para luego poder testear si las decisiones que toma
    //Tu agente usando el perceptrón son las mismas que se tomarian desde Python.
    public void SaveInGameData(string file)
    {
        if (!saveInGameDataToTest)
            return;
        string s = "";
        for (int i = 0; i <inGamePerceptions.Count; i++)
        {
            float[] input = inGamePerceptions[i];
            if( i == 0)
            {
                for (int j = 0; j < input.Length; j++)
                {
                    s += j + ",";
                }
                s += (input.Length + 1) +"\n";
            }
            int act = inGameActions[i];
            for(int j = 0; j <  input.Length; j++)
            {
                string t = "" + input[j];
                t = t.Replace(",", ".");
                s += t + ",";
            }
            s += act + "\n";
        }
        File.WriteAllText(file + ".csv", s);
        Debug.Log("File " + file + " save");
    }
}
