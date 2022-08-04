using System.Text;
using Azure.Identity;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton(new BlobContainerClient(
    new Uri($"https://stpwdless.blob.core.windows.net/data"),
    new DefaultAzureCredential()));
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.Map("ping", () => "pong");

app.Map("blob", async (BlobContainerClient bcc) => {
    var bc = bcc.GetBlobClient("greeting.txt");
    using var ms = new MemoryStream();
    await bc.DownloadToAsync(ms);
    var greeting = Encoding.UTF8.GetString(ms.ToArray());
    return greeting;
});

app.Run();
