using MongoDB.Bson;

namespace Mass.Extensions;

public static class BsonDocumentExtension
{
    /// <summary>
    /// Add an Search Index to the specified BsonDocument
    /// </summary>
    /// <param name="doc">Bson Document</param>
    /// <param name="name">Index Name</param>
    public static void Index(this BsonDocument doc, string name)
    {
        doc.Add("index", name);
    }

    /// <summary>
    /// Add Text field to the specified BsonDocument
    /// </summary>
    /// <param name="doc">Bson Document</param>
    /// <param name="allowDuplicateNames">(allowing duplicates is not recommended)</param>
    public static BsonDocument Text(this BsonDocument doc, bool allowDuplicateNames = false)
    {
        doc.Add("text", new BsonDocument
        {
            AllowDuplicateNames = allowDuplicateNames
        });

        return doc;
    }

    /// <summary>
    /// Add Query field to the specified BsonDocument
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="query"></param>
    /// <exception cref="Exception"></exception>
    public static BsonDocument Query(this BsonDocument doc, string query)
    {
        try
        {
            doc["text"].AsBsonDocument.Add("query", query);
            return doc;
        }
        catch (KeyNotFoundException e)
        {
            throw new KeyNotFoundException("You must call Text() before calling Query()", e);
        }
    }

    /// <summary>
    /// Add Path field to the specified BsonDocument
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="path"></param>
    /// <exception cref="Exception"></exception>
    public static BsonDocument Path(this BsonDocument doc, string path)
    {
        try
        {
            doc["text"].AsBsonDocument.Add("path", new BsonArray(new[] { path }));
            return doc;
        }
        catch (KeyNotFoundException e)
        {
            throw new KeyNotFoundException("You must call Text() before calling Path()", e);
        }
    }

    /// <summary>
    /// Add Path field to the specified BsonDocument
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="fuzzy"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    public static BsonDocument Fuzzy(this BsonDocument doc, int fuzzy)
    {
        try
        {
            if (fuzzy is <= 0 or > 2)
                throw new ArgumentOutOfRangeException(nameof(fuzzy), "Fuzzy value must be either 1 or 2");

            doc["text"].AsBsonDocument.Add("fuzzy", new BsonDocument("maxEdits", fuzzy));
            
            return doc;
        }
        catch (KeyNotFoundException e)
        {
            throw new KeyNotFoundException("You must call Text() before calling Fuzzy()", e);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new ArgumentOutOfRangeException(nameof(fuzzy), "Fuzzy value must be either 1 or 2");
        }
    }

    /// <summary>
    /// Add Highlight field to the specified BsonDocument
    /// </summary>
    /// <param name="doc"></param>
    /// <param name="fields">Array of fields to highlight. If null, then it'll use path.</param>
    /// <exception cref="KeyNotFoundException"></exception>
    public static BsonDocument Highlight(this BsonDocument doc, string[]? fields = null)
    {
        try
        {
            // check if document contains path field to use it as default
            if (fields is null && doc["text"]["path"] is not null)
                fields = doc["text"]["path"].AsBsonArray.Select(x => x.AsString).ToArray();

            doc.Add("highlight", new BsonArray(fields));
            return doc;
        }
        catch (KeyNotFoundException e)
        {
            throw new KeyNotFoundException("You must call Path() before calling Highlight()", e);
        }
    }
}