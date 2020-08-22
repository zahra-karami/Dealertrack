using System.Collections.Generic;
using System.IO;
using DealerTrack.Web.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace DealerTrack.Web.Services
{
    public class FileValidator : IFileValidator
    {
        private readonly List<string> _permittedExtensions;
        private readonly long _fileSizeLimit;


        public FileValidator(IConfiguration config)
        {
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
            _permittedExtensions = config.GetSection("PermittedExtensions").Get<List<string>>();
        }

        public List<string> Validate(IFormFile file)
        {
            var errors = new List<string>();
            if (file == null || file.Length <= 0)
            {
                errors.Add("The file is not provided");
                return errors;
            }

            // File extension validation
            var fileName = file.FileName;
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(ext) || !_permittedExtensions.Contains(ext))
            {
                errors.Add("The file type is invalid.");
            }

            // File size validation
            if (file.Length > _fileSizeLimit)
            {
                errors.Add("The file is too large.");
            }

            return errors;
        }
    }
}
