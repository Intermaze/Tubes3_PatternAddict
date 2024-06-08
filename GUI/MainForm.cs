using System;
using System.Collections;
using System.Diagnostics;
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

        Label percentageLabel;
        bool isBM = true;
        Biodata ans;
        string path;

        Label timeLabel;

        int time;
        float percentage;

        Color[] colors = {
            Color.FromRgb(0x577a76),
            Color.FromRgb(0x8eadc5),
            Color.FromRgb(0x8de1bd),
            Color.FromRgb(0x9bdb87),
            Color.FromRgb(0x7fba67)
        };

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
                Stopwatch watch = new Stopwatch();
                watch.Start();
                if (isBM)
                {
                    Console.WriteLine("Searching using BM algorithm");
                    Console.WriteLine(wantToCompare);
                    (ans, path, percentage) = await Task.Run(() => Database.CompareFingerprintBM(wantToCompare));
                    Console.WriteLine(percentage);
                }
                else
                {
                    Console.WriteLine("Searching using KMP algorithm");
                    (ans, path, percentage) = await Task.Run(() => Database.CompareFingerprintKMP(wantToCompare));
                }
                watch.Stop();
                time = (int)watch.Elapsed.TotalMilliseconds;
                
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

            labelAlgorithm = CreateLabel("Algorithm in use: BM"); // Initialize labelAlgorithm

            var biodataFont = new Font(SystemFont.Default, 10);

            nameLabel = CreateLabel("Nama: ", biodataFont);
            addressLabel = CreateLabel("Alamat: ", biodataFont);
            jobLabel = CreateLabel("Pekerjaan: ", biodataFont);
            dobLabel = CreateLabel("Tanggal Lahir: ", biodataFont);
            pobLabel = CreateLabel("Tempat Lahir: ", biodataFont);
            nationalityLabel = CreateLabel("Kewarganegaraan: ", biodataFont);
            religionLabel = CreateLabel("Agama: ", biodataFont);
            pathAns = CreateLabel("Path: ", biodataFont);
            timeLabel = CreateLabel("Waktu Eksekusi:");
            percentageLabel = CreateLabel("Persentase Kecocokan:");

            inputImageView = new ImageView 
            {
                Width = 200, 
                Image = new Bitmap(Path.GetFullPath("../Assets/default.BMP"))
            };
            outputImageView = new ImageView 
            {
                Width = 200, 
                Image = new Bitmap(Path.GetFullPath("../Assets/default.BMP")), 
                BackgroundColor = colors[4]
            };

            var row2 = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                BackgroundColor = colors[3],
                Items = {
                    inputImageView,
                    outputImageView,
                    new StackLayout {
                            Orientation = Orientation.Vertical,
                            Items =
                            {
                                nameLabel,
                                addressLabel,
                                jobLabel,
                                dobLabel,
                                pobLabel,
                                nationalityLabel,
                                religionLabel,
                                pathAns
                            },
                            Width = 300,
                            Padding = new Padding(8)
                    }
                }
            };
            var row3 = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Center,
                BackgroundColor = colors[1],
                Padding = new Padding(6),
                Width = 800
            };

            row3.Items.Add(new StackLayoutItem(
                new TableLayout{
                    Padding = new Padding(80, 0, 0, 0),
                    Rows =
                    {
                        new TableRow(
                            new Button
                            {
                                Text = "Pilih Citra",
                                Command = new Command(chooseFileDialog),
                                Width = 100,
                                BackgroundColor = colors[0],
                                Font = new Font(SystemFont.Bold, 11), 
                                TextColor = colors[2]
                            },
                            new Button { 
                                Text = "BM/KMP", 
                                Command = new Command(processAlgorithm), 
                                Width = 100,
                                BackgroundColor = colors[0],
                                Font = new Font(SystemFont.Bold, 11),
                                TextColor = colors[2]
                            },
                            new Button { 
                                Text = "Search", 
                                Command = new Command(search), 
                                Width = 100 ,
                                BackgroundColor = colors[0],
                                Font = new Font(SystemFont.Bold, 11),
                                TextColor = colors[2]
                            }
                        ),
                    },
                }
            ));

            row3.Items.Add(new StackLayout(
                new TableLayout{
                    Padding = new Padding(160, 0, 0, 0),
                    Rows =
                    {
                        new TableRow(labelAlgorithm),
                        new TableRow(timeLabel),
                        new TableRow(percentageLabel)
                    }
                }
            ));

            Label Title = CreateLabel("Aplikasi C# Tugas Besar 3 Strategi Algoritma 2023/2024", new Font(SystemFont.Bold, 14)); 

            var layout = new StackLayout{
                HorizontalContentAlignment = HorizontalAlignment.Center,
                BackgroundColor = colors[0],
                Items = {
                    new StackLayout{
                        Items = {Title},
                        Padding = new Padding(10),
                        BackgroundColor = colors[2],
                        Width = 800,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    },
                    new StackLayoutItem{
                        Control = row2,
                        Expand = true,
                    },
                    row3
                }
            };

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
                timeLabel.Text = $"Waktu Eksekusi: {time} ms";
                percentageLabel.Text = $"Persentase Kecocokan: {percentage}%";
                Console.WriteLine("Fuck you: " + percentage);
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

        private Label CreateLabel(string labelText, Font font = null)
        {
            return new Label { Text = labelText, Wrap = WrapMode.Word, Font = font ?? SystemFonts.Default(), TextColor = Color.FromRgb(0x000000)};
        }

    }
}