using System.Threading.Tasks;

namespace TokenService
{
    public interface ITokenService
    {
        Task<string> GetReadSharedAccessSignature();
        Task<string> GetWriteSharedAccessSignature();
    }
}