using DealerTrack.Web.Services;
using DealerTrack.Web.Services.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace DealerTrack.Test
{
    public class CustomServiceBuilder
    {
        public ServiceProvider ServiceProvider { get; private set; }
        public CustomServiceBuilder()
        {
            var service = new ServiceCollection();
            service.AddSingleton<IPasswordHasher, PasswordHasher>();
            service.AddSingleton<IUserService, UserService>();
            ServiceProvider = service.BuildServiceProvider();

        }
    }
}
