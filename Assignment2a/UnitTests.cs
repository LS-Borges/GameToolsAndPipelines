using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Assignment2ab
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
            Assert.IsTrue(weaponCollection.Load(inputPath));
            int result = weaponCollection.GetHighestBaseAttack();
            Assert.AreEqual(result, 48);
        }

        [Test]
        public void WeaponCollection_GetLowestBaseAttack_LowestValue()
        {
            Assert.IsTrue(weaponCollection.Load(inputPath));
            int result = weaponCollection.GetLowestBaseAttack();
            Assert.AreEqual(result, 23);
        }

        [TestCase(WeaponType.Sword, 21)]
        public void WeaponCollection_GetAllWeaponsOfType_ListOfWeapons(WeaponType type, int expectedValue)
        {
            Assert.IsTrue(weaponCollection.Load(inputPath));
            int result = weaponCollection.GetAllWeaponsOfType(type).Count;
            Assert.AreEqual(result, expectedValue);
        }


        [TestCase(5, 10)]
        public void WeaponCollection_GetAllWeaponsOfRarity_ListOfWeapons(int stars, int expectedValue)
        {
            Assert.IsTrue(weaponCollection.Load(inputPath));
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
            Assert.IsTrue(weaponCollection.Load(inputPath));
            Assert.IsTrue(weaponCollection.Save(false, outputPath));
            Assert.IsTrue(weaponCollection.Load(outputPath));
            Assert.IsTrue(weaponCollection.Count > 0);
        }

        [Test]
        public void WeaponCollection_SaveEmpty_TrueAndEmpty()
        {
            // After saving an empty WeaponCollection, load the file and expect WeaponCollection to be empty.
            weaponCollection.Clear();
            Assert.IsTrue(weaponCollection.Save(false, outputPath));
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
            string line = "1,Bulbasaur,A,B,C,65,65";
            string[] values = line.Split(',');
            Weapon actual;

            Assert.IsFalse(Weapon.TryParse(values.Length, line, out actual));
            Assert.IsTrue(actual.Name == null);
            Assert.IsTrue(actual.Type == 0);
            Assert.IsTrue(actual.BaseAttack == 0);
            Assert.IsTrue(actual.Image == null);
            Assert.IsTrue(actual.Rarity == 0);
            Assert.IsTrue(actual.SecondaryStat == null);
            Assert.IsTrue(actual.Passive == null);
        }

        //Test LoadCsv Valid
        [Test]
        public void WeaponCollection_Load_Save_Load_ValidCsv()
        {
            Assert.IsTrue(weaponCollection.Load(inputPath));
            Assert.IsTrue(weaponCollection.Save(false, outputPath));
            Assert.IsTrue(weaponCollection.Load(outputPath));
            Assert.AreEqual(weaponCollection.Count, 95);
        }

        [Test]
        public void WeaponCollection_Load_SaveAsCSV_LoadCSV_ValidCsv()
        {
            Assert.IsTrue(weaponCollection.Load(inputPath));
            Assert.IsTrue(weaponCollection.SaveAsCSV(outputPath));
            Assert.IsTrue(weaponCollection.LoadCSV(outputPath));
            Assert.AreEqual(weaponCollection.Count, 95);
        }

        //Test SaveAsCSV Empty
        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidCsv()
        {
            weaponCollection.Clear();
            Assert.IsTrue(weaponCollection.SaveAsCSV(outputPath));
            Assert.IsTrue(weaponCollection.Load(outputPath));
            Assert.IsTrue(weaponCollection.Count == 0);
        }

    }
}

//Test LoadJson Valid
//1.WeaponCollection_Load_Save_Load_ValidJson - Load the data2.csv and Save () it to
//weapons.json and call Load () output and validate that there’s 95 entries
//2. WeaponCollection_Load_SaveAsJSON_Load_ValidJson - Load the data2.csv and
//SaveAsJSON () it to weapons.json and call Load () output and validate that there’s 95
//entries
//3. WeaponCollection_Load_SaveAsJSON_LoadJSON_ValidJson - Load the data2.csv
//and SaveAsJSON () it to weapons.json and call LoadJSON () on output and validate that
//there’s 95 entries
//4. WeaponCollection_Load_Save_LoadJSON_ValidJson - Load the data2.csv and
//Save () it to weapons.json and call LoadJSON () on output and validate that there’s 95
//entries

//Test LoadXML Valid
//1. WeaponCollection_Load_Save_Load_ValidXml - Load the data2.csv and Save () it to
//weapons.xml and Load () output and validate that there’s 95 entries
//2. WeaponCollection_Load_SaveAsXML_LoadXML_ValidXml - Load the data2.csv and
//SaveAsXML () it to weapons.xml and LoadXML () output and validate that there’s 95
//entries

//Test SaveAsJSON Empty
//1. WeaponCollection_SaveEmpty_Load_ValidJson - Create an empty
//WeaponCollection, call SaveAsJSON () to empty.json, and Load () the output and verify
//the WeaponCollection has a Count of 0

//Test SaveAsXML Empty
//1. WeaponCollection_SaveEmpty_Load_ValidXml - Create an empty
//WeaponCollection, call SaveAsXML () to empty.xml, and Load and verify the
//WeaponCollection has a Count of 0

//Test Load InvalidFormat
//1. WeaponCollection_Load_SaveJSON_LoadXML_InvalidXml - Load the data2.csv and
//SaveAsJSON () it to weapons.json and call LoadXML () output and validate that it returns
//false, and there’s 0 entries
//2. WeaponCollection_Load_SaveXML_LoadJSON_InvalidJson - Load the data2.csv
//and SaveAsXML () it to weapons.xml and call LoadJSON () output and validate that it
//returns false, and there’s 0 entries
//3. WeaponCollection_ValidCsv_LoadXML_InvalidXml - LoadXML() on the data2.csv
//and validate that returns false, and there’s 0 entries
//4. WeaponCollection_ValidCsv_LoadJSON_InvalidJson - LoadJSON() on the
//data2.csv and validate that Load returns false, and there’s 0 entries