using Backend_test.Models;
using Microsoft.Extensions.Caching.Memory;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Backend_test.Services
{
    public class API
    {
        private static readonly string APIEndpoint = "https://jsonplaceholder.typicode.com";

        public static async Task<IEnumerable<Post>> GetPostsAsync(IMemoryCache cache)
        {
            if (cache.TryGetValue("Posts", out IEnumerable<Post> cachePosts))
            {
                return cachePosts;
            }

            var client = new RestClient($"{APIEndpoint}/posts");
            var request = new RestRequest(Method.GET);
            var response = await client.ExecuteAsync(request);
            var posts = JsonSerializer.Deserialize<List<Post>>(response.Content);

            cache.Set("Posts", posts, DateTime.Now.AddMinutes(5));

            return posts;
        }

        public static async Task<IEnumerable<Comment>> GetCommentsAsync(IMemoryCache cache)
        {
            if (cache.TryGetValue("Comments", out IEnumerable<Comment> cacheComments))
            {
                return cacheComments;
            }

            var client = new RestClient($"{APIEndpoint}/comments");
            var request = new RestRequest(Method.GET);
            var response = await client.ExecuteAsync(request);
            var comments = JsonSerializer.Deserialize<List<Comment>>(response.Content);

            cache.Set("Comments", comments, DateTime.Now.AddMinutes(5));

            return comments;
        }
    }
}
