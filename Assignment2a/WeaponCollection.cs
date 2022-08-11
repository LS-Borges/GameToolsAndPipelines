using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment2a
{
    public class WeaponCollection : List<Weapon>, IPeristence
    {
        List<Weapon> collection = new List<Weapon>();

        bool IPeristence.Load(string filename)
        {
            
        }

        bool IPeristence.Save(string filename)
        {

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
            return 0;
        }
        public List<Weapon> GetAllWeaponsOfRarity(int stars)
        {
            return List<Weapon> a;
        }
        public void SortBy(string columnName)
        {
            
        }
    }
}
