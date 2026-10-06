using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AddinArtama {
  public class ApiErrorResponse {
    public string mensagem { get; set; }
    public string pilha { get; set; }
  }

  public static class ApiError {
    /// <summary>
    /// Interpreta o conteúdo JSON retornado pela API e retorna uma mensagem legível.
    /// Suporta arrays, objetos únicos ou JSONs inesperados.
    /// </summary>
    public static string Parse(string content) {
      if (string.IsNullOrWhiteSpace(content))
        return "Erro desconhecido (resposta vazia).";

      string errorMessage = "";

      try {
        // Remove espaços antes de verificar o tipo
        string trimmed = content.TrimStart();

        if (trimmed.StartsWith("[")) {
          // Caso o retorno seja um array de erros
          var list = JsonConvert.DeserializeObject<List<ApiErrorResponse>>(content);
          if (list != null && list.Any()) {
            errorMessage = string.Join("\r\n", list
                .Where(e => !string.IsNullOrWhiteSpace(e.mensagem))
                .Select(e => e.mensagem));
          }
        } else if (trimmed.StartsWith("{")) {
          // Caso o retorno seja um único objeto
          var obj = JsonConvert.DeserializeObject<ApiErrorResponse>(content);
          if (obj != null && !string.IsNullOrWhiteSpace(obj.mensagem))
            errorMessage = obj.mensagem;
        }

        if (string.IsNullOrWhiteSpace(errorMessage)) {
          // Fallback genérico — tenta pegar o campo "mensagem" direto
          var jsonObj = JObject.Parse(content);
          errorMessage = jsonObj["mensagem"]?.ToString()
              ?? jsonObj["error"]?.ToString()
              ?? jsonObj["message"]?.ToString()
              ?? "Erro desconhecido ao interpretar a resposta.";
        }
      } catch (Exception ex) {
        // Fallback final — retorna o conteúdo bruto
        errorMessage = $"Erro inesperado ao interpretar resposta: {ex.Message}\r\nConteúdo: {content}";
      }

      return errorMessage.Trim();
    }
  }
}
