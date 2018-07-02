using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfiumViewer;

namespace CashmaticApp
{
    public static class PrinterCommands
    {
        public static bool PrintPDF( string printer, string paperName, string filename, int copies)
        {
            try
            {
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
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string PrintPDFResize(string fileNamePath)
        {

            string fileDirectory = Path.GetDirectoryName(fileNamePath);
            string fileName = Path.GetFileNameWithoutExtension(fileNamePath);
            string newFile = fileDirectory + fileName + "_print.pdf";
            // open the reader
            iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(fileNamePath);

            iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(1);
            //Document document = new Document(size);
            iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(size.Width + 15, size.Height));

            // open the writer
            FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, fs);
            document.Open();

            // the pdf content
            iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;

            //// create the new page and add it to the pdf
            iTextSharp.text.pdf.PdfImportedPage page = writer.GetImportedPage(reader, 1);
            cb.AddTemplate(page, 10, 0);

            // close the streams and voilá the file should be changed :)
            document.Close();
            fs.Close();
            writer.Close();
            reader.Close();

            return newFile;
        }
    }
}
