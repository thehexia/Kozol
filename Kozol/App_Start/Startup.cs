using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Kozol.Startup))]
namespace Kozol {
    public class Startup {
        public void Configuration(IAppBuilder app) {
            app.MapSignalR();
        }
    }
}