public class MetricsData
{
    public float time;
    public int checkpoints;
    public int collisions;
    public string id;

    public MetricsData()
    {
        id = "";
        time = 0f;
        checkpoints = 0;
        collisions = 0;
    }

    public static string GenerateIDArray(MetricsData[] metrics)
    {
        string ret = "";
        for(int i = 0; i < metrics.Length-1; i++)
        {
            ret += metrics[i].id + ";";
        }
        ret += metrics[metrics.Length-1].id;
        return ret;
    }

    public static string GenerateTimeArray(MetricsData[] metrics)
    {
        string ret = "";
        for (int i = 0; i < metrics.Length - 1; i++)
        {
            ret += metrics[i].time + ";";
        }
        ret += metrics[metrics.Length - 1].time;
        return ret;
    }

    public static string GenerateCheckpointsArray(MetricsData[] metrics)
    {
        string ret = "";
        for (int i = 0; i < metrics.Length - 1; i++)
        {
            ret += metrics[i].checkpoints + ";";
        }
        ret += metrics[metrics.Length - 1].checkpoints;
        return ret;
    }

    public static string GenerateCollisionsArray(MetricsData[] metrics)
    {
        string ret = "";
        for (int i = 0; i < metrics.Length - 1; i++)
        {
            ret += metrics[i].collisions + ";";
        }
        ret += metrics[metrics.Length - 1].collisions;
        return ret;
    }
}
