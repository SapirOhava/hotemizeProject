
using BulkyBookWeb.Models;

namespace BulkyBookWeb.HttpServices
{
    public interface IChainResource
    {
        Task<ExchangeRateList> GetValue();
    }
}