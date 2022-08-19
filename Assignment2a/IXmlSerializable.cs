using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2ab
{
    public interface IXmlSerializable
    {
        public bool LoadXML(string path);
        public bool SaveAsXML(string path);
    }
}
