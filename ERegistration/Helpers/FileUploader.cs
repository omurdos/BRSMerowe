using Core.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Hosting.Internal;

namespace DashBoard.Helpers
{
    public class FileUploader : IFileUploader
    {
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _webHostingEnvironment;
        private readonly string ImagesRootPath;
        public FileUploader(Microsoft.AspNetCore.Hosting.IHostingEnvironment webHostEnvironment)
        {
            _webHostingEnvironment = webHostEnvironment;
            ImagesRootPath = Path.Combine("D:\\", "Images");
        }

        public async Task<FileUploadResult> UploadImage(IBrowserFile file, string folder)
        {
            try
            {
                if (!Directory.Exists(ImagesRootPath))
                {
                    Directory.CreateDirectory(ImagesRootPath);
                }
                if (!Directory.Exists(Path.Combine(ImagesRootPath, "Profile")))
                {
                    Directory.CreateDirectory(Path.Combine(ImagesRootPath, "Profile"));
                }
                var fileSplit = file.Name.Split(".");
                var fileExtention = fileSplit.LastOrDefault();
                string UniqueFileName = Guid.NewGuid().ToString() + "." + fileExtention;
                string filePath = Path.Combine(ImagesRootPath, folder, UniqueFileName);
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    await file.OpenReadStream().CopyToAsync(fs);
                    await fs.FlushAsync();
                    fs.Close();
                }
                if (File.Exists(filePath))
                {
                    return new FileUploadResult
                    {
                        Succeed = true,
                        Message = "Image uploaded successfully",
                        FileName = UniqueFileName
                    };
                }
                else
                {
                    return new FileUploadResult
                    {
                        Succeed = false,
                        Message = "Failed to upload image"
                    };
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<string> ToBase64(IBrowserFile file)
        {

            using (var ms = new MemoryStream())
            {
                await file.OpenReadStream().CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                string s = Convert.ToBase64String(fileBytes);
                return s;
            }
        }
    }
}
