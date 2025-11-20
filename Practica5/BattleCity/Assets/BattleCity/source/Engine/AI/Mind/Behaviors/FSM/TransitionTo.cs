using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mind
{
    public class TransitionTo
    {

        private float _min;
        private float _max;
        private float _prob;
        private string _to;


        public TransitionTo(float min, float max, float pro, string to)
        {
            _min = min;
            _max = max;
            _prob = pro;
            _to = to;
        }
        public float Min
        {
            get
            {
                return _min;
            }

            set
            {
                _min = value;
            }
        }

        public float Max
        {
            get
            {
                return _max;
            }

            set
            {
                _max = value;
            }
        }

        public bool CheckProbability(float d)
        {
            return d >= _min && d < _max;
        }

        public float Prob
        {
            get
            {
                return _prob;
            }

            set
            {
                _prob = value;
            }
        }

        public string To
        {
            get
            {
                return _to;
            }

            set
            {
                _to = value;
            }
        }
    }
}


