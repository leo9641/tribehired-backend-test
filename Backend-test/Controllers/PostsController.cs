using Backend_test.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public PostsController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetTopPosts()
        {
            var posts = await API.GetPostsAsync(_cache);
            var comments = await API.GetCommentsAsync(_cache);

            var topPosts = (from p in posts
                            join c in comments on p.Id equals c.PostId
                            group new { p, c } by new { p.Title, p.Body, c.PostId } into g
                            orderby g.Key.PostId
                            select new
                            {
                                post_id = g.Key.PostId,
                                post_title = g.Key.Title,
                                post_body = g.Key.Body,
                                total_number_of_comments = g.Count()
                            }).OrderByDescending(x => x.total_number_of_comments).ToList();

            return new OkObjectResult(topPosts);
        }
    }
}
