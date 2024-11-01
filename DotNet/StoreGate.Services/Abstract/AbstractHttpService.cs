using System.Collections.Specialized;
using System.Dynamic;
using System.Web;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StoreGate.Models.Common;

namespace StoreGate.Services.Abstract;

public abstract class AbstractHttpService<TModel> : AbstractService
{
    private HttpClient? _client;
    protected HttpClient Client => _client ??= CreateClient();

    protected AbstractHttpService(ILogger logger) : base(logger)
    {
    }
    
    protected abstract string ApiUri { get; }
    protected abstract string BasePath { get; }

    protected virtual JsonSerializerSettings RequestJsonSettings { get; } = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    private HttpClient CreateClient()
    {
        HttpClient client = new();
        client.DefaultRequestHeaders.UserAgent.TryParseAdd("StoreGate");
        InitClient(client);
        return client;
    }

    protected virtual void InitClient(HttpClient httpClient)
    {
    }

    protected Uri GetApiUri(string? path = null, ExpandoObject? query = null)
        => GetUri(ApiUri, BasePath, path, query);

    protected Uri GetUri(string host, string basePath, string? path = null, ExpandoObject? query = null)
    {
        UriBuilder uriBuilder = new($"{host}/{basePath}{(path != null ? "/" : "")}{path}")
        {
            Query = StringifyQuery(query)
        };
        return uriBuilder.Uri;

        static string? StringifyQuery(ExpandoObject? query = null)
        {
            NameValueCollection collection = HttpUtility.ParseQueryString(string.Empty);

            if (query != null)
            {
                foreach (KeyValuePair<string, object?> item in query)
                {
                    collection.Add(item.Key, item.Value?.ToString());
                }
            }

            return collection.ToString();
        }
    }

    protected async Task<TModel?> PostAsync(Uri uri, TModel model)
        => await PostAsync<TModel>(uri, model);

    protected async Task<T?> PostAsync<T>(Uri uri, T model)
        => await ProcessResponseAsync<T>(await Client.PostAsync(uri, GetRequestContent(model)));

    protected async Task<TModel?> GetAsync(Uri uri)
        => await GetAsync<TModel>(uri);

    protected async Task<T?> GetAsync<T>(Uri uri)
        => await ProcessResponseAsync<T>(await Client.GetAsync(uri));

    protected async Task<TModel?> PatchAsync(Uri uri, TModel model)
        => await PatchAsync<TModel>(uri, model);

    protected async Task<T?> PatchAsync<T>(Uri uri, T model)
        => await ProcessResponseAsync<T>(await Client.PatchAsync(uri, GetRequestContent(model)));

    private HttpContent GetRequestContent(object? model)
        => new StringContent(JsonConvert.SerializeObject(model, RequestJsonSettings));

    protected async Task<T?> ProcessResponseAsync<T>(HttpResponseMessage responseMessage)
    {
        await ProcessResponseAsync(responseMessage);

        string content = await responseMessage.Content.ReadAsStringAsync();

        try
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
        catch
        {
            if (!string.IsNullOrEmpty(content))
            {
                Logger.LogWarning($"Could not deserialise response: {content}");
            }

            return default;
        }
    }
    
    protected async Task ProcessResponseAsync(HttpResponseMessage responseMessage)
    {
        if (!responseMessage.IsSuccessStatusCode)
        {
            string? requestContent = null;
            if (responseMessage.RequestMessage?.Content != null)
            {
                requestContent = await responseMessage.RequestMessage.Content.ReadAsStringAsync();
            }

            throw new ApiException
            {
                RequestUri = responseMessage.RequestMessage?.RequestUri,
                RequestContent = requestContent,
                RequestMethod = responseMessage.RequestMessage?.Method,
                ResponseStatusCode = responseMessage.StatusCode,
                ResponseContent = await responseMessage.Content.ReadAsStringAsync()
            };
        }
    }
}