using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeyRed.Mime;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Shared.Services
{

    public enum AttachmentType
    {
        PERSONAL_PICTURE,
        IDENTITY,
        MEDICAL_REPORT,
        RECIEPT
    }
    public class FileUploadService
    {
        private IWebHostEnvironment _hostingEnvironment;
        private readonly string AttachmentsRootPath;
        private readonly ILogger<FileUploadService> _logger;
        private readonly IConfiguration _configuration;
        public FileUploadService(IWebHostEnvironment webHostEnvironment, ILogger<FileUploadService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _hostingEnvironment = webHostEnvironment;
            //AttachmentsRootPath = Path.Combine(_hostingEnvironment.ContentRootPath, "Attachments");
            _configuration = configuration;

            AttachmentsRootPath = _configuration.GetValue<string>("Attachments:Path") ?? "";
            if (!string.IsNullOrEmpty(AttachmentsRootPath))
            {
                if (Directory.Exists(AttachmentsRootPath))
                {
                    _logger.LogInformation("Attachments root path exists: {AttachmentsRootPath}", AttachmentsRootPath);
                }
                else
                {
                    _logger.LogInformation("Attachments root path does not exist, creating: {AttachmentsRootPath}", AttachmentsRootPath);
                    Directory.CreateDirectory(AttachmentsRootPath);

                }
            }


        }

        public async Task<FileUploadResult> Upload(string fileData, AttachmentType attachmentType)
        {
            try
            {
                _logger.LogInformation("Uploading file of type: {AttachmentType}", attachmentType.ToString());
                if (!Directory.Exists(AttachmentsRootPath))
                {
                    _logger.LogInformation("Creating Attachments root directory: {AttachmentsRootPath}", AttachmentsRootPath);
                    Directory.CreateDirectory(AttachmentsRootPath);

                }

                if (!Directory.Exists(Path.Combine(AttachmentsRootPath, "PersonalPictures")))

                {
                    _logger.LogInformation("Creating PersonalPictures directory: {PersonalPicturesPath}", Path.Combine(AttachmentsRootPath, "PersonalPictures"));
                    Directory.CreateDirectory(Path.Combine(AttachmentsRootPath, "PersonalPictures"));
                }

                if (!Directory.Exists(Path.Combine(AttachmentsRootPath, "IdentityProofs")))
                {
                    _logger.LogInformation("Creating IdentityProofs directory: {IdentityProofsPath}", Path.Combine(AttachmentsRootPath, "IdentityProofs"));
                    Directory.CreateDirectory(Path.Combine(AttachmentsRootPath, "IdentityProofs"));
                }

                if (!Directory.Exists(Path.Combine(AttachmentsRootPath, "MedicalReports")))
                {
                    _logger.LogInformation("Creating MedicalReports directory: {MedicalReportsPath}", Path.Combine(AttachmentsRootPath, "MedicalReports"));
                    Directory.CreateDirectory(Path.Combine(AttachmentsRootPath, "MedicalReports"));
                }

                if (!Directory.Exists(Path.Combine(AttachmentsRootPath, "Receipts")))
                {
                    _logger.LogInformation("Creating Receipts directory: {ReceiptsPath}", Path.Combine(AttachmentsRootPath, "Receipts"));
                    Directory.CreateDirectory(Path.Combine(AttachmentsRootPath, "Receipts"));
                }


                byte[] bytes = Convert.FromBase64String(fileData);
                string UniqueFileName = Guid.NewGuid().ToString();
                string fileExtension = $".{MimeGuesser.GuessExtension(bytes)}";
                string folder = attachmentType switch
                {
                    AttachmentType.PERSONAL_PICTURE => "PersonalPictures",
                    AttachmentType.IDENTITY => "IdentityProofs",
                    AttachmentType.MEDICAL_REPORT => "MedicalReports",
                    AttachmentType.RECIEPT => "Receipts",
                    _ => "",
                };
                string uploadFolder = Path.Combine(AttachmentsRootPath, folder);
                string filePath = Path.Combine(uploadFolder, UniqueFileName + fileExtension);
                await File.WriteAllBytesAsync(filePath, bytes);
                if (File.Exists(filePath))
                {
                    return new FileUploadResult
                    {
                        Succeed = true,
                        Message = "File uploaded successfully",
                        FileName = UniqueFileName + fileExtension
                    };
                }
                else
                {
                    return new FileUploadResult
                    {
                        Succeed = false,
                        Message = "Failed to upload file"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file of type: {AttachmentType}", attachmentType.ToString());
                return new FileUploadResult
                {
                    Succeed = false,
                    Message = "Error uploading file: " + ex.Message
                };
                throw;
            }
        }


    }
}

