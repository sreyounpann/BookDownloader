using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookDownloader
{
    public partial class Form1 : Form
    {
        private Dictionary<string, string> bookUrlsl;
        private bool bookSelected = false;
        private string selectedBookTitle = ""; 
        public Form1()
        {
            InitializeComponent();
         

            bookUrlsl = new Dictionary<string, string>
            {
                { "LINCOLN LETTERS" , "ftp://gutenberg.pglaf.org/8/1/1/8110/8110.txt"  },
                { "NARRATIVE OF THE LIFE" , "ftp://gutenberg.pglaf.org/2/23/23-0.txt" },
                { "THE SCARLET LETTER" , "ftp://gutenberg.pglaf.org/3/33/33-0.txt"},
                { "MISS MEREDITH" , "ftp://gutenberg.pglaf.org/5/9/9/9/59990/59990.txt" },

              
            };

            pictureBox1.Tag = "LINCOLN LETTERS";
            pictureBox2.Tag = "NARRATIVE OF THE LIFE";
            pictureBox3.Tag = "THE SCARLET LETTER";
            pictureBox4.Tag = "MISS MEREDITH"; 
        }

  
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string selectedBookTitle = pictureBox1.Tag.ToString();
            string bookText = Get_Data(selectedBookTitle); 
            richTextBox1.Text = bookText;

            linkLabel1.Text = selectedBookTitle;

            //pop up the selected book cover in picturebox6
            DisplayBookCover(selectedBookTitle); 

            bookSelected = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

            string selectedBookTitle = pictureBox2.Tag.ToString();
            string bookText = Get_Data(selectedBookTitle);
            richTextBox1.Text = bookText;

            linkLabel2.Text = selectedBookTitle;

          
            DisplayBookCover(selectedBookTitle);
            bookSelected = true;
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string selectedBookTitle = pictureBox3.Tag.ToString();
            string bookText = Get_Data(selectedBookTitle);
            richTextBox1.Text = bookText;

            linkLabel3.Text = selectedBookTitle;

          
            DisplayBookCover(selectedBookTitle);

            bookSelected = true;
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string selectedBookTitle = pictureBox4.Tag.ToString();
            string bookText = Get_Data(selectedBookTitle);
            richTextBox1.Text = bookText;

            linkLabel4.Text = selectedBookTitle;

          
            DisplayBookCover(selectedBookTitle);

            bookSelected = true;
        }


        private string Get_Data(string bookTitle)
        {
            string result = string.Empty;
           

            if (bookUrlsl.ContainsKey(bookTitle))
            {
                string bookUrl = bookUrlsl[bookTitle];

                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(bookUrl);
                    request.Method = WebRequestMethods.Ftp.DownloadFile;

                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    using (Stream responseStream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        result = reader.ReadToEnd();
                    }


                    // Save file locally (optional)
                    using (StreamWriter file = File.CreateText($"{bookTitle}.txt"))
                    {
                        file.WriteLine(result);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error downloading book: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Book URL not found.");
            }

            return result;
        }

        private void DisplayBookCover(string bookTitle)
        {
            switch (bookTitle)
            {
                case "LINCOLN LETTERS":
                    pictureBox6.Image = Properties.Resources.Book1; 
                    break;
                case "NARRATIVE OF THE LIFE":
                    pictureBox6.Image = Properties.Resources.Book2; 
                    break;
                case "THE SCARLET LETTER": 
                    pictureBox6.Image= Properties.Resources.Book3;
                    break;
                case "MISS MEREDITH":
                    pictureBox6.Image = Properties.Resources.Book4; 
                    break;
                default:
                    pictureBox6.Image = null;
                    break;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string selectedBookTitle = linkLabel1.Text;

            string bookText = Get_Data(selectedBookTitle); 
            richTextBox1.Text = bookText;

            linkLabel1.Text = selectedBookTitle;

            DisplayBookCover (selectedBookTitle);

            bookSelected = true; 
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string selectedBookTitle = linkLabel2.Text; 

            string bookText = Get_Data(selectedBookTitle);

            richTextBox1.Text = bookText;

            linkLabel2.Text = selectedBookTitle;

            DisplayBookCover(selectedBookTitle);

            bookSelected = true;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string selectedBookTitle = linkLabel3.Text;

            string bookText = Get_Data(selectedBookTitle);

            richTextBox1.Text = bookText;

            linkLabel3.Text = selectedBookTitle;

            DisplayBookCover(selectedBookTitle);

            bookSelected = true;
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string selectedBookTitle = linkLabel4.Text;

            string bookText = Get_Data(selectedBookTitle);

            richTextBox1.Text = bookText;

            linkLabel4.Text = selectedBookTitle;

            DisplayBookCover(selectedBookTitle);

            bookSelected = true;
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!bookSelected)
            {
               
                MessageBox.Show("Please select a book before downloading.");
                return;
            }

            foreach (string bookTitle in bookUrlsl.Keys)
            {
                string bookText = Get_Data(bookTitle);
               // richTextBox1.Text = bookText;

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt | *.txt | PDF File (*.pdf) | *.pdf",
                    Title = "Save Book Text", 
                    FileName = $"{bookTitle}.txt"     
                };

                DialogResult dialogResult = saveFileDialog.ShowDialog();

                // If the user cancels, exit the loop and return from the button1_Click event
                if (dialogResult != DialogResult.OK)
                {

                   return; 
                   
                }

                string fileName = saveFileDialog.FileName;
                if (saveFileDialog.FilterIndex == 1)
                {
                    SaveTextFile(fileName, bookText);
                }
                else if (saveFileDialog.FilterIndex == 2)
                {
                    SavePdfFile(fileName, bookText);
                }

                richTextBox1.Text = bookText;
            }
        }

        private void SaveTextFile(string fileName , string bookText)
        {
            try
            {
                using (StreamWriter file = File.CreateText(fileName))
                {
                    file.Write(bookText);
                }
                MessageBox.Show("Book text saved as a TXT file successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving book text as a TXT file: " + ex.Message);
            }
        }

        private void SavePdfFile(string fileName, string bookText)
        {
            //I am using iTextSharp library (to do this i installed iTextSharp in NuGet package)
            try
            {
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document();
                using (FileStream fs = new FileStream(fileName, FileMode.Create))
                {
                    iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, fs);
                    pdfDoc.Open();
                    pdfDoc.Add(new iTextSharp.text.Paragraph(bookText));
                    pdfDoc.Close();
                }
                MessageBox.Show("Book text saved as a PDF file successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving book text as a PDF file: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
