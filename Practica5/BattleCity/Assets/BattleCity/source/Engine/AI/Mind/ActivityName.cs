using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mind
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ActivityDescription : System.Attribute
    {
        private string _name;
        private string[] _input;
        private string[] _output;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string[] Input
        {
            get { return _input; }
            set { _input = value; }
        }

        public string[] Output
        {
            get { return _output; }
            set { _output = value; }
        }
    }
}

