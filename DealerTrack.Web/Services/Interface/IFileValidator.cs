using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace DealerTrack.Web.Services.Interface
{
    public interface IFileValidator
    {
        List<string> Validate(IFormFile file);
    }
}
