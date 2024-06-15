using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using UseThi.Models;

public class CryptoService
{
    private readonly string _apiKey;

    public CryptoService(string apiKey)
    {
        _apiKey = apiKey;
    }

    public async Task<List<CryptoCurrency>> GetCryptocurrenciesAsync()
    {
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", _apiKey);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync("https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(jsonString);

                var cryptos = json["data"].Select(x => new CryptoCurrency
                {
                    Name = x["name"].ToString(),
                    Price = ParseDecimal(x["quote"]["USD"]["price"].ToString()),
                    Symbol = x["symbol"].ToString()
                }).ToList();

                return cryptos;
            }
            else
            {
                throw new Exception("Unable to retrieve data from CoinMarketCap API");
            }
        }
    }

    private decimal ParseDecimal(string value)
    {
        // Замінюємо кому на крапку перед парсингом
        value = value.Replace(',', '.');

        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
        {
            return result;
        }

        throw new FormatException($"Unable to parse '{value}' as decimal.");
    }
}
