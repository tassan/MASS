using Mass.Extensions;
using Mass.Generator;
using MongoDB.Bson;

namespace Mass.Tests.Extensions;

public class BsonDocumentArrayExtensionTest
{
    private readonly BsonArray _documents;

    public BsonDocumentArrayExtensionTest()
    {
        var generator = new SearchGenerator();
        _documents = generator.BuildSearch("default", "search");
    }

    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Project")]
    public void Project_ShouldAddProjectToBsonDocument()
    {
        // Arrange
        var fields = new[] { "name", "description" };

        // Act
        _documents.Project(fields);

        // Assert
        Assert.Equal(2, _documents.Count);
        Assert.Equal("$search", _documents[0].AsBsonDocument.GetElement(0).Name);
        Assert.Equal("$project", _documents[1].AsBsonDocument.GetElement(0).Name);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Meta")]
    public void Meta_ShouldAddMetaToBsonDocument()
    {
        // Arrange
        var generator = new SearchGenerator();
        var pipe = generator.BuildSearch("default", "test");
        
        // Act
        pipe.Project(new []{ "name", "description" })
            .Meta("name", "textScore");
        
        // Assert
        Assert.Equal(2, pipe.Count);
        Assert.Equal("$search", pipe[0].AsBsonDocument.GetElement(0).Name);
        Assert.Equal("$project", pipe[1].AsBsonDocument.GetElement(0).Name);
        Assert.Equal("name", pipe[1].AsBsonDocument.GetElement(1).Value.AsBsonDocument.GetElement(0).Name);
        Assert.Equal("description", pipe[1].AsBsonDocument.GetElement(1).Value.AsBsonDocument.GetElement(1).Name);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Meta")]
    public void Meta_ShouldThrowExceptionWhenProjectIsNotCalled()
    {
        // Arrange
        var generator = new SearchGenerator();
        var pipe = generator.BuildSearch("default", "test");
        
        // Act
        var exception = Assert.Throws<KeyNotFoundException>(() => pipe.Meta("name", "textScore"));
        
        // Assert
        Assert.Equal("You must call Project() before calling Meta()", exception.Message);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Score")]
    public void Score_ShouldAddScoreToBsonDocument()
    {
        // Arrange
        var generator = new SearchGenerator();
        var pipe = generator.BuildSearch("default", "test");
        pipe.Project(new[] { "name", "description" });
        
        // Act
        pipe.Score();
        
        // Assert
        Assert.Equal(2, pipe.Count);
        Assert.Equal("$search", pipe[0].AsBsonDocument.GetElement(0).Name);
        Assert.Equal("$project", pipe[1].AsBsonDocument.GetElement(0).Name);
        Assert.Equal("score", pipe[1].AsBsonDocument["$project"].AsBsonDocument.GetElement(0).Name);
    }
}