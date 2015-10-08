using System.Threading.Tasks;

namespace TokenService
{
    public interface ITokenService
    {
        Task<string> GetSharedAccessSignature(string ruleName);
    }
}