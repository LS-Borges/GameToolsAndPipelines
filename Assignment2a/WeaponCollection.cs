using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text.Json;

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
                string extension = Path.GetExtension(filename);

                if (extension.ToLower() == ".csv")
                {
                    LoadCSV(filename);
                    return true;
                }
                else if (extension.ToLower() == ".json")
                {
                    LoadJSON(filename);
                    return true;
                }
                else if (extension.ToLower() == ".xml")
                {
                    LoadXML(filename);
                    return true;
                }
                else
                {
                    Console.WriteLine($"{extension} EXTENSION NOT SUPPORTED");
                    return false;
                }
            }           
        }

        public bool LoadCSV(string filename)
        {
            WeaponDataParse(filename);
            return true;
        }

        public bool LoadJSON(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                WeaponCollection temp = new WeaponCollection();

                if (reader.Peek() > 0)
                {
                    string data = reader.ReadToEnd();
                    temp = JsonSerializer.Deserialize<WeaponCollection>(data);

                    foreach (var weapon in temp)
                    {
                        Add(weapon);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool LoadXML(string filename)
        {
            return true;
        }

        // Save Functions 
        public bool Save(bool appendToFile, string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                string extension = Path.GetExtension(filename);

                if (extension.ToLower() == ".csv")
                {
                    SaveAsCSV(appendToFile, filename);
                    return true;
                }
                else if (extension.ToLower() == ".json")
                {
                    SaveAsJSON(appendToFile, filename);
                    return true;
                }
                else if (extension.ToLower() == ".xml")
                {
                    SaveAsXML(appendToFile, filename);
                    return true;
                }
                else
                {
                    Console.WriteLine($"{extension} EXTENSION NOT SUPPORTED");
                    return false;
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

        public bool SaveAsCSV(bool appendToFile, string filename)
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

        public bool SaveAsJSON(bool appendToFile, string filename)
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
                var options = new JsonSerializerOptions { WriteIndented = true };
                string data = JsonSerializer.Serialize<WeaponCollection>(this, options);
                writer.WriteLine(data);
            }

            Console.WriteLine("File has been saved");
            return true;
        }

        public bool SaveAsXML(bool appendToFile, string filename)
        {
            FileStream fs;

            // Check if the append flag is set, and if so, then open the file in append mode; otherwise, create the file to write.
            if (appendToFile && File.Exists((filename)))
            {
                fs = File.Open(filename, FileMode.Append, FileAccess.ReadWrite);
            }
            else
            {
                fs = File.Open(filename, FileMode.Create, FileAccess.ReadWrite);
            }

            XmlSerializer xs = new XmlSerializer(typeof(WeaponCollection));
            xs.Serialize(fs, this);

            return true;
        }

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
            if(columnName.ToLower() == "name")
            {
                this.Sort(Weapon.CompareByName);
            }
            else if (columnName.ToLower() == "type")
            {
                this.Sort(Weapon.CompareByType);
            }
            else if (columnName.ToLower() == "rarity")
            {
                this.Sort(Weapon.CompareByRarity);
            }
            else if (columnName.ToLower() == "baseattack")
            {
                this.Sort(Weapon.CompareByBaseAttack);
            }
            else if (columnName.ToLower() == "image")
            {
                this.Sort(Weapon.CompareByImage);
            }
            else if (columnName.ToLower() == "secondarystat")
            {
                this.Sort(Weapon.CompareBySecondaryStat);
            }
            else if (columnName.ToLower() == "passive")
            {
                this.Sort(Weapon.CompareByPassive);
            } 
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
