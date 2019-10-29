using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Pdf;

using FileFormat = Spire.Doc.FileFormat;
using Image = System.Drawing.Image;

namespace ToPng
{
    class Program
    {

        static void Main(string[] args)
        {
            var path = ConfigurationSettings.AppSettings["filepath"];

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            Console.WriteLine($"1.开始转换门户网站:{path}");
            convertToPng(directoryInfo);

            var path2 = ConfigurationSettings.AppSettings["newsSiteFilepath"];
            DirectoryInfo directoryInfo2 = new DirectoryInfo(path2);
            Console.WriteLine($"2.开始转换新闻网站:{path2}");
            convertToPng(directoryInfo2);

            Console.WriteLine($"全部完成!");
            Console.ReadKey();

        }

        public static void convertToPng(DirectoryInfo directoryInfo)
        {
            foreach (var directory in directoryInfo.GetDirectories())
            {
                convertToPng(directory);
            }

            foreach (var fileInfo in directoryInfo.GetFiles())
            {
                var localFileName = fileInfo.Name;
                var localFilePath = fileInfo.DirectoryName + "/" + fileInfo.Name;

                //如果是pdf 或者 word 转图片保存
                var path = string.Empty;
                if (localFileName.EndsWith(".pdf"))
                {
                    PdfDocument doc = new PdfDocument();
                    doc.LoadFromStream(fileInfo.OpenRead());

                    path = localFilePath.Replace(".pdf", "");
                    for (int i = 0; i < doc.Pages.Count; i++)
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        var image = doc.SaveAsImage(i);

                        removeTop20px(image);

                        image.Save(path + $"/{i + 1}.png");

                        Console.WriteLine(path + $"/{i + 1}.png"+"已转换");
                    }
                }
                else if (localFileName.EndsWith(".docx"))
                {
                    Document doc = new Document();
                    doc.AddSection();
                    doc.Sections[0].AddParagraph();
                    doc.InsertTextFromStream(fileInfo.OpenRead(), FileFormat.Docx);

                    doc.Sections[0].Paragraphs[0].AppendBreak(BreakType.PageBreak);

                    path = localFilePath.Replace(".docx", "");
                    for (int i = 1; i < doc.PageCount; i++)
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        var image = doc.SaveToImages(i, ImageType.Bitmap);

                        removeTop20px(image);

                        image.Save(path + $"/{i }.png");
                        Console.WriteLine(path + $"/{i }.png" + "已转换");
                    }
                }
                else if (localFileName.EndsWith(".doc"))
                {
                    Document doc = new Document();
                    doc.AddSection();
                    doc.Sections[0].AddParagraph();
                    doc.InsertTextFromStream(fileInfo.OpenRead(), FileFormat.Doc);

                    doc.Sections[0].Paragraphs[0].AppendBreak(BreakType.PageBreak);

                    path = localFilePath.Replace(".doc", "");
                    for (int i = 1; i < doc.PageCount; i++)
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        var image = doc.SaveToImages(i, ImageType.Bitmap);

                        removeTop20px(image);

                        image.Save(path + $"/{i}.png");
                        Console.WriteLine(path + $"/{i }.png" + "已转换");
                    }
                }
            }
        }

        private static void removeTop20px(Image image)
        {
            //定义截取矩形
            System.Drawing.Rectangle cropArea = new System.Drawing.Rectangle(0, 20, image.Width, image.Height - 20); //要截取的区域大小
            var pickedG = System.Drawing.Graphics.FromImage(image);

            //设置质量
            pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            Rectangle fromR = new Rectangle(0, 20, image.Width, image.Height - 20); //原图裁剪定位
            Rectangle toR = new Rectangle(0, 0, image.Width, image.Height); //目标定位

            //裁剪
            pickedG.DrawImage(image, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
        }


    }
}
