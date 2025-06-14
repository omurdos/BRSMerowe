using Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class ImageUploadService
    {

        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        private readonly string ImagesRootPath;
        public ImageUploadService(Microsoft.AspNetCore.Hosting.IHostingEnvironment webHostEnvironment  )
        {
            _hostingEnvironment = webHostEnvironment;
            bool _isProduction = true;
            if (_isProduction)
            {
                ImagesRootPath = Path.Combine("/var/www/images");

            }
            else
            {
                ImagesRootPath = Path.Combine("D:\\", "Images");

            }

        }
        public async Task<FileUploadResult> Upload(string imageData, string folder)
        {
            try
            {
                if (!Directory.Exists(ImagesRootPath))
                {
                    Directory.CreateDirectory(ImagesRootPath);

                }

                if (!Directory.Exists(Path.Combine(ImagesRootPath, folder)))
                {
                    Directory.CreateDirectory(Path.Combine(ImagesRootPath, folder));
                }

                byte[] bytes = Convert.FromBase64String(imageData);
                string UniqueFileName = Guid.NewGuid().ToString();
                string uploadFolder = Path.Combine(ImagesRootPath, folder);
                string filePath = Path.Combine(uploadFolder, UniqueFileName + ".png");
                await File.WriteAllBytesAsync(filePath, bytes);
                if (File.Exists(filePath))
                {
                    return new FileUploadResult
                    {
                        Succeed = true,
                        Message = "Image uploaded successfully",
                        FileName = UniqueFileName + ".png"
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
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<byte[]> ConvertToBytes(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }


    }
}
