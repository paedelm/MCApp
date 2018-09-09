using System.Threading.Tasks;

namespace MCApp.API.BackgroundServices
{
    public interface IProcess<TProcessParam>
    {
        Task ProcessAsync(TProcessParam param);
        Task ProcessAsync();
    }
}