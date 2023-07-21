using MongoDB.Bson;

namespace Mass.Extensions;

public static class BsonDocumentArrayExtension
{
    /// <summary>
    ///  Add Project field to the specified BsonDocument
    /// </summary>
    /// <param name="pipeline"></param>
    /// <param name="fields"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static BsonArray Project(this BsonArray pipeline, string[] fields)
    {
        try
        {
            var project = new BsonDocument("$project", new BsonDocument());
            var projectFields = new BsonDocument();

            foreach (var field in fields)
            {
                projectFields.Add(field, 1);
            }

            project.Add("fields", projectFields);

            pipeline.Add(project);

            return pipeline;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static BsonArray Meta(this BsonArray pipeline, string name, string value)
    {
        try
        {
            if (pipeline.Last() is not BsonDocument doc || doc["$project"] is null)
                throw new KeyNotFoundException("You must call Project() before calling Meta()");

            doc.Add(name, new BsonDocument { { "$meta", value } });

            return pipeline;
        }
        catch (KeyNotFoundException ke)
        {
            throw new KeyNotFoundException("You must call Project() before calling Meta()", ke);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    ///  Add Score field to the specified BsonDocument inside Project
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static BsonArray Score(this BsonArray pipeline, string name = "score")
    {
        try
        {
            if (pipeline.Last().AsBsonDocument["$project"] is null)
                throw new Exception("You must call Project() before calling Score()");
            
            pipeline.Last().AsBsonDocument["$project"].AsBsonDocument.Meta(name, "searchScore");

            return pipeline;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Add Highlights field to the specified BsonDocument inside Project
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static BsonArray Highlights(this BsonArray pipeline, string name = "highlights")
    {
        try
        {
            if (pipeline.Last() is not BsonDocument doc || doc["$project"] is null)
                throw new Exception("You must call Project() before calling Highlights()");
            
            pipeline.Last().AsBsonDocument.Meta(name, "searchHighlights");
            
            return pipeline;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}