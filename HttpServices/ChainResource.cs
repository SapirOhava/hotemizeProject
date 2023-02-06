using System.Text.Json;
using BulkyBookWeb.Models;
using Microsoft.Extensions.Caching.Memory;

namespace BulkyBookWeb.HttpServices
{
    public class ChainResource : IChainResource
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private IMemoryCache _cache;
        private Memory memory;
        private WebService webService;
        private FileSystem fileSystem;
        private const string fileSystemPath = @"C:\Users\Public\hotelmize.json";
        private const string ratesCacheKey = "ratesList";

        public ChainResource(HttpClient httpClient, IConfiguration configuration, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _cache = cache;
            memory = new Memory();
            webService = new WebService();
            fileSystem = new FileSystem();
        }
        private void insertToMemory(ExchangeRateList rateRes)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
            _cache.Set(ratesCacheKey, rateRes, cacheEntryOptions);
        }
        private void insertToFileSystem(ExchangeRateList rateRes)
        {
            string json = JsonSerializer.Serialize(rateRes);
            File.WriteAllText(fileSystemPath, json);
        }
        private async Task<ExchangeRateList?> GetFromMemory()
        {
            if (_cache.TryGetValue(ratesCacheKey, out ExchangeRateList rateList))
            {
                return rateList;
            }
            else
            {
                return null;
            }
        }
        private async Task<ExchangeRateList?> GetFromFileSystem()
        {
            if (File.Exists(fileSystemPath) && (DateTime.Now - File.GetLastWriteTime(fileSystemPath)).Hours < 4)
            {
                var rates = File.ReadAllText(fileSystemPath);
                var updatedRate = JsonSerializer.Deserialize<ExchangeRateList>(rates);
                insertToMemory(updatedRate);
                return updatedRate;
            }
            return null;
        }
        private async Task<ExchangeRateList?> GetFromWebService()
        {
            try
            {
                var resBodyString = await _httpClient.GetStringAsync($"{_configuration["openexchangerates"]}");
                var rates = JsonSerializer.Deserialize<ExchangeRateList>(resBodyString);
                insertToFileSystem(rates);
                insertToMemory(rates);
                return rates;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<ExchangeRateList?> GetValue()
        {
            IStorage[] resources = { memory, fileSystem, webService };
            List<Func<Task<ExchangeRateList?>>> DelegateStartup =
            new List<Func<Task<ExchangeRateList?>>>() { GetFromMemory, GetFromFileSystem, GetFromWebService };
            for (int i = 0; i < resources.Length; i++)
            {
                var res = await DelegateStartup[i]();
                if (res != null)
                {
                    return res;
                }
            }
            return null;
        }

    }
}