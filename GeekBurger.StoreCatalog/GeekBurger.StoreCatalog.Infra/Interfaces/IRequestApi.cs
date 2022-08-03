using System.Net.Http;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Infra.Interfaces
{
    public interface IRequestApi
    {
        Task<HttpResponseMessage> GetProductionAreas();
        Task<HttpResponseMessage> GetProducts(string restrictions);
    }
}
