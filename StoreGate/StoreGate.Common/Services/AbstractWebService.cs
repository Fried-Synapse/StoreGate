using System.Collections.Specialized;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace StoreGate.Common.Services;

public abstract class AbstractWebService<TModel> : IService
{
    protected AbstractWebService()
    {
        Client = new();
        Client.DefaultRequestHeaders.UserAgent.TryParseAdd("StoreGate");
    }

    protected HttpClient Client { get; }
    protected abstract string ApiUri { get; }
    protected abstract string BasePath { get; }

    protected virtual JsonSerializerSettings RequestJsonSettings { get; } = new()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    protected Uri GetUri(string? path = null, params (string Key, string Value)[] query)
    {
        UriBuilder uriBuilder = new($"{ApiUri}/{BasePath}{(path != null ? "/" : "")}{path}")
        {
            Query = StringifyQuery(query)
        };
        return uriBuilder.Uri;

        static string? StringifyQuery((string Key, string Value)[] query)
        {
            NameValueCollection collection = HttpUtility.ParseQueryString(string.Empty);

            foreach (var item in query)
            {
                collection.Add(item.Key, item.Value);
            }

            return collection.ToString();
        }
    }

    protected async Task PostAsync(Uri uri, TModel model)
        => await PostAsync<TModel>(uri, model);

    protected async Task PostAsync<T>(Uri uri, T model)
        => await ProcessResponse(await Client.PostAsync(uri, GetRequestContent(model)));

    protected async Task<TModel?> GetAsync(Uri uri)
        => await GetAsync<TModel>(uri);

    protected async Task<T?> GetAsync<T>(Uri uri)
        => await ProcessResponse<T>(await Client.GetAsync(uri));

    protected async Task PatchAsync(Uri uri, TModel model)
        => await PatchAsync<TModel>(uri, model);

    protected async Task PatchAsync<T>(Uri uri, T model)
        => await ProcessResponse(await Client.PatchAsync(uri, GetRequestContent(model)));

    private HttpContent GetRequestContent(object? model)
        => new StringContent(JsonConvert.SerializeObject(model, RequestJsonSettings));

    private static async Task ProcessResponse(HttpResponseMessage responseMessage)
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

    private static async Task<T?> ProcessResponse<T>(HttpResponseMessage responseMessage)
    {
        await ProcessResponse(responseMessage);
        return JsonConvert.DeserializeObject<T>(await responseMessage.Content.ReadAsStringAsync());
    }
}