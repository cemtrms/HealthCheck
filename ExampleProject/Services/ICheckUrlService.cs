using System.Net.Http;
using System.Threading.Tasks;

namespace ExampleProject.Services
{
    public interface ICheckUrlService
    {
        Task<HttpResponseMessage> Check(int monitoringTaskId);
    }
}