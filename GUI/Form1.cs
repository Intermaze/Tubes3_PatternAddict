using System;
using System.IO;
using System.Windows.Forms;
using Tubes3;

namespace GUI
{
    public partial class Form1 : Form
    {
        private string lastAccessedDirectory;
        private string selectedFilePath;



        public Form1()
        {
            InitializeComponent();
            lastAccessedDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Database.Initialize();
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = lastAccessedDirectory;
            openFileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;*.gif;*.tiff;*.tif";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = openFileDialog.FileName;
                lastAccessedDirectory = Path.GetDirectoryName(selectedFilePath);
                labelFilePath.Text = "Selected file: " + selectedFilePath;
            }
        }

        private void buttonCompare_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Please select an image file first.");
                return;
            }

            string wantToCompare = Converter.ImageToAsciiStraight(selectedFilePath);
            string algorithm = comboBoxAlgorithm.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(algorithm))
            {
                MessageBox.Show("Please select an algorithm.");
                return;
            }
            if (algorithm == "KMP")
            {
                Database.CompareFingerprintKMP(wantToCompare);
            }
            else if (algorithm == "BM")
            {
                Database.CompareFingerprintBM(wantToCompare);
            }

            MessageBox.Show("Comparison completed.");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DcomboBoxAlgorithm.Items.Add("BM");
            comboBoxAlgorithm.Items.Add("KMP");
        }
    }
}
