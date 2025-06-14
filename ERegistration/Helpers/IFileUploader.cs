
using Core.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace DashBoard.Helpers
{
    public interface IFileUploader
    {
        public Task<FileUploadResult> UploadImage(IBrowserFile file, string folder);
        public Task<string> ToBase64(IBrowserFile file);
    }
}
