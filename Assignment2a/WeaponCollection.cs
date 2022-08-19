using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace Assignment2ab
{
    public class WeaponCollection : List<Weapon>, IPersistence, IJsonSerializable, ICsvSerializable, IXmlSerializable
    {
        // Load Functions
        public bool Load(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                Console.WriteLine("FAILED TO READ FILE");
                return false;
            }
            else if (!File.Exists(filename))
            {
                Console.WriteLine($"{filename} DOES NOT EXIST");
                return false;
            }
            else
            {
                WeaponDataParse(filename);
                return true;
            }           
        }

        public bool LoadCSV(string path)
        {

        }

        public bool LoadJSON(string path)
        {

        }

        //public bool LoadXML(string path)
        //{

        //}

        // Save Functions 
        public bool Save(bool appendToFile, string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                FileStream fs;

                // Check if the append flag is set, and if so, then open the file in append mode; otherwise, create the file to write.
                if (appendToFile && File.Exists((filename)))
                {
                    fs = File.Open(filename, FileMode.Append);
                }
                else
                {
                    fs = File.Open(filename, FileMode.Create);
                }

                // opens a stream writer with the file handle to write to the output file.
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    // Hint: use writer.WriteLine
                    writer.WriteLine("Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive");

                    for (int j = 0; j < this.Count; ++j)
                    {
                        writer.WriteLine(this[j].ToString());
                    }

                    Console.WriteLine("File has been saved");
                    return true;
                }
            }
            else
            {
                // prints out each entry in the weapon list results.
                for (int i = 0; i < this.Count; i++)
                {
                    Console.WriteLine(this[i]);
                }

                Console.WriteLine("OUTPUT FILE DOES NOT EXIST OR NOT SPECIFIED");
                return false;
            }
        }

        public bool SaveAsCSV(string path)
        {

        }

        public bool SaveAsJSON(string path)
        {
            string data = JsonUtility.ToJson(path., true);
            File.WriteAllText(path, data);
            return true;
        }

        //public bool SaveAsXML(string path)
        //{

        //}

        // Get Functions
        public int GetHighestBaseAttack()
        {
            int result = 0;

            foreach(var weapon in this)
            {
                if(weapon.BaseAttack > result)
                {
                    result = weapon.BaseAttack;
                }
            }
            return result;
        }

        public int GetLowestBaseAttack()
        {
            int result = 100000;

            foreach (var weapon in this)
            {
                if (weapon.BaseAttack < result)
                {
                    result = weapon.BaseAttack;
                }
            }
            return result;
        }

        public List<Weapon> GetAllWeaponsOfType(WeaponType type)
        {
            List<Weapon> resultList = new List<Weapon>();

            foreach (var weapon in this)
            {
                if (weapon.Type == type)
                {
                    resultList.Add(weapon);
                }
            }

            return resultList;
        }

        public List<Weapon> GetAllWeaponsOfRarity(int stars)
        {
            List<Weapon> resultList = new List<Weapon>();

            foreach (var weapon in this)
            {
                if (weapon.Rarity == stars)
                {
                    resultList.Add(weapon);
                }
            }

            return resultList;
        }

        public void SortBy(string columnName)
        {
            
        }

        private void WeaponDataParse(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string header = reader.ReadLine();
                string[] headerValues = header.Split(',');

                while (reader.Peek() > 0)
                {
                    string line = reader.ReadLine();

                    if (Weapon.TryParse(headerValues.Length, line, out Weapon weapon))
                    {
                        Add(weapon);
                    }
                }
            }
        }
    }
}
