using System.Threading.Tasks;
using System.Web.Http;
using TokenService.Models;

namespace TokenService.Controllers
{
    [RoutePrefix("api")]
    public class TokenController : ApiController
    {
        private readonly ITokenService tokenService;
        private readonly IConfiguration configuration;

        public TokenController(ITokenService tokenService, IConfiguration configuration)
        {
            this.tokenService = tokenService;
            this.configuration = configuration;
        }

        [Route("readtoken")]
        public async Task<Token> GetReadToken()
        {
            var ruleName = configuration.Find("ReadAuthorizationRuleName");
            return new Token { SharedAccessSignature = await tokenService.GetSharedAccessSignature(ruleName) };
        }

        [Route("writetoken")]
        public async Task<Token> GetWriteToken()
        {
            var ruleName = configuration.Find("WriteAuthorizationRuleName");
            return new Token { SharedAccessSignature = await tokenService.GetSharedAccessSignature(ruleName) };
        }
    }
}