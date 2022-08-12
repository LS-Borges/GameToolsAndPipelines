using System.Collections.Generic;
using System.IO;

namespace Assignment2a
{
    public class WeaponCollection : List<Weapon>, IPersistence
    {
        public bool Load(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                Console.WriteLine("FAILED TO READ FILE");
            }
            else if (!File.Exists(filename))
            {
                Console.WriteLine($"{filename} DOES NOT EXIST");
            }
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
