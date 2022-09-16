using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Xml.Serialization;
using TextureAtlasLib;
using Path = System.IO.Path;

namespace Assignment3
{
    public partial class MainWindow : Window
    {
        private Spritesheet spriteSheet = new Spritesheet();
        bool includeMetaData = false;

        string fullPath = string.Empty;
        bool fileWasOpened = false;
        List<Image> images = new List<Image>();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            spriteSheet.InitializeSheet();
        }

        private void MetadataCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            includeMetaData = true;
            spriteSheet.IncludeMetaData = includeMetaData;
        }

        private void MetadataCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            includeMetaData = false;
            spriteSheet.IncludeMetaData = includeMetaData;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog fileDialog = new Microsoft.Win32.SaveFileDialog();
            fileDialog.FileName = "SpriteSheet";
            fileDialog.DefaultExt = ".png";
            fileDialog.Filter = "PNG (.png)|.png";

            Nullable<bool> result = fileDialog.ShowDialog();

            if (result == true)
            {
                spriteSheet.OutputDirectory = Path.GetDirectoryName(fileDialog.FileName);
                spriteSheet.OutputFile = Path.GetFileNameWithoutExtension(fileDialog.FileName) + ".xml";
                tbOutputDir.Text = spriteSheet.OutputDirectory;
                tbOutputFile.Text = Path.GetFileName(fileDialog.FileName);
                spriteSheet.PNGOutputFile = tbOutputFile.Text;
                fileWasOpened = true;
                ProjectName.Text = spriteSheet.OutputFile;
                fullPath = spriteSheet.OutputDirectory + spriteSheet.OutputFile;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PNG Files(*.png)|*.png";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Images Selector";
            Nullable<bool> result = openFileDialog.ShowDialog();

            if (result == true)
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    spriteSheet.InputPaths.Add(file);
                    Image image = new Image();

                    try
                    {
                        image.Source = new BitmapImage(new Uri(file));
                    }
                    catch (Exception i)
                    {
                        MessageBox.Show("Loading Image failed:" + i.Message);
                        continue;
                    }

                    images.Add(image);
                }

                ImagesBox.DataContext = images;
                ImagesBox.ItemsSource = images;
                tbColumns.Text = ImagesBox.Items.Count.ToString();
                spriteSheet.Columns = ImagesBox.Items.Count;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Remove();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(tbColumns.Text, out int columns);
            spriteSheet.Columns = columns;
            spriteSheet.OutputFile = tbOutputFile.Text;
            spriteSheet.OutputDirectory = spriteSheet.OutputDirectory;
            spriteSheet.IncludeMetaData = includeMetaData;
            spriteSheet.InputPaths = spriteSheet.InputPaths;

            spriteSheet.Generate(true);

            string message = (includeMetaData == true) ? "SpriteSheet and metadata file created" : "SpriteSheet created";

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = @$"{spriteSheet.OutputDirectory}";
            process.Start();
        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (fileWasOpened)
            {
                string message = "Would you like to save your changes?";
                string caption = "File detected.";
                MessageBoxButton button = MessageBoxButton.YesNo;
                var result = MessageBox.Show(message, caption, button, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.None:
                        break;
                    case MessageBoxResult.OK:
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.Yes:
                        Save();
                        Reset();
                        break;
                    case MessageBoxResult.No:
                        Reset();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Reset();
            }
        }

        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (fileWasOpened)
            {
                string message = "Would you like to save your changes?";
                string caption = "File detected.";
                MessageBoxButton button = MessageBoxButton.YesNo;
                var result = MessageBox.Show(message, caption, button, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.None:
                        break;
                    case MessageBoxResult.OK:
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.Yes:
                        Save();
                        Open();
                        break;
                    case MessageBoxResult.No:
                        Open();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Open();
            }
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (fileWasOpened)
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
        }

        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (fileWasOpened)
            {
                string message = "Would you like to save your changes?";
                string caption = "File detected.";
                MessageBoxButton button = MessageBoxButton.YesNo;
                var result = MessageBox.Show(message, caption, button, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.None:
                        break;
                    case MessageBoxResult.OK:
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.Yes:
                        Save();
                        Application.Current.Shutdown();
                        break;
                    case MessageBoxResult.No:
                        Reset();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void SaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveAs();
        }

        private void RemoveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (spriteSheet.InputPaths.Count < 1)
                e.CanExecute = false;
            else if (spriteSheet.InputPaths.Count >= 1)
                e.CanExecute = true;
        }

        private void RemoveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Remove();
        }

        private void RemoveAllCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (spriteSheet.InputPaths.Count < 1)
                e.CanExecute = false;
            else if (spriteSheet.InputPaths.Count >= 1)
                e.CanExecute = true;
        }

        private void RemoveAllCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            spriteSheet.InputPaths.Clear();
            images.Clear();
            ImagesBox.ItemsSource = null;
            ImagesBox.SelectedIndex = -1;
            tbColumns.Text = ImagesBox.Items.Count.ToString();
            spriteSheet.Columns = ImagesBox.Items.Count;
        }

        private void Reset()
        {
            tbColumns.Text = String.Empty;
            tbOutputDir.Text = String.Empty;
            tbOutputFile.Text = String.Empty;
            includeMetaData = false;
            MetadataCheckbox.IsChecked = false;
            ImagesBox.ItemsSource = null;
            ImagesBox.SelectedIndex = -1;
            spriteSheet.InputPaths.Clear();
            spriteSheet.IncludeMetaData = false;
            spriteSheet.PNGOutputFile = String.Empty;
            spriteSheet.OutputFile = String.Empty;
            spriteSheet.OutputDirectory = String.Empty;
            spriteSheet.Columns = 0;
            images.Clear();
            fileWasOpened = false;
            tbColumns.Text = ImagesBox.Items.Count.ToString();
        }

        private void Save()
        {
            try
            {
                FileMode mode = FileMode.Open;

                if (File.Exists((fullPath)))
                {
                    using (FileStream fs = new FileStream(fullPath, mode, FileAccess.ReadWrite))
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(Spritesheet));
                        xs.Serialize(fs, spriteSheet);
                    }
                }
                else
                {
                    SaveAs();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Saving file as XML file failed.\nException: " + ex.Message);
                return;
            }
        }

        private void SaveAs()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "SpriteSheet"; 
            dlg.DefaultExt = ".xml"; 
            dlg.Filter = "XML File (.xml)|.xml";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    fullPath = dlg.FileName;
                    spriteSheet.OutputDirectory = Path.GetDirectoryName(dlg.FileName);
                    spriteSheet.OutputFile = Path.GetFileName(dlg.FileName);
                    tbOutputDir.Text = spriteSheet.OutputDirectory;
                    ProjectName.Text = spriteSheet.OutputFile;

                    FileMode mode = FileMode.Create;

                    using (FileStream fs = new FileStream(fullPath, mode, FileAccess.ReadWrite))
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(Spritesheet));
                        xs.Serialize(fs, spriteSheet);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Saving file as XML file failed.\nException: " + ex.Message);
                    return;
                }

                fileWasOpened = true;
            }
        }

        private void Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files(*.xml)|*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (openFileDialog.ShowDialog() == true)
            {
                using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    List<string> validImages = new List<string>();
                    List<string> missingImages = new List<string>();
                    Spritesheet tempSheet = new Spritesheet();

                    try
                    {
                        XmlSerializer xs = new XmlSerializer(typeof(Spritesheet));
                        tempSheet = (Spritesheet)xs.Deserialize(fileStream);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show("Loading file failed. Error: " + ex.Message);
                        return;
                    }

                    spriteSheet.OutputDirectory = tempSheet.OutputDirectory;
                    spriteSheet.OutputFile = tempSheet.OutputFile;
                    spriteSheet.IncludeMetaData = tempSheet.IncludeMetaData;
                    spriteSheet.PNGOutputFile = tempSheet.PNGOutputFile;
                    tbOutputDir.Text = spriteSheet.OutputDirectory;
                    tbOutputFile.Text = spriteSheet.PNGOutputFile;
                    MetadataCheckbox.IsChecked = includeMetaData;
                    ProjectName.Text = spriteSheet.OutputFile;

                    spriteSheet.InputPaths.Clear();

                    foreach (var image in tempSheet.InputPaths)
                    {
                        if (!File.Exists(image))
                        {
                            missingImages.Add(image);
                        }
                        else
                        {
                            validImages.Add(image);
                        }
                    }

                    foreach (var image in validImages)
                    {
                        BitmapImage bitmap = new BitmapImage();
                        Image newImage = new Image();

                        try
                        {
                            newImage.Source = new BitmapImage(new Uri(image));
                        }
                        catch (Exception i)
                        {
                            MessageBox.Show("Loading Image failed:" + i.Message);
                            continue;
                        }

                        images.Add(newImage);
                    }

                    if (missingImages.Count > 0)
                    {
                        string missingItems = "Missing Images:\n";
                        for (int i = 0; i < missingImages.Count; i++)
                        {
                            missingItems += missingImages[i] + "\n";
                        }
                        MessageBox.Show(missingItems);
                    }

                    spriteSheet.InputPaths = validImages;
                    ImagesBox.ItemsSource = images;
                    fileWasOpened = true;
                    tbColumns.Text = ImagesBox.Items.Count.ToString();
                    spriteSheet.Columns = ImagesBox.Items.Count;
                }
            }
        }

        private void Remove()
        {
            if (spriteSheet.InputPaths.Count == 0)
            {
                MessageBox.Show("EMPTY COLLECTION");
                return;
            }
            else if (ImagesBox.SelectedIndex == -1)
            {
                MessageBox.Show("NO IMAGE SELECTED");
                return;
            }

            var selectedImage = images[ImagesBox.SelectedIndex];
            var selectedPath = spriteSheet.InputPaths[ImagesBox.SelectedIndex];
            spriteSheet.InputPaths.Remove(selectedPath);
            images.Remove(selectedImage);
            ImagesBox.ItemsSource = null;
            ImagesBox.DataContext = images;
            ImagesBox.ItemsSource = images;
            ImagesBox.SelectedIndex = -1;
            tbColumns.Text = ImagesBox.Items.Count.ToString();
            spriteSheet.Columns = ImagesBox.Items.Count;
        }
    }

    [XmlRoot("CustomCommand")]
    public static class CustomCommands
    {
        // Code taken from: https://www.wpf-tutorial.com/commands/implementing-custom-commands/
        public static readonly RoutedUICommand Exit = new RoutedUICommand
            (
                "Exit",
                "Exit",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.F4, ModifierKeys.Alt)
                }
            );

        //Define more commands here, just like the one above

        public static readonly RoutedUICommand SaveAs = new RoutedUICommand
            (
                "Save As",
                "Save As",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.S, ModifierKeys.Alt)
                }
            );

        public static readonly RoutedUICommand Remove = new RoutedUICommand
            (
                "Remove",
                "Remove",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.X, ModifierKeys.Control)
                }
            );

        public static readonly RoutedUICommand RemoveAll = new RoutedUICommand
            (
                "Remove All",
                "Remove All",
                typeof(CustomCommands),
                new InputGestureCollection()
                {
                    new KeyGesture(Key.X, ModifierKeys.Alt)
                }
            );
    }
}
