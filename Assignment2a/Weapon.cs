using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public enum WeaponType
{
    Sword,
    Polearm,
    Claymore,
    Catalyst,
    Bow,
    None
}

namespace Assignment2a
{
    public class Weapon
    { 
        // Name,Type,Rarity,BaseAttack
        public string Name { get; set; }
        public WeaponType Type { get; set; }
        public int Rarity { get; set; }
        public int BaseAttack { get; set; }
        public string Image { get; set; }
        public string SecondaryStat { get; set; }
        public string Passive { get; set; }
        
        /// <summary>
        /// The Comparator function to check for name
        /// </summary>
        /// <param name="left">Left side Weapon</param>
        /// <param name="right">Right side Weapon</param>
        /// <returns> -1 (or any other negative value) for "less than", 0 for "equals", or 1 (or any other positive value) for "greater than"</returns>
        public static int CompareByName(Weapon left, Weapon right)
        {
            return left.Name.CompareTo(right.Name);
        }

        // CompareByType
        public static int CompareByType(Weapon left, Weapon right)
        {
            return left.Type.CompareTo(right.Type);
        }
        // CompareByRarity
        public static int CompareByRarity(Weapon left, Weapon right)
        {
            return left.Rarity.CompareTo(right.Rarity);
        }
        // CompareByBaseAttack
        public static int CompareByBaseAttack(Weapon left, Weapon right)
        {
            return left.BaseAttack.CompareTo(right.BaseAttack);
        }

        /// <summary>
        /// The Weapon string with all the properties
        /// </summary>
        /// <returns>The Weapon formated string</returns>
        public override string ToString()
        {
            return $"{Name},{Type},{Image},{Rarity},{BaseAttack},{SecondaryStat},{Passive}";
        }

        public static bool TryParse(int headerLength, string rawData, out Weapon weapon)
        {
            weapon = new Weapon();

            string[] values = rawData.Split(',');

            if (values.Length != headerLength)
            {
                Console.WriteLine("Length invalid");
                return false;
            }

            if (Enum.TryParse(values[1], out WeaponType enumResult))
            {
                weapon.Type = enumResult;
            }
            else
            {
                Console.WriteLine($"Weapon Type is not valid [{enumResult}], please change.");
                return false;
            }

            if (int.TryParse(values[3], out int intResult))
            {
                weapon.Rarity = intResult;
            }
            else
            {
                Console.WriteLine($"Rarity is not valid [{intResult}], please change.");
                return false;
            }

            if (int.TryParse(values[4], out int intResult2))
            {
                weapon.BaseAttack = intResult2;
            }
            else
            {
                Console.WriteLine($"BaseAttack is not valid [{intResult2}], please change.");
                return false;
            }

            weapon.Name = values[0];
            weapon.Image = values[2];
            weapon.SecondaryStat = values[5];
            weapon.Passive = values[6];

            return true;
        }
    }
}
