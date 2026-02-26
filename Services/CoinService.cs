using CryptoSilverTracker.Models;
using Newtonsoft.Json.Linq;

namespace CryptoSilverTracker.Services
{
    public class CoinService
    {
        private readonly HttpClient _httpClient;

        public CoinService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "CryptoTrackerApp/1.0");
        }

        
        public async Task<List<CoinPrice>> GetPricesAsync()
        {
            var url = "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin,ethereum,tether,tether-gold&vs_currencies=usd";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return new List<CoinPrice>();

            var jsonString = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(jsonString);
            var prices = new List<CoinPrice>();

            if (data["bitcoin"] != null) prices.Add(new CoinPrice { Id = "bitcoin", CurrentPrice = (decimal)data["bitcoin"]["usd"], Currency = "usd" });
            if (data["ethereum"] != null) prices.Add(new CoinPrice { Id = "ethereum", CurrentPrice = (decimal)data["ethereum"]["usd"], Currency = "usd" });
            if (data["tether-gold"] != null) prices.Add(new CoinPrice { Id = "tether-gold", CurrentPrice = (decimal)data["tether-gold"]["usd"], Currency = "usd" });

            return prices;
        }

        
        public async Task<CoinHistory> GetHistoryAsync(string coinId, int days)
        {
            var url = $"https://api.coingecko.com/api/v3/coins/{coinId}/market_chart?vs_currency=usd&days={days}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) return null;

            var jsonString = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(jsonString);

            var history = new CoinHistory
            {
                CoinId = coinId,
                
                Prices = data["prices"]?.ToObject<List<decimal[]>>() ?? new List<decimal[]>()
            };

            return history;
        }
    }
}