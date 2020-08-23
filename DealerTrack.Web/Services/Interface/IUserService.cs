using System.Threading.Tasks;
using DealerTrack.Web.Models;
using DealerTrack.Web.Utilities;

namespace DealerTrack.Web.Services.Interface
{
    public interface IUserService
    {
        Task<bool> IsOnline(string name);

        Task<ResponseModel<bool>> RegisterUser(UserModel user);
        Task<ResponseModel<UserModel>> Authenticate(string username, string password);

    }
}