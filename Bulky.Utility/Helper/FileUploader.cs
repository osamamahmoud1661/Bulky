using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.BL.Services
{
    public static class FileUploader
    {
        public static string UploadFile(string LocalPath , IFormFile File)
        {
            string FolderPath = Directory.GetCurrentDirectory()+LocalPath;
            string FileName = Guid.NewGuid()+ Path.GetFileName(File.FileName);
            string FinalPth = Path.Combine(FolderPath, FileName);
            using(var stream = new FileStream(FinalPth, FileMode.Create))
            {
                File.CopyTo(stream);
            }
            return FileName;
        }
        public static string DeleteFile(string LocalPath , string FileName)
        {
            if (File.Exists(Directory.GetCurrentDirectory() + LocalPath+ FileName))
            {
                File.Delete(Directory.GetCurrentDirectory() + LocalPath + FileName);
                string resut = "File Deleted";
                return resut;
            }
            else
            {
                return "something went wronge!!";
            }
            
        }
    }
}
