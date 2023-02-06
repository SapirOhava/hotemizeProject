using System.Text.Json;
using BulkyBookWeb.Data;
using BulkyBookWeb.Models;

namespace BulkyBookWeb.HttpServices
{
    public class ChainResource : IChainResource
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private Memory memory;
        private WebService webService;
        private FileSystem fileSystem;
        private const string fileSystemPath = @"D:\path.json";

        public ChainResource(HttpClient httpClient, IConfiguration configuration, ApplicationDbContext db)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _db = db;
            memory = new Memory();
            webService = new WebService();
            fileSystem = new FileSystem();
        }
        private void insertToMemory(ExchangeRateList rateRes)
        {
            _db.RatesRes.Add(rateRes);
            _db.SaveChanges();
            memory.LastUpdatedTime = DateTime.Now;
        }
        private void insertToFileSystem(ExchangeRateList rateRes)
        {
            string json = JsonSerializer.Serialize(rateRes);
            File.WriteAllText(fileSystemPath, json);
            fileSystem.LastUpdatedTime = DateTime.Now;
        }
        private async Task<ExchangeRateList?> GetFromMemory()
        {
            return _db.RatesRes.FirstOrDefault();
        }
        private async Task<ExchangeRateList?> GetFromFileSystem()
        {
            if (File.Exists(fileSystemPath))
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
                //we make a post async request to our client
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
            var currentTime = DateTime.Now;
            IStorage[] resources = { memory, fileSystem, webService };
            TimeSpan timeDiff;
            List<Func<Task<ExchangeRateList?>>> DelegateStartup =
            new List<Func<Task<ExchangeRateList?>>>() { GetFromMemory, GetFromFileSystem, GetFromWebService };
            for (int i = 0; i < resources.Length; i++)
            {
                var resource = resources[i];
                timeDiff = currentTime - resource.LastUpdatedTime;
                if (resource.IsInserted && timeDiff.Hours < resource.HoursUntilExpire)
                {
                    return await DelegateStartup[i]();
                }
            }
            return null;
        }

    }
}