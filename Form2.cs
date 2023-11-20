using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB_EXAM
{
    public partial class Form2 : Form
    {
        private object modernFont;

        public Form2()
        {
            InitializeComponent();
            PrivateFontCollection modernFont = new PrivateFontCollection();

            modernFont.AddFontFile("Kanit-Medium.ttf");
          

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
           
            // Show();
           // ShowMainForm();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void GenPDF()
        {
            // Output PDF file path
            string outputPath = "example.pdf";

            // Create a new PDF document
            using (PdfDocument document = new PdfDocument())
            {
                // Add a page to the document
                PdfPage page = document.AddPage();

                // Get an XGraphics object for drawing on the page
                using (XGraphics gfx = XGraphics.FromPdfPage(page))
                {
                    // Set up a font and brush for drawing text
                    XFont font = new XFont("Arial", 12);
                    XBrush brush = XBrushes.Black;

                    // Draw "Hello, this is your PDF document!" on the page
                   
                    gfx.DrawString("Hello, this is your PDF document!", font, brush, 20, 50);

                    // You can add more drawing operations, images, etc., as needed
                }

                // Save the document to the output file
                document.Save(outputPath);
            }


        }

        private void BtnGenPDF_Click(object sender, EventArgs e)
        {
            GenPDF();
        }
    }
}
