using System;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SiteServer.Utils;
using SiteServer.CMS.Core;
using SiteServer.Utils.Enumerations;
using Spire.Pdf;
using Spire.Doc;
using Spire.Doc.Documents;
using FileFormat = Spire.Doc.FileFormat;
using Image = System.Drawing.Image;

namespace SiteServer.BackgroundPages.Cms
{
    public class ModalUploadFile : BasePageCms
    {
        public HtmlInputFile HifUpload;
        public DropDownList DdlIsFileUploadChangeFileName;
        public Literal LtlScript;


        private EUploadType _uploadType;
        private string _realtedPath;
        private string _textBoxClientId;



        public static string GetOpenWindowStringToTextBox(int siteId, EUploadType uploadType, string textBoxClientId)
        {
            return LayerUtils.GetOpenScript("上传附件", PageUtils.GetCmsUrl(siteId, nameof(ModalUploadFile), new NameValueCollection
            {
                {"uploadType", EUploadTypeUtils.GetValue(uploadType)},
                {"TextBoxClientID", textBoxClientId}
            }), 550, 250);
        }

        public static string GetOpenWindowStringToList(int siteId, EUploadType uploadType, string realtedPath)
        {
            return LayerUtils.GetOpenScript("上传附件", PageUtils.GetCmsUrl(siteId, nameof(ModalUploadFile), new NameValueCollection
            {
                {"uploadType", EUploadTypeUtils.GetValue(uploadType)},
                {"realtedPath", realtedPath}
            }), 550, 250);
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsForbidden) return;

            PageUtils.CheckRequestParameter("siteId");
            _uploadType = EUploadTypeUtils.GetEnumType(AuthRequest.GetQueryString("uploadType"));
            _realtedPath = AuthRequest.GetQueryString("realtedPath");
            _textBoxClientId = AuthRequest.GetQueryString("TextBoxClientID");

            if (IsPostBack) return;

            EBooleanUtils.AddListItems(DdlIsFileUploadChangeFileName, "采用系统生成文件名", "采用原有文件名");
            ControlUtils.SelectSingleItemIgnoreCase(DdlIsFileUploadChangeFileName, SiteInfo.Additional.IsFileUploadChangeFileName.ToString());
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (HifUpload.PostedFile == null || "" == HifUpload.PostedFile.FileName) return;


            var filePath = HifUpload.PostedFile.FileName;

            try
            {
                var fileExtName = PathUtils.GetExtension(filePath).ToLower();
                var localDirectoryPath = PathUtility.GetUploadDirectoryPath(SiteInfo, fileExtName);
                if (!string.IsNullOrEmpty(_realtedPath))
                {
                    localDirectoryPath = PathUtility.MapPath(SiteInfo, _realtedPath);
                    DirectoryUtils.CreateDirectoryIfNotExists(localDirectoryPath);
                }
                var localFileName = PathUtility.GetUploadFileName(SiteInfo, filePath, TranslateUtils.ToBool(DdlIsFileUploadChangeFileName.SelectedValue));

                var localFilePath = PathUtils.Combine(localDirectoryPath, localFileName);

                if (_uploadType == EUploadType.Image && !EFileSystemTypeUtils.IsImageOrFlashOrPlayer(fileExtName))
                {
                    FailMessage("此格式不允许上传，此文件夹只允许上传图片以及音视频文件！");
                    return;
                }
                if (_uploadType == EUploadType.Video && !EFileSystemTypeUtils.IsImageOrFlashOrPlayer(fileExtName))
                {
                    FailMessage("此格式不允许上传，此文件夹只允许上传图片以及音视频文件！");
                    return;
                }
                if (_uploadType == EUploadType.File && !PathUtility.IsFileExtenstionAllowed(SiteInfo, fileExtName))
                {
                    FailMessage("此格式不允许上传，请选择有效的文件！");
                    return;
                }

                if (!PathUtility.IsFileSizeAllowed(SiteInfo, HifUpload.PostedFile.ContentLength))
                {
                    FailMessage("上传失败，上传文件超出规定文件大小！");
                    return;
                }

                //如果是pdf 或者 word 转图片保存
                var path = string.Empty;
                if (localFileName.EndsWith(".pdf"))
                {
                    PdfDocument doc = new PdfDocument();
                    doc.LoadFromStream(HifUpload.PostedFile.InputStream);

                    path = localFilePath.Replace(".pdf", "") + $"_{ doc.Pages.Count}";
                    localFilePath = $"{path}.pdf";
                    for (int i = 0; i < doc.Pages.Count; i++)
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        var image = doc.SaveAsImage(i);

                        removeTop20px(image);

                        image.Save(path + $"/{i + 1}.png");
                    }
                }
                else if (localFileName.EndsWith(".docx"))
                {
                    Document doc = new Document();
                    doc.LoadFromStream(HifUpload.PostedFile.InputStream, FileFormat.Docx);

                    path = localFilePath.Replace(".docx", "") + $"_{ doc.PageCount}";
                    localFilePath = $"{path}.docx";
                    for (int i = 0; i < doc.PageCount; i++)
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        var image = doc.SaveToImages(i, ImageType.Bitmap);

                        removeTop20px(image);

                        image.Save(path + $"/{i + 1}.png");
                    }
                }
                else if (localFileName.EndsWith(".doc"))
                {
                    Document doc = new Document();
                    doc.LoadFromStream(HifUpload.PostedFile.InputStream, FileFormat.Doc);

                    path = localFilePath.Replace(".doc", "") + $"_{ doc.PageCount}";
                    localFilePath = $"{path}.doc";
                    for (int i = 0; i < doc.PageCount; i++)
                    {
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        var image = doc.SaveToImages(i, ImageType.Bitmap);

                        removeTop20px(image);

                        image.Save(path + $"/{i + 1}.png");
                    }
                }

                HifUpload.PostedFile.SaveAs(localFilePath);
                FileUtility.AddWaterMark(SiteInfo, localFilePath);

                var fileUrl = PageUtility.GetSiteUrlByPhysicalPath(SiteInfo, localFilePath, true);
                var textBoxUrl = PageUtility.GetVirtualUrl(SiteInfo, fileUrl);

                if (string.IsNullOrEmpty(_textBoxClientId))
                {
                    LayerUtils.Close(Page);
                }
                else
                {
                    LtlScript.Text = $@"
<script type=""text/javascript"" language=""javascript"">
    if (parent.document.getElementById('{_textBoxClientId}') != null)
    {{
        parent.document.getElementById('{_textBoxClientId}').value = '{textBoxUrl}';
    }}
    {LayerUtils.CloseScript}
</script>";
                }
            }
            catch (Exception ex)
            {
                FailMessage(ex, "文件上传失败");
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
