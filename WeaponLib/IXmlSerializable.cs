using System;
using System.Collections.Generic;
using System.Text;

namespace WeaponLib
{
    public interface IXmlSerializable
    {
        public bool LoadXML(string path);
        public bool SaveAsXML(bool appendToFile, string path);
    }
}
