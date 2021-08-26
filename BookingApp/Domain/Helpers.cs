using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using SelectPdf;

namespace Domain
{
    public static class Helpers
    {
        public static FileResult HtmlToPdf(string html, string documentName)
        {

            //var renderer = new HtmlToPdf();
            //// add a header to every page easily
            //renderer.PrintOptions.FirstPageNumber = 1;
            //renderer.PrintOptions.Header.DrawDividerLine = true;
            //renderer.PrintOptions.Header.CenterText = "{url}";
            //renderer.PrintOptions.Header.FontFamily = "Arial,Sakkal Majalla";
            //renderer.PrintOptions.Header.FontSize = 12;
            //// add a footer too
            //renderer.PrintOptions.Footer.DrawDividerLine = true;
            //renderer.PrintOptions.Footer.FontFamily = "Arial,Sakkal Majalla";
            //renderer.PrintOptions.Footer.FontSize = 10;
            //renderer.PrintOptions.Footer.LeftText = "{date} {time}";
            //renderer.PrintOptions.Footer.RightText = "{page} of {total-pages}";

            //renderer.PrintOptions.InputEncoding = System.Text.Encoding.UTF8;
            
            //var pdfDocument = renderer.RenderHtmlAsPdf(html);
            //pdfDocument.Print();


            ////var OutputPath = "HtmlToPDF.pdf";
            ////pdfDocument.SaveAs(OutputPath).Print();


            // read parameters from the webpage
            string baseUrl = "";

            string pdf_page_size = "A4";
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize),
                pdf_page_size, true);

            string pdf_orientation = "Portrait";
            PdfPageOrientation pdfOrientation =
                (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);

            int webPageWidth = 1024;
            try
            {
                webPageWidth = Convert.ToInt32(1024);
            }
            catch { }

            int webPageHeight = 0;
            try
            {
                webPageHeight = Convert.ToInt32(0);
            }
            catch { }

            // instantiate a html to pdf converter object
            SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;
            converter.Options.MarginTop = 10;
            converter.Options.MarginBottom = 10;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            converter.Options.EmbedFonts = true;

            // create a new pdf document converting an url
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(html);

            doc.AddFont(PdfStandardFont.Helvetica);
            // save pdf document
            byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();

            // return resulted pdf document
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = documentName + ".pdf";

            return fileResult;
        }
    }
}
