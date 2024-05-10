using System;
using Eto.Drawing;
using Eto.Forms;

delegate void Runnable();

namespace Tubes3_PatternAddict
{
	public class MainForm : Form
	{
		public MainForm()
		{
			Resizable = false;
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
						new ImageView { Height = 300 },
						new ImageView { Height = 300 },
						new ImageView { Height = 300 }
					),
					new TableRow(
						new Button
						{
							Text = "Pilih Citra",
							Command = new Command(chooseFileDialog)
						},
						new Button { Text = "BM/KMP" },
						new Button { Text = "Search" },
						new TableLayout
						{
							Rows =
							{
								new TableRow(new Label { Text = "Waktu Pencarian: 0s" }),
								new TableRow(new Label { Text = "Presentase Kecocokan: 0%" })
							},
						}
					)
				}
			};
		}

		EventHandler<EventArgs> chooseFileDialog = (object o, EventArgs e) =>
		{
			Console.WriteLine("File dialog clicked!");
		};
	}
}
