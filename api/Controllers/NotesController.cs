using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using my_auth_api_demo.Authorization;
using my_auth_api_demo.Services;

namespace my_auth_api_demo.Controllers
{
    [Authorize]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [Route("[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NoteStore _store;
        private readonly IAuthorizationService _authService;

        public NotesController(NoteStore store, IAuthorizationService authService)
        {
            _store = store;
            _authService = authService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNote(int id)
        {
            var note = _store.GetById(id);
            if (note is null) return NotFound();

            var result = await _authService.AuthorizeAsync(User, note, new SameOwnerRequirement());
            if (!result.Succeeded) return Forbid();

            return Ok(note);
        }
    }
}
