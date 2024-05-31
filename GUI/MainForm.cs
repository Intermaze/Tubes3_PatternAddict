using System;
using System.IO;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using Tubes3;


namespace GUI
{
    public class MainForm : Form
    {
        ImageView imageView;
        Label nameLabel;
        Label addressLabel;
        Label jobLabel;
        Label dobLabel;
        Label pobLabel;
        Label nationalityLabel;
        Label religionLabel;
        Label pathAns;
        string selectedImagePath;
        Label labelAlgorithm;
        bool isBM = true;
        Biodata ans;
        string path;

        public MainForm()
        {
            Database.Initialize();
            Size = new Size(800, 500);
            EventHandler<EventArgs> chooseFileDialog = (object o, EventArgs e) =>
            {
                String[] extensions = new String[] { "png", "jpg", "bmp" };
                var filter = new FileFilter("image", extensions);

                var dialog = new OpenFileDialog();
                dialog.Filters.Add(filter);
                dialog.CurrentFilterIndex = 0;
                dialog.ShowDialog(this);

                selectedImagePath = dialog.FileName;
                Console.WriteLine(selectedImagePath);

                if (imageView.Image != null)
                {
                    imageView.Image.Dispose(); // Dispose the previous image to release resources
                }

                imageView.Image = new Bitmap(selectedImagePath);
            };

            EventHandler<EventArgs> processAlgorithm = (object sender, EventArgs e) =>
            {
                if (isBM)
                {
                    Console.WriteLine("KMP process");
                    UpdateAlgorithmState("KMP");
                }
                else
                {
                    Console.WriteLine("BM process");
                    UpdateAlgorithmState("BM");
                }

                // Update toggle state
                isBM = !isBM;
            };

            EventHandler<EventArgs> search = async (object sender, EventArgs e) =>
            {
                if (selectedImagePath == null)
                {
                    Console.WriteLine("Please select an image first");
                    return;
                }
                string fileNameWithExtension = Path.GetFileName(selectedImagePath);
                string basepath = "..\\Data";
                string fullPath = Path.Combine(basepath, fileNameWithExtension);
                string wantToCompare = Converter.ImageToAsciiStraight(fullPath);
                Console.WriteLine(wantToCompare);
                if (isBM)
                {
                    Console.WriteLine("Searching using BM algorithm");
                    Console.WriteLine(wantToCompare);
                    (ans, path) = await Task.Run(() => Database.CompareFingerprintBM(wantToCompare));
                }
                else
                {
                    Console.WriteLine("Searching using KMP algorithm");
                    (ans, path) = await Task.Run(() => Database.CompareFingerprintKMP(wantToCompare));
                }
                
                // Update UI with the answer
                if (ans != null)
                {
                    // Update labels with Biodata information
                    nameLabel.Text = $"Nama: {ans.nama}";
                    addressLabel.Text = $"Alamat: {ans.alamat}";
                    jobLabel.Text = $"Pekerjaan: {ans.pekerjaan}";
                    dobLabel.Text = $"Tanggal Lahir: {ans.tanggal_lahir}";
                    pobLabel.Text = $"Tempat Lahir: {ans.tempat_lahir}";
                    nationalityLabel.Text = $"Kewarganegaraan: {ans.kewarganegaraan}";
                    religionLabel.Text = $"Agama: {ans.agama}";
                    pathAns.Text = $"Path: {path}";

                    // Set the image
                    SetImage(selectedImagePath);
                    
                    // Show labels
                    nameLabel.Visible = true;
                    addressLabel.Visible = true;
                    jobLabel.Visible = true;
                    dobLabel.Visible = true;
                    pobLabel.Visible = true;
                    nationalityLabel.Visible = true;
                    religionLabel.Visible = true;
                    pathAns.Visible = true;
                }
            };

            Resizable = false;

            labelAlgorithm = new Label { Text = "Algorithm in use: BM" }; // Initialize labelAlgorithm

            nameLabel = new Label { Text = "Nama: ", Visible = false };
            addressLabel = new Label { Text = "Alamat: ", Visible = false };
            jobLabel = new Label { Text = "Pekerjaan: ", Visible = false };
            dobLabel = new Label { Text = "Tanggal Lahir: ", Visible = false };
            pobLabel = new Label { Text = "Tempat Lahir: ", Visible = false };
            nationalityLabel = new Label { Text = "Kewarganegaraan: ", Visible = false };
            religionLabel = new Label { Text = "Agama: ", Visible = false };
            pathAns = new Label { Text = "Path: ", Visible = false };

            imageView = new ImageView { Height = 300 };


            Content = new TableLayout
            {
                Spacing = new Size(5, 5),
                Padding = new Padding(10, 10, 10, 10),
                Rows =
                {
                    new TableRow(
                        new Label
                        {
                            Text = "Aplikasi C# Tugas Besar 3 Strategi Algoritma 2023/2024",
                            TextAlignment = TextAlignment.Center
                        }
                    ),
                    new TableRow(
                        imageView, // Single ImageView
                        new TableLayout 
                        {
                            Rows =
                            {
                                new TableRow(nameLabel),
                                new TableRow(addressLabel),
                                new TableRow(jobLabel),
                                new TableRow(dobLabel),
                                new TableRow(pobLabel),
                                new TableRow(nationalityLabel),
                                new TableRow(religionLabel),
                                new TableRow(pathAns)
                            }
                        }
                    ),
                    new TableRow(
                        new TableLayout
                        {
                            Rows =
                            {
                                new TableRow(
                                    new Button
                                    {
                                        Text = "Pilih Citra",
                                        Command = new Command(chooseFileDialog),
                                        Width = -1
                                    },
                                    new Button { Text = "BM/KMP", Command = new Command(processAlgorithm), Width = -1 },
                                    new Button { Text = "Search", Command = new Command(search), Width = -1 }
                                ),
                                new TableRow(
                                    new TableLayout
                                    {
                                        Rows =
                                        {
                                            new TableRow(labelAlgorithm),
                                            new TableRow(new Label { Text = "Waktu Pencarian: 0s" }),
                                            new TableRow(new Label { Text = "Presentase Kecocokan: 0%" })
                                        }
                                    }
                                )
                            }
                        }
                    )
                }
            };
        }
        void UpdateAlgorithmState(string algorithm)
        {
            labelAlgorithm.Text = $"Algorithm in use: {algorithm}";
        }

        public void UpdateBiodata(Biodata biodata, string filePath)
        {
            if (biodata != null)
            {
                nameLabel.Text = $"Nama: {biodata.nama}";
                addressLabel.Text = $"Alamat: {biodata.alamat}";
                jobLabel.Text = $"Pekerjaan: {biodata.pekerjaan}";
                dobLabel.Text = $"Tanggal Lahir: {biodata.tanggal_lahir}";
                pobLabel.Text = $"Tempat Lahir: {biodata.tempat_lahir}";
                nationalityLabel.Text = $"Kewarganegaraan: {biodata.kewarganegaraan}";
                religionLabel.Text = $"Agama: {biodata.agama}";
                pathAns.Text = $"Path: {path}";
            }
            SetImage(filePath);
        }

        public void SetImage(string path)
        {
            if (imageView.Image != null)
            {
                imageView.Image.Dispose(); 
            }

            imageView.Image = new Bitmap(path);
        }
    }
}