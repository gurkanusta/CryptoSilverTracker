using CryptoSilverTracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace CryptoSilverTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoinsController : ControllerBase
    {
        private readonly CoinService _coinService;

        public CoinsController(CoinService coinService)
        {
            _coinService = coinService;
        }

        [HttpGet("prices")]
        public async Task<IActionResult> GetPrices()
        {
            var prices = await _coinService.GetPricesAsync();
            if (prices == null || prices.Count == 0) return NotFound("Veri çekilemedi.");
            return Ok(prices);
        }

        
        [HttpGet("history/{coinId}/{days}")]
        public async Task<IActionResult> GetHistory(string coinId, int days)
        {
            var data = await _coinService.GetHistoryAsync(coinId, days);
            if (data == null) return NotFound("Grafik verisi bulunamadı.");

            return Ok(data);
        }
    }
}