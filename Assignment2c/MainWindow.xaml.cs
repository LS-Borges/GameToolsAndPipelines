using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WeaponLib;

namespace Assignment2c
{
    public partial class MainWindow : Window
    {
        WeaponCollection myWeapons = new WeaponCollection();
        List<Weapon> helperListWeapons = new List<Weapon>();
        string currentSortOption = string.Empty;
        WeaponType currentTypeSelection = WeaponType.None;
        Weapon selectedWeapon = null;
        EditWeaponWindow editWeaponWindow = new EditWeaponWindow();

        public MainWindow()
        {
            InitializeComponent();

            editWeaponWindow.Closing += (object sender, System.ComponentModel.CancelEventArgs e) =>
            {
                e.Cancel = true;
                editWeaponWindow.Hide();
            };

            var optionNames = Enum.GetNames(typeof(WeaponType));
            foreach (var option in optionNames)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = option;
                WeaponTypeBox.Items.Add(lbi);
            }
        }

        private void SaveClicked(object sender, RoutedEventArgs e)
        {
            if (myWeapons.Count == 0)
                return;

            Microsoft.Win32.SaveFileDialog fileDialog = new Microsoft.Win32.SaveFileDialog();
            fileDialog.FileName = "Collection";
            fileDialog.DefaultExt = ".csv";
            fileDialog.Filter = "CSV (.csv)|.csv;|JSON (.json)|.json|XML (.xml)|.xml";

            Nullable<bool> result = fileDialog.ShowDialog();

            if (result == true)
            {
                string filename = fileDialog.FileName;
                myWeapons.Save(false, filename);

                if(File.Exists(filename))
                {
                    MessageBox.Show("FILE SAVED");
                }
            }
        }

        private void LoadClicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Files(*.csv)|*.csv|JSON Files(*.json)|*.json|XML Files(*.xml)|*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                myWeapons.Load(openFileDialog.FileName);
                FillWeaponList();
            }
        }

        private void FillWeaponList()
        {
            if (myWeapons.Count == 0)
            {
                MessageBox.Show("EMPTY COLLECTION");
                return;
            }

            if (currentSortOption != string.Empty)
            {
                myWeapons.SortBy(currentSortOption);
            }

            if (WeaponTypeBox.SelectedIndex == -1)
            {
                WeaponTypeBox.SelectedIndex = 0;
            }

            helperListWeapons = null;
            helperListWeapons = myWeapons.GetAllWeaponsOfType(currentTypeSelection);

            if(!String.IsNullOrEmpty(FilterInput.Text))
            {
                List<Weapon> temp = new List<Weapon>();
                foreach (var item in helperListWeapons)
                {
                    if (item.ToString().IndexOf(FilterInput.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        temp.Add(item);
                    }
                }

                helperListWeapons = temp;
            }

            WeaponsViewList.ItemsSource = null;
            WeaponsViewList.Items.Clear();
            WeaponsViewList.ItemsSource = helperListWeapons;
            WeaponsViewList.SelectedIndex = -1;

            LabelCount.Content = $"Total/Displayed - {myWeapons.Count}/{helperListWeapons.Count}";
        }

        public void UpdateWeaponList()
        {
            FillWeaponList();
        }

        public void RegisterNewWeapon(Weapon weapon)
        {
            myWeapons.Add(weapon);
            FillWeaponList();
        }

        private void SortRadioSelected(object sender, RoutedEventArgs e)
        {
            var radioButton = sender as RadioButton;

            if (radioButton == null)
            {
                return;
            }
                
            string name = radioButton.Content.ToString();
            currentSortOption = name;
            FillWeaponList();
        }

        private void OpenWeaponEditWindow(EditWindowMode mode)
        {
            editWeaponWindow.SetWindowMode(mode, this);
            editWeaponWindow.Show();
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            OpenWeaponEditWindow(EditWindowMode.Add);
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            OpenWeaponEditWindow(EditWindowMode.Edit);
            editWeaponWindow.ShowSelectedWeaponData(selectedWeapon);
        }

        private void RemoveClicked(object sender, RoutedEventArgs e)
        {
            if (helperListWeapons.Count == 0)
            {
                MessageBox.Show("EMPTY COLLECTION");
                return;
            }
            else if (WeaponsViewList.SelectedIndex == -1)
            {
                MessageBox.Show("NO WEAPON SELECTED");
                return;
            }

            selectedWeapon = helperListWeapons[WeaponsViewList.SelectedIndex];
            helperListWeapons.Remove(selectedWeapon);
            myWeapons.Remove(selectedWeapon);
            selectedWeapon = null;
            FillWeaponList();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FillWeaponList();
        }

        private void WeaponType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (myWeapons.Count == 0)
            {
                MessageBox.Show("EMPTY COLLECTION");
                WeaponTypeBox.SelectedIndex = -1;
                return;
            }

            currentTypeSelection = (WeaponType)Enum.Parse(typeof(WeaponType), WeaponTypeBox.SelectedIndex.ToString());

            FillWeaponList();
        }

        private void WeaponsViewList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(helperListWeapons.Count == 0 || WeaponsViewList.SelectedIndex == -1)
            {
                return;
            }

            selectedWeapon = helperListWeapons[WeaponsViewList.SelectedIndex];
        }
    }
}
