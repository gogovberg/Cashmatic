using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hgi.Environment;
using PdfiumViewer;

namespace CashmaticApp
{
    public static class PrinterCommands
    {
        public static void PrintPDF( string printer, string paperName, string filename, int copies)
        {
            try
            {
                Debug.Log("CashmaticApp", "PrintPDF");
                // Create the printer settings for our printer
                var printerSettings = new PrinterSettings
                {
                    PrinterName = printer,
                    Copies = (short)copies,
                };

                // Create our page settings for the paper size selected
                var pageSettings = new PageSettings(printerSettings);
                foreach (PaperSize paperSize in printerSettings.PaperSizes)
                {
                    if (paperSize.PaperName == paperName)
                    {
                        pageSettings.PaperSize = paperSize;
                        break;
                    }
                }

                // Now print the PDF document
                using (var document = PdfDocument.Load(filename))
                {
                    using (var printDocument = document.CreatePrintDocument())
                    {
                        printDocument.PrinterSettings = printerSettings;
                        printDocument.DefaultPageSettings = pageSettings;
                        printDocument.PrintController = new StandardPrintController();
                        printDocument.Print();
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
        }

        public static string PrintPDFResize(string fileNamePath)
        {
            Debug.Log("CashmaticApp", "PrintPDFResize");
            string fileDirectory = Path.GetDirectoryName(fileNamePath);
            string fileName = Path.GetFileNameWithoutExtension(fileNamePath);
            string newFile = fileDirectory + "\\" + fileName + "_print.pdf";

            

            try
            {
                // open the reader
                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(fileNamePath);

                iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
                //Document document = new Document(size);
                iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(size.Width + 15, size.Height + Global.CardPaymentPrintPageSizeAddHeight));

                // open the writer
                FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, fs);
                document.Open();

                // the pdf content
                iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;

                if (Global.cardholderReceipt != "")
                {
                    // select the font properties
                    iTextSharp.text.pdf.BaseFont bf = iTextSharp.text.pdf.BaseFont.CreateFont(iTextSharp.text.pdf.BaseFont.COURIER_BOLD, iTextSharp.text.pdf.BaseFont.CP1252, iTextSharp.text.pdf.BaseFont.EMBEDDED);
                    cb.SetColorFill(iTextSharp.text.BaseColor.BLACK);
                    cb.SetFontAndSize(bf, 8);

                    int StartAt = Global.CardPaymentPrintStartAt + Global.CardPaymentPrintPageSizeAddHeight;

                    foreach (string line in Global.cardholderReceipt.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
                    {
                        // write the text in the pdf content
                        cb.BeginText();
                        // put the alignment and coordinates here
                        cb.ShowTextAligned(0, line, Global.CardPaymentPrintLeft, StartAt, 0);
                        cb.EndText();
                        StartAt = StartAt - Global.CardPaymentPrintLineHeight;
                    }
                }

                //// create the new page and add it to the pdf
                iTextSharp.text.pdf.PdfImportedPage page = writer.GetImportedPage(reader, 1);
                cb.AddTemplate(page, 10, +Global.CardPaymentPrintPageSizeAddHeight);

                // close the streams and voilá the file should be changed :)
                document.Close();
                fs.Close();
                writer.Close();
                reader.Close();
            }
            catch(Exception ex)
            {
                Debug.Log("CashmaticApp", ex.ToString());
            }
          
            return newFile;
        }
    }
}
