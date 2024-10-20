using System.Net;

namespace StoreGate.Common;

public class ApiException : Exception
{
    public Uri? RequestUri { get; init; }
    public HttpMethod? RequestMethod { get; init; }
    public string? RequestContent { get; init; }
    public HttpStatusCode ResponseStatusCode { get; init; }
    public string? ResponseContent { get; init; }

    public override string ToString() =>
        @$"Api Exception
            Request Uri: {RequestUri}
            Request Content: {RequestContent}
            Request Method: {RequestMethod}
            Response Status Code: {ResponseStatusCode}
            Response Content: {ResponseContent}";
}