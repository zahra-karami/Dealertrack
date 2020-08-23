using System.Threading.Tasks;
using DealerTrack.Web.Models;
using DealerTrack.Web.Utilities;

namespace DealerTrack.Web.Services.Interface
{
    public interface IUserService
    {

        Task<ResponseModel<bool>> RegisterUser(UserModel user);
        Task<ResponseModel<UserInfoModel>> Authenticate(string username, string password);

    }
}