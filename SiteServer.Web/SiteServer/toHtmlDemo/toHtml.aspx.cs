using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Spire.Doc;
using Spire.Pdf;
using FileFormat = Spire.Doc.FileFormat;

namespace SiteServer.API.SiteServer.toHtmlDemo
{
    public partial class toHtml : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void FileUpload1_DataBinding(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (this.FileUpload1.HasFile)
            {

                MemoryStream stream =new MemoryStream();

                if (FileUpload1.FileName.EndsWith("pdf"))
                {
                    PdfDocument doc = new PdfDocument();
                    doc.LoadFromStream(FileUpload1.FileContent);


                    //var path = Server.MapPath("~/");
                    //doc.SaveToFile(path+"11111111111.html", Spire.Pdf.FileFormat.HTML);
                    for (int i = 0; i < doc.Pages.Count; i++)
                    {
                        var path = Server.MapPath("~/");
                        var image = doc.SaveAsImage(i);
                        image.Save(path + $"{i+1}.png");
                    }
                }
                else
                {
                    Document document = new Document();
                    document.LoadFromStream(FileUpload1.FileContent, FileFormat.Docx);
                    document.SaveToStream(stream, FileFormat.Html);
                }

                var bs = stream.ToArray();
                this.htmlContent.InnerHtml = Encoding.UTF8.GetString(bs);
                stream.Close();

            }
        }
    }
}