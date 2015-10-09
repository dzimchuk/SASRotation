using System.Threading.Tasks;
using System.Web.Http;
using TokenService.Models;

namespace TokenService.Controllers
{
    [RoutePrefix("api")]
    public class TokenController : ApiController
    {
        private readonly ITokenService tokenService;

        public TokenController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        [Route("readtoken")]
        public async Task<Token> GetReadToken()
        {
            return new Token { SharedAccessSignature = await tokenService.GetReadSharedAccessSignature() };
        }

        [Route("writetoken")]
        public async Task<Token> GetWriteToken()
        {
            return new Token { SharedAccessSignature = await tokenService.GetWriteSharedAccessSignature() };
        }
    }
}