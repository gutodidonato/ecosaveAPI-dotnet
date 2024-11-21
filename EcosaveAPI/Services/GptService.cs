using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EcosaveAPI.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EcosaveAPI.Services
{
    public class GptService : IGptService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GptService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenAI:ApiKey"] 
                      ?? throw new ArgumentNullException("Chave da API do OpenAI não configurada.");
        }

        public async Task<string> ObterDicaReducaoConsumoAsync()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", _apiKey);

                var requestBody = new GptRequest
                {
                    Model = "gpt-4",
                    Messages = new[]
                    {
                        new GptMessage { Role = "system", Content = "Você é um assistente energético." },
                        new GptMessage { Role = "user", Content = "Como posso reduzir meu gasto de energia?" }
                    },
                    MaxTokens = 150
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

                if (!response.IsSuccessStatusCode)
                {
                    // Logar o erro se necessário
                    return "Não foi possível obter uma dica no momento. Por favor, tente novamente mais tarde.";
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GptResponse>(responseBody);

                return result?.Choices?.FirstOrDefault()?.Message?.Content 
                       ?? "Não foi possível interpretar a resposta da API.";
            }
            catch (Exception ex)
            {
                return "Ocorreu um erro ao processar sua solicitação. Por favor, tente novamente.";
            }
        }
    }

    // Classes auxiliares para representar a estrutura da API
    public class GptRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("messages")]
        public GptMessage[] Messages { get; set; }

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; }
    }

    public class GptMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    public class GptResponse
    {
        [JsonPropertyName("choices")]
        public GptChoice[] Choices { get; set; }
    }

    public class GptChoice
    {
        [JsonPropertyName("message")]
        public GptMessage Message { get; set; }
    }
}
