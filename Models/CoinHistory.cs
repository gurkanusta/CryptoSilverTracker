namespace CryptoSilverTracker.Models
{
    public class CoinHistory
    {
        public string CoinId { get; set; }
        public List<decimal[]> Prices { get; set; }
    }
}