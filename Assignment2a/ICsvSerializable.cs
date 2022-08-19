using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2ab
{
    public interface ICsvSerializable
    {
        public bool LoadCSV(string path);
        public bool SaveAsCSV(string path);
    }
}
