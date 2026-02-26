
using System.Text;
using Newtonsoft.Json.Linq;
using CryptoSilverTracker.Models;

namespace CryptoSilverTracker.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetMarketAnalysisAsync(List<CoinPrice> prices)
        {
            var apiKey = _configuration["GeminiApiKey"];
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-pro:generateContent?key={apiKey}";

            var prompt = "Şu anki piyasa fiyatları: ";
            foreach (var p in prices) prompt += $"{p.Id}: {p.CurrentPrice}$, ";
            prompt += "Bu fiyatlara bakarak Twitter'da paylaşmalık kısa, dikkat çekici ve Türkçe bir finansal yorum yazar mısın? (Sadece tweet içeriğini ver)";

            var requestBody = new
            {
                contents = new[] { new { parts = new[] { new { text = prompt } } } }
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            var jsonString = await response.Content.ReadAsStringAsync();

            var data = JObject.Parse(jsonString);
            return data["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString() ?? "Yorum oluşturulamadı.";
        }
    }
}