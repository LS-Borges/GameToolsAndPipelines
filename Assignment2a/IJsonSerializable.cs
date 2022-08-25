using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2ab
{
    public interface IJsonSerializable
    {
        public bool LoadJSON(string path);
        public bool SaveAsJSON(bool appendToFile, string path);
    }
}
