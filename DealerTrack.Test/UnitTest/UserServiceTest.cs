using System;
using System.Linq;
using System.Threading.Tasks;
using DealerTrack.Web.Models;
using DealerTrack.Web.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
namespace DealerTrack.Test.UnitTest
{

    public class UserServiceTest : IClassFixture<CustomServiceBuilder>
    {
        private static readonly Random random = new Random();
        private readonly IUserService _userService;
        public UserServiceTest(CustomServiceBuilder builder)
        {
            _userService = builder.ServiceProvider.GetService<IUserService>();
        }

        [Fact]
        public async Task RegisterUserTest_Ok()
        {
            (_, UserModel user) = GetUser();
            var response = await _userService.RegisterUser(user);
            Assert.True(response.IsSucceeded);
        }


        [Fact]
        public async Task RegisterUserTest_DuplicateUsername()
        {
            (_, UserModel user) = GetUser();
            await _userService.RegisterUser(user);
            var response = await _userService.RegisterUser(user);

            Assert.True(response.IsSucceeded == false);
            Assert.Contains($"Username {user.Username} is already taken", response.ResponseMessage);
        }

        [Fact]
        public async Task AuthenticateTest_Ok()
        {
            (var password, UserModel user) = GetUser();
            await _userService.RegisterUser(user);
            var response = await _userService.Authenticate(user.Username, password);

            Assert.True(response.IsSucceeded);
            Assert.True(response.Result.FirstName == user.FirstName);
            Assert.True(response.Result.LastName == user.LastName);
        }


        [Fact]
        public async Task AuthenticateTest_InvalidUsername()
        {
            (var password, UserModel user) = GetUser();
            await _userService.RegisterUser(user);
            var response = await _userService.Authenticate("invalid", password);

            Assert.True(response.IsSucceeded == false);
            Assert.Contains($"Username or password is incorrect", response.ResponseMessage);
        }

        [Fact]
        public async Task AuthenticateTest_InvalidPassword()
        {
            (_, UserModel user) = GetUser();
            await _userService.RegisterUser(user);
            var response = await _userService.Authenticate(user.Username, "invalid");

            Assert.True(response.IsSucceeded == false);
            Assert.Contains($"Username or password is incorrect", response.ResponseMessage);
        }

        private (string password, UserModel  user) GetUser()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var username = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
            var user = new UserModel
            {
                FirstName = "test",
                LastName = "test",
                Password = "test12345",
                Username = username,
            };
            return (user.Password, user);
        }
    }
}
