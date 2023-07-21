using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mass.Extensions;
using Mass.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mass.Generator
{
    public class SearchGenerator
    {
        public BsonDocument[] BuildSearch(string index, string searchTerm, IOptions<SearchOptions>? options = null)
        {
            var search = new BsonDocument("$search", new BsonDocument());

            search.Index(index);

            var pipeline = new[]
            {
                search
            };

            return pipeline;
        }
    }
}
