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
        public BsonArray BuildSearch(string index, string searchTerm, IOptions<SearchOptions>? options = null)
        {
            var search = Search();
            search.Index(index);
            
            var pipeline = new BsonArray
            {
                search
            };

            return pipeline;
        }

        /// <summary>
        ///  Add Search field to the specified BsonDocument
        /// </summary>
        /// <returns></returns>
        public BsonDocument Search()
        {
            return new("$search", new BsonDocument());
        }
    }
}