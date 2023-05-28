using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAccountCache _accountCache;

        public AccountController(IAccountService accountService, IAccountCache accountCache)
        {
            _accountService = accountService;
            _accountCache = accountCache;
        }

        [Authorize] 
        [HttpGet]
        public ValueTask<Account> Get()
        {
            #region TODO 3: Get user id from cookie
            return _accountService.LoadOrCreateAsync(
                User.FindFirst(x => x.Type == 
                ClaimsIdentity.DefaultNameClaimType).Value);
            #endregion
        }

        #region TODO 5: Endpoint should works only for users with "Admin" Role
        [Authorize(Roles = "Admin")]
        #endregion
        [HttpGet("{id}")]
        public Account GetByInternalId([FromRoute] int id)
        {
            return _accountService.GetFromCache(id);
        }

        [Authorize]
        [HttpPost("counter")]
        public async Task UpdateAccount()
        {
            var account = await Get();
            account.Counter++;
            #region TODO 6
            _accountCache.AddOrUpdate(account);
            #endregion
        }
    }
}