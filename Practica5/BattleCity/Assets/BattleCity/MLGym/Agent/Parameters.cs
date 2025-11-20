using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLGym
{
    public class Parameters
    {
        public float[] parametersValue;
        public float timeStamp;
        public Parameters(int numParameters, float time)
        {
            parametersValue = new float[numParameters];
            timeStamp = time;
        }

        public float this[int index]
        {
            get
            {
                return parametersValue[index];
            }

            set
            {
                parametersValue[index] = value;
            }
            // get and set accessors

        }

        public int Size
        {
            get
            {
                return parametersValue.Length;
            }
        }

        public int Length
        {
            get
            {
                return parametersValue.Length;
            }
        }

        public float[] ConvertToFloatArray()
        {
            float[] ret = new float[parametersValue.Length + 1];
            for (int i = 0; i < parametersValue.Length; i++)
            {
                ret[i] = parametersValue[i];
            }
            ret[parametersValue.Length] = timeStamp;
            return ret;
        }

        public override string ToString()
        {
            return ParseToString(",");
        }

        public string ParseToString(string delimiter)
        {
            string s = "";
            for (int j = 0; j < Length; j++)
            {
                string f = parametersValue[j].ToString();
                f = f.Replace(",", ".");
                s += f + delimiter;
            }
            string tf = timeStamp.ToString();
            tf = tf.Replace(",", ".");
            s += tf;
            return s;
        }

        internal string Serialize()
        {

            /*string ret = "";
            for (int i = 0; i < metrics.Length - 1; i++)
            {
                ret += metrics[i].id + ";";
            }
            ret += metrics[metrics.Length - 1].id;*/
            return ParseToString(";");

        }
    }
}
