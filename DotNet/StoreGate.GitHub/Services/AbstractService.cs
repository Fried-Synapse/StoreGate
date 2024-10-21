using System.Dynamic;
using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;
using StoreGate.Common.Services;
using StoreGate.GitHub.Models;

namespace StoreGate.GitHub.Services;

public abstract class AbstractService<TModel> : AbstractHttpService<TModel>
{
    protected AbstractService(Config config, ILogger logger) : base(logger)
    {
        Config = config;
    }


    protected override void InitClient(HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Config.Token);
    }

    protected Config Config { get; }
    protected override string ApiUri => $"https://api.github.com/repos/{Config.Repo}";
    protected virtual string UploadUri => $"https://uploads.github.com/repos/{Config.Repo}";

    protected Uri GetUploadUri(string? path = null, ExpandoObject? query = null)
        => GetUri(UploadUri, BasePath, path, query);

    protected async Task UploadAsync(Uri uri, string fileName)
        => await UploadAsync(uri, await File.ReadAllBytesAsync(fileName));

    protected async Task UploadAsync(Uri uri, byte[] bytes)
        => await UploadAsync(uri, new ByteArrayContent(bytes));

    protected async Task UploadAsync(Uri uri, HttpContent content)
    {
        content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
        await ProcessResponseAsync(await Client.PostAsync(uri, content));
    }
}