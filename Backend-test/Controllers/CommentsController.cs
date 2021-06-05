using Backend_test.Models;
using Backend_test.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public CommentsController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> SearchComments(int postId, int id, string name, string email, string body)
        {
            IEnumerable<Comment> comments = await API.GetCommentsAsync(_cache);

            if (postId > 0) comments = comments.Where(x => x.PostId == postId);
            if (id > 0) comments = comments.Where(x => x.Id == id);
            if (!string.IsNullOrEmpty(name)) comments = comments.Where(x => x.Name.Contains(name));
            if (!string.IsNullOrEmpty(email))comments = comments.Where(x => x.Email.Contains(email));
            if (!string.IsNullOrEmpty(body)) comments = comments.Where(x => x.Body.Contains(body));

            var filteredComments = comments.ToList();

            return new OkObjectResult(filteredComments);
        }

    }
}
