﻿using AcBlog.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.SDK
{
    public static class KeywordsServiceExtensions
    {
        public static Task<Keyword?[]> GetKeywords(this IKeywordService service, IEnumerable<string> ids)
        {
            List<Task<Keyword?>> posts = new List<Task<Keyword?>>();
            foreach (var id in ids)
                posts.Add(service.Get(id));
            return Task.WhenAll(posts.ToArray());
        }
    }
}
