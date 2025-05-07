using RestSharp;

namespace AddinArtama {
  internal partial class Api {
    //private static string token = "eyJhbGciOiJFUzI1NiJ9.eyJpc3MiOiJhcGkiLCJhdWQiOiJhcGkiLCJleHAiOjE4OTE3OTMzODIsInN1YiI6IlJhZmFlbHAiLCJjc3dUb2tlbiI6IjVuODRGaFRLIiwiZGJOYW1lU3BhY2UiOiJjb25zaXN0ZW0ifQ.UMlmtHqPvo92FCGOQH5rlggmqS2y2rl8LhmoYEgsSMQBFmFbA6LBVVtglNAwapBKCDiM8uqKZSasabiyhi7jEw";
    private static string token = "eyJhbGciOiJFUzI1NiJ9.eyJpc3MiOiJhcGkiLCJhdWQiOiJhcGkiLCJleHAiOjE4OTg5NzU2MzEsInN1YiI6Ikxlb25hcmRvIiwiY3N3VG9rZW4iOiI3ODJpRVJFbiIsImRiTmFtZVNwYWNlIjoiY29uc2lzdGVtIn0.znZb_yqAmw4SxBJEZutPLi0Kx45CyxT0vLqyxXJr3mg7cB1JG3OzTC8eSCyMgWFbVHhWqCMGzONlQyrxQqTmcA";

    internal static RestClient GetClient(string modulo, string endpoint) {
      //var client = new RestClient($"https://192.168.1.250/api/{modulo}/v10/{endpoint}");
      var client = new RestClient($"https://192.168.1.100/api/{modulo}/v10/{endpoint}");

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
