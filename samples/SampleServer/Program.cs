using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SampleServer
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);
      builder.Services.AddSignalR();
      builder.Services.AddHostedService<WinformHost>();
      builder.Services.AddSingleton<TwainForm>();
      
      var app = builder.Build();
      app.MapHub<TwainHub>("/twain");
      app.MapGet("/about", (TwainForm form) => form.Text);

      app.Run();
    }
  }
}