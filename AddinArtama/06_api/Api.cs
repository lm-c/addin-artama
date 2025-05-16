using RestSharp;

namespace AddinArtama {
  internal partial class Api {
    public static string url = string.Empty;
    public static string token = string.Empty;

    internal static RestClient GetClient(string modulo, string endpoint) {
      var client = new RestClient($"{url}/{modulo}/v10/{endpoint}");

      return client;
    }
 
    internal static RestRequest CreateRequest(Method method) {
      RestRequest request = new RestRequest(method);

      request.AddHeader("accept", "application/json");
      request.AddHeader("Authorization", token);
      request.AddHeader("empresa", "1");

      return request;
    }

    public class ApiErrorResponse {
      public string mensagem { get; set; }
    }
  }
}
