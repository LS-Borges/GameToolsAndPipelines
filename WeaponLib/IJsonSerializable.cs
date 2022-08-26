using System;
using System.Collections.Generic;
using System.Text;

namespace WeaponLib
{
    public interface IJsonSerializable
    {
        public bool LoadJSON(string path);
        public bool SaveAsJSON(bool appendToFile, string path);
    }
}
