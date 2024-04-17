using WhatsappWrapper;
using IApplicationLifetime = Microsoft.Extensions.Hosting.IApplicationLifetime;
var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
var httpClientFactory = app.Services.GetRequiredService<IHttpClientFactory>();
IApplicationLifetime applicationLifetime = app.Services.GetService<IApplicationLifetime>();
startup.Configure(app, applicationLifetime);
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
