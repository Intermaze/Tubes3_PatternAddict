using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using Eto.Drawing;
using Eto.Forms;
using Tubes3;


namespace GUI
{
    public class MainForm : Form
    {
        ImageView inputImageView;
        ImageView outputImageView;
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

                Console.WriteLine(dialog.FileName);

                if (!string.IsNullOrEmpty(dialog.FileName)) // Check if openFileDialog is canceled
                {
                    selectedImagePath = dialog.FileName;
                    SetImage(inputImageView, selectedImagePath);
                }
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
                if (string.IsNullOrEmpty(selectedImagePath))
                {
                    Console.WriteLine("Please select an image first");
                    return;
                }
                string wantToCompare = Converter.ImageToAsciiStraight(selectedImagePath);
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
                    UpdateBiodata(ans, Path.GetFullPath(path));

                    // // Show labels
                    // nameLabel.Visible = true;
                    // addressLabel.Visible = true;
                    // jobLabel.Visible = true;
                    // dobLabel.Visible = true;
                    // pobLabel.Visible = true;
                    // nationalityLabel.Visible = true;
                    // religionLabel.Visible = true;
                    // pathAns.Visible = true;
                }
            };

            Resizable = false;

            labelAlgorithm = new Label { Text = "Algorithm in use: BM" }; // Initialize labelAlgorithm

            nameLabel = new Label { Text = "Nama: " };
            addressLabel = new Label { Text = "Alamat: "};
            jobLabel = new Label { Text = "Pekerjaan: "};
            dobLabel = new Label { Text = "Tanggal Lahir: "};
            pobLabel = new Label { Text = "Tempat Lahir: "};
            nationalityLabel = new Label { Text = "Kewarganegaraan: "};
            religionLabel = new Label { Text = "Agama: "};
            pathAns = new Label { Text = "Path: "};

            inputImageView = new ImageView {Width = 200, BackgroundColor = Color.FromRgb(0xffff00)};
            outputImageView = new ImageView {Width = 200, BackgroundColor = Color.FromRgb(0xbbff00)};

            var layout = new StackLayout{
                HorizontalContentAlignment = HorizontalAlignment.Center
            };
            var row2 = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                BackgroundColor = Color.FromRgb(0xFFF000)
            };
            var row3 = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.FromArgb(0, 200, 0, 200)
            };

            row2.Items.Add(new StackLayoutItem{
                Control = inputImageView,
                // Expand = true,
            });
            row2.Items.Add(new StackLayoutItem{
                Control = outputImageView,
            });
            row2.Items.Add(new StackLayoutItem(
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
            ));

            row3.Items.Add(new StackLayoutItem(
                new TableLayout{
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
                    }
                }
            ));

            row3.Items.Add(new StackLayoutItem(
                new TableLayout{
                    Rows =
                    {
                        new TableRow(labelAlgorithm),
                        new TableRow(new Label { Text = "Waktu Pencarian: 0s" }),
                        new TableRow(new Label { Text = "Presentase Kecocokan: 0%" })
                    }
                }
            ));

            layout.Items.Add(new StackLayoutItem(
                new Label{
                Text = "Aplikasi C# Tugas Besar 3 Strategi Algoritma 2023/2024",
                TextAlignment = TextAlignment.Center,
                BackgroundColor = Color.FromArgb(31, 107, 196, 80)
            }));
            layout.Items.Add(new StackLayoutItem{Control = row2, Expand = true});
            layout.Items.Add(new StackLayoutItem{Control = row3});
            
            Content = layout;
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
            SetImage(outputImageView, filePath);
        }

        public void SetImage(ImageView imageView, string path)
        {
            if (imageView.Image != null && !inputImageView.Image.IsDisposed)
            {
                imageView.Image.Dispose(); 
            }

            imageView.Image = new Bitmap(path);
        }
    }
}