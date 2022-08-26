using System;
using System.Collections.Generic;
using System.Text;

namespace WeaponLib
{
    public interface ICsvSerializable
    {
        public bool LoadCSV(string path);
        public bool SaveAsCSV(bool appendToFile, string path);
    }
}
