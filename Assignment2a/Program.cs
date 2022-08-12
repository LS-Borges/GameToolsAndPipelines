using System;
using System.IO;

// TODO: Fill in your name and student number.
// Assignment 2a
// NAME: Leon Borges
// STUDENT NUMBER: 202-2595

namespace Assignment2a
{
    enum ColumnNames
    {
        Name,
        Type,
        Rarity,
        BaseAttack
    }
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Variables and flags

            // The path to the input file to load.
            string inputFile = string.Empty;

            // The path of the output file to save.
            string outputFile = string.Empty;

            // The flag to determine if we overwrite the output file or append to it.
            bool appendToFile = false;

            // The flag to determine if we need to display the number of entries
            bool displayCount = false;

            // The flag to determine if we need to sort the results via name.
            bool sortEnabled = false;

            // The column name to be used to determine which sort comparison function to use.
            string sortColumnName = string.Empty;

            // The results to be output to a file or to the console
            WeaponCollection results = new WeaponCollection();

            for (int i = 0; i < args.Length; i++)
            {
                // h or --help for help to output the instructions on how to use it
                if (args[i] == "-h" || args[i] == "--help")
                {
                    Console.WriteLine("-i <path> or --input <path> : loads the input file path specified (required)");
                    Console.WriteLine("-o <path> or --output <path> : saves result in the output file path specified (optional)");
                    Console.WriteLine("-c or --count : displays the number of entries in the input file (optional).");
                    Console.WriteLine("-a or --append : enables append mode when writing to an existing output file (optional)");
                    Console.WriteLine("-s or --sort <column name> : outputs the results sorted by column name");

                    break;
                }
                else if (args[i] == "-i" || args[i] == "--input")
                {
                    // Check to make sure there's a second argument for the file name.
                    if (args.Length > i + 1)
                    {
                        // stores the file name in the next argument to inputFile
                        ++i;
                        inputFile = args[i];

                        if (string.IsNullOrEmpty(inputFile))
                        {
                            Console.WriteLine("FAILED TO READ FILE");
                        }
                        else if (!File.Exists(inputFile))
                        {
                            Console.WriteLine($"{inputFile} DOES NOT EXIST");
                        }
                        else
                        {
                            // This function returns a List<Weapon> once the data is parsed.
                            results.Load(inputFile);
                            Console.WriteLine("File loaded");
                        }
                    }
                }
                else if (args[i] == "-s" || args[i] == "--sort")
                {
                    sortEnabled = true;
                    ++i;
                    sortColumnName = args[i];
                }
                else if (args[i] == "-c" || args[i] == "--count")
                {
                    displayCount = true;
                }
                else if (args[i] == "-a" || args[i] == "--append")
                {
                    appendToFile = true;
                }
                else if (args[i] == "-o" || args[i] == "--output")
                {
                    // validation to make sure we do have an argument after the flag
                    if (args.Length > i + 1)
                    {
                        // increment the index.
                        ++i;
                        string filePath = args[i];
                        if (string.IsNullOrEmpty(filePath))
                        {
                            Console.WriteLine("NO OUTPUT FILE SPECIFIED");
                        }
                        else
                        {
                            outputFile = filePath;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("The argument Arg[{0}] = [{1}] is invalid", i, args[i]);
                }
            }

            if (sortEnabled)
            {
                Console.WriteLine($"Sorting by [{sortColumnName}]");

                if (sortColumnName == "Name")
                {
                    results.Sort(Weapon.CompareByName);
                }
                else if (sortColumnName == "Type")
                {
                    results.Sort(Weapon.CompareByType);
                }
                else if (sortColumnName == "Rarity")
                {
                    results.Sort(Weapon.CompareByRarity);
                }
                else if (sortColumnName == "BaseAttack")
                {
                    results.Sort(Weapon.CompareByBaseAttack);
                }
            }

            if (displayCount)
            {
                Console.WriteLine("There are {0} entries", results.Count);
            }

            if (results.Count > 0)
            {
                results.Save(appendToFile, outputFile);
            }

            Console.WriteLine("Done!");
        }        
    }


}
