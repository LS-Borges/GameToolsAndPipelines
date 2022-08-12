using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Assignment2a
{
    [TestFixture]
    public class UnitTests
    {
        private WeaponCollection weaponCollection;
        private string inputPath;
        private string outputPath;

        const string INPUT_FILE = "data2.csv";
        const string OUTPUT_FILE = "output.csv";

        // A helper function to get the directory of where the actual path is.
        private string CombineToAppPath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        [SetUp]
        public void SetUp()
        {
            inputPath = CombineToAppPath(INPUT_FILE);
            outputPath = CombineToAppPath(OUTPUT_FILE);
            weaponCollection = new WeaponCollection();
        }

        [TearDown]
        public void CleanUp()
        {
            // We remove the output file after we are done.
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }

        // WeaponCollection Unit Tests
        [Test]
        public void WeaponCollection_GetHighestBaseAttack_HighestValue()
        {
            weaponCollection.Load(inputPath);
            int result = weaponCollection.GetHighestBaseAttack();
            Assert.AreEqual(result, 48);
        }

        [Test]
        public void WeaponCollection_GetLowestBaseAttack_LowestValue()
        {
            weaponCollection.Load(inputPath);
            int result = weaponCollection.GetLowestBaseAttack();
            Assert.AreEqual(result, 23);
        }

        [TestCase(WeaponType.Sword, 21)]
        public void WeaponCollection_GetAllWeaponsOfType_ListOfWeapons(WeaponType type, int expectedValue)
        {
            weaponCollection.Load(inputPath);
            int result = weaponCollection.GetAllWeaponsOfType(type).Count;
            Assert.AreEqual(result, expectedValue);
        }


        [TestCase(5, 10)]
        public void WeaponCollection_GetAllWeaponsOfRarity_ListOfWeapons(int stars, int expectedValue)
        {
            // TODO: call WeaponCollection.GetAllWeaponsOfRarity(stars) and confirm that the weapons list returns Count matches the expected value using asserts.
            weaponCollection.Load(inputPath);
            int result = weaponCollection.GetAllWeaponsOfRarity(stars).Count;
            Assert.AreEqual(result, expectedValue);
        }

        [Test]
        public void WeaponCollection_LoadThatExistAndValid_True()
        {
            Assert.IsTrue(weaponCollection.Load(inputPath));
            Assert.AreEqual(weaponCollection.Count, 95);
        }

        [Test]
        public void WeaponCollection_LoadThatDoesNotExist_FalseAndEmpty()
        {
            Assert.IsFalse(weaponCollection.Load("BadFilePath.png"));
            Assert.IsTrue(weaponCollection.Count == 0);
        }

        [Test]
        public void WeaponCollection_SaveWithValuesCanLoad_TrueAndNotEmpty()
        {
            // TODO: save returns true, load returns true, and WeaponCollection is not empty.

            Assert.IsTrue(weaponCollection.Save(outputPath));
            Assert.IsTrue(weaponCollection.Load(outputPath));
            Assert.IsTrue(weaponCollection.Count > 0);
        }

        [Test]
        public void WeaponCollection_SaveEmpty_TrueAndEmpty()
        {
            // After saving an empty WeaponCollection, load the file and expect WeaponCollection to be empty.
            weaponCollection.Clear();
            Assert.IsTrue(weaponCollection.Save(outputPath));
            Assert.IsTrue(weaponCollection.Load(outputPath));
            Assert.IsTrue(weaponCollection.Count == 0);
        }

        // Weapon Unit Tests
        [Test]
        public void Weapon_TryParseValidLine_TruePropertiesSet()
        {
            Weapon expected = null;

            expected = new Weapon()
            {
                Name = "Skyward Blade",
                Type = WeaponType.Sword,
                Image = "https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png",
                Rarity = 5,
                BaseAttack = 46,
                SecondaryStat = "Energy Recharge",
                Passive = "Sky-Piercing Fang"
            };

            string line = "Skyward Blade,Sword,https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png,5,46,Energy Recharge,Sky-Piercing Fang";
            string[] values = line.Split(',');
            Weapon actual;

            Assert.IsTrue(Weapon.TryParse(values.Length, line, out actual));
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.AreEqual(expected.BaseAttack, actual.BaseAttack);
            Assert.AreEqual(expected.Image, actual.Image);
            Assert.AreEqual(expected.Rarity, actual.Rarity);
            Assert.AreEqual(expected.SecondaryStat, actual.SecondaryStat);
            Assert.AreEqual(expected.Passive, actual.Passive);
        }

        [Test]
        public void Weapon_TryParseInvalidLine_FalseNull()
        {
            // TODO: use "1,Bulbasaur,A,B,C,65,65", Weapon.TryParse returns false, and Weapon is null.
        }
    }
}
