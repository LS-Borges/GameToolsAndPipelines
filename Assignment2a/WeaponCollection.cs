using System.Collections.Generic;
using System.IO;

namespace Assignment2a
{
    public class WeaponCollection : List<Weapon>, IPersistence
    {
        List<Weapon> collection = new List<Weapon>();

        public bool Load(string filename)
        {
            //do error handling first
            WeaponDataParse(filename);
            return true;
        }

        public bool Save(string filename)
        {
            return true;
        }

        public int GetHighestBaseAttack()
        {
            
            return 0;
        }

        public int GetLowestBaseAttack()
        {
            return 0;
        }

        public List<Weapon> GetAllWeaponsOfType(WeaponType type)
        {
            return null;
        }

        public List<Weapon> GetAllWeaponsOfRarity(int stars)
        {
            return null;
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
