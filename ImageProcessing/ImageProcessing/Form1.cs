using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HNUDIP;
using WebCamLib;


namespace ImageProcessing
{

    public partial class Form1 : Form
    {
        Bitmap loaded;
        Bitmap processed;

        Bitmap imageB, imageA, colorgreen;
        public Form1()
        {
            InitializeComponent();

             basicCopyToolStripMenuItem1.Enabled = false;
             greyscaleToolStripMenuItem1.Enabled = false;
             colorInversionToolStripMenuItem.Enabled = false;
             histogramToolStripMenuItem.Enabled = false;
             sepiaToolStripMenuItem.Enabled = false;
             saveToolStripMenuItem.Enabled = false;
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                String file_name = openDialog.FileName;
                loaded = (Bitmap)Image.FromFile(file_name);

                pictureBox1.Image = loaded;

                basicCopyToolStripMenuItem1.Enabled = true;
                greyscaleToolStripMenuItem1.Enabled = true;
                colorInversionToolStripMenuItem.Enabled = true;
                histogramToolStripMenuItem.Enabled = true;
                sepiaToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
            }

        }

        private void basicCopyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Color pixel;
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, pixel);
                }
            }
            pictureBox2.Image = processed;
        }

        private void greyscaleToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            Color pixel;
            int greyscale;
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    greyscale = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                    processed.SetPixel(x, y, Color.FromArgb(greyscale, greyscale, greyscale));
                }
            }
            pictureBox2.Image = processed;
        }

        private void colorInversionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            Color pixel;
            for (int x = 0; x < loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x, y, Color.FromArgb(255 - pixel.R, 255 - pixel.B, 255 - pixel.G));
                }
            }
            pictureBox2.Image = processed;
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color pixel;
            int greyscale;
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);
                    greyscale = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                    processed.SetPixel(x, y, Color.FromArgb(greyscale, greyscale, greyscale));
                }

            int[] histogram = new int[256];
            Color value;
            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    value = processed.GetPixel(x, y);
                    histogram[value.R]++;
                }

            Bitmap matrix = new Bitmap(256, 800);
               for (int x = 0; x < 256; x++)
               
                   for (int y = 0; y < 800; y++)
                   {
                    matrix.SetPixel(x, y, Color.White);
                   }

            for (int x = 0; x < 256; x++)

                for (int y = 0; y < Math.Min(histogram[x]/5, 800); y++)
                {
                    matrix.SetPixel(x, 799 - y, Color.Black);
                }
            pictureBox2.Image = matrix;
        }

        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color pixel;
            processed = new Bitmap(loaded.Width, loaded.Height);

            for(int x = 0; x < loaded.Width; x++)
            {
                for(int y = 0; y < loaded.Height; y++)
                {
                    pixel = loaded.GetPixel(x, y);

                    int green = (int)(0.349 * pixel.R + 0.686 * pixel.G + 0.168 * pixel.B);
                    int red = (int)(0.393 * pixel.R + 0.769 * pixel.G + 0.189 * pixel.B);
                    int blue = (int)(0.272 * pixel.R + 0.534 * pixel.G + 0.131 * pixel.B);

                    processed.SetPixel(x, y, Color.FromArgb(Math.Min(255, red), Math.Min(255, green), Math.Min(255, blue)));
                }
            }
            pictureBox2.Image = processed;

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
             if (processed != null)
             {
                 saveFileDialog1.ShowDialog();
             }
             else
             {
                 MessageBox.Show("No image to be save.");
             }

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            imageB = new Bitmap(openFileDialog1.FileName);
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            imageA = new Bitmap(openFileDialog2.FileName);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog2.ShowDialog();
            String file_name = openFileDialog2.FileName;
            loaded = (Bitmap)Image.FromFile(file_name);
            pictureBox2.Image = imageA;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Color mygreen = Color.FromArgb(0, 0, 255);
            int greygreen = (mygreen.R + mygreen.G + mygreen.B) / 3;
            int treshold = 5;
            Bitmap resultImage = new Bitmap(imageB.Width, imageB.Height);

            for (int x = 0; x < imageB.Width; x++)
            {
                for (int y = 0; y < imageB.Height; y++)
                {
                    Color pixel = imageB.GetPixel(x, y);
                    Color backpixel = imageA.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    int subtractvalue = Math.Abs(grey - greygreen);
                    if (subtractvalue > treshold)
                    {
                        resultImage.SetPixel(x, y, pixel);
                    }
                    else
                    {
                        resultImage.SetPixel(x, y, backpixel);
                    }
                }
                pictureBox3.Image = resultImage;
            }
        }

        Device[] devices = DeviceManager.GetAllDevices();
        Device camera = DeviceManager.GetDevice(0);

        private void timer1_Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image image;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();

            int treshold = 100;

            if (data != null)
            {
                image = (Image)(data.GetData("System.Drawing.Bitmap", true));

                // Check if the retrieved data is a valid image
                if (image != null)
                {
                    Bitmap bitmap = new Bitmap(image);

                    ImageProcess2.BitmapFilter.Subtract(bitmap, imageA, Color.Green, treshold);

                    pictureBox4.Image = bitmap;
                }
                else
                {
                    // Handle case where clipboard data is not a valid image
                    Console.WriteLine("Invalid Image Found!.");
                }
            }
            else
            {
                // Handle case where clipboard data is not available
                Console.WriteLine("No Image Found!.");
            }
        }



        private void button4_Click(object sender, EventArgs e)
        {
            IDataObject data;
            Image image;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();
            image = (Image)(data.GetData("System.Drawing.Bitmap", true));

            if (imageA != null && image.Size == imageA.Size)
            {
                timer1.Enabled = true;
            }
            else if (imageA == null)
            {
                Console.WriteLine("No Background Found!");
            }
            else
            {
                Console.WriteLine(imageA.Size);
                Console.WriteLine(image.Size);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            camera.ShowWindow(pictureBox1);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image image;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();

            int treshold = 100;

            if (data != null)
            {
                image = (Image)(data.GetData("System.Drawing.Bitmap", true));

                // Check if the retrieved data is a valid image
                if (image != null)
                {
                    Bitmap bitmap = new Bitmap(image);

                    ImageProcess2.BitmapFilter.GrayScale(bitmap);

                    pictureBox4.Image = bitmap;
                }
                else
                {
                    // Handle case where clipboard data is not a valid image
                    Console.WriteLine("Invalid Image Found!.");
                }
            }
            else
            {
                // Handle case where clipboard data is not available
                Console.WriteLine("No Image Found!.");
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            IDataObject data;
            Image image;
            devices[0].Sendmessage();
            data = Clipboard.GetDataObject();

            int treshold = 100;

            if (data != null)
            {
                image = (Image)(data.GetData("System.Drawing.Bitmap", true));

                // Check if the retrieved data is a valid image
                if (image != null)
                {
                    Bitmap bitmap = new Bitmap(image);

                    ImageProcess2.BitmapFilter.Invert(bitmap);

                    pictureBox4.Image = bitmap;
                }
                else
                {
                    // Handle case where clipboard data is not a valid image
                    Console.WriteLine("Invalid Image Found!.");
                }
            }
            else
            {
                // Handle case where clipboard data is not available
                Console.WriteLine("No Image Found!.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = true;
            timer3.Enabled = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = true;
        }

        

        private void button6_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            processed.Save(saveFileDialog1.FileName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            String file_name = openFileDialog1.FileName;
            loaded = (Bitmap)Image.FromFile(file_name);
            pictureBox1.Image = imageB;
        }
    }
}