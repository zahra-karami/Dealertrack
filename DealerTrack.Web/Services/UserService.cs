using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealerTrack.Web.Models;
using DealerTrack.Web.Services.Interface;
using DealerTrack.Web.Utilities;


namespace DealerTrack.Web.Services
{
    public class UserService : IUserService
    {
        private static readonly ConcurrentBag<UserModel> _userStore;
        static UserService()
        {
            _userStore = new ConcurrentBag<UserModel>();
        }

        public Task<ResponseModel<bool>> RegisterUser(UserModel user)
        {
            var response = new ResponseModel<bool>();
            CleanUser(user);
            var isTaken = _userStore.Any(c => c.Username == user.Username);

            if (!isTaken)
            {
                _userStore.Add(user);
                response.IsSucceeded = true;
            }
            else
            {
                response.ResponseMessage.Add($"Username {user.Username} is already taken");
            }
            
            return Task.FromResult(response);
        }

        public Task<ResponseModel<UserModel>> Authenticate(string username, string password)
        {
            var response = new ResponseModel<UserModel>();
           
            var user = _userStore.FirstOrDefault(c => c.Username == username.Trim().ToLower());

            if (user != null)
            {
                if (user.Password == password.Trim().ToLower())
                {
                    response.Result = user;
                    response.IsSucceeded = true;
                }
                else
                {
                    // here we can set try number and if it exceeds from n block the user
                    response.ResponseMessage.Add("Username or password is incorrect");
                }
            }
            else
            {
                response.ResponseMessage.Add("Username or password is incorrect");
            }

            return Task.FromResult(response);
        }
        private void CleanUser(UserModel userModel)
        {
            userModel.Username = userModel.Username.Trim().ToLower();
            userModel.Password = userModel.Password.Trim().ToLower();
            userModel.FirstName = userModel.FirstName.Trim().ToLower();
            userModel.LastName = userModel.LastName.Trim().ToLower();
            
            // hash user password
        }


        public Task<bool> IsOnline(string name)
        {
            return Task.FromResult(true);
        }

    }
}
