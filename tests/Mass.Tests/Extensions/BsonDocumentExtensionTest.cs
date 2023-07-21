using Mass.Extensions;
using MongoDB.Bson;

namespace Mass.Tests.Extensions;

[Trait("Category", "Unit")]
[Trait("Category", "Extensions")]
public class BsonDocumentExtensionTest
{
    private readonly BsonDocument _document;

    public BsonDocumentExtensionTest()
    {
        _document = new BsonDocument();
    }

    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Index")]
    public void Index_ShouldAddIndexToBsonDocument()
    {
        // Arrange
        var index = "default";

        // Act
        _document.Index(index);

        // Assert
        Assert.Equal(index, _document["index"].AsString);
    }

    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Index")]
    public void Index_ShouldAddIndexToSearchDocument()
    {
        var search = new BsonDocument("$search", new BsonDocument());

        var index = "default";

        search.Index(index);

        Assert.Equal(index, search["index"].AsString);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Text")]
    public void Text_ShouldAddTextToBsonDocument()
    {
        // Act
        _document.Text();

        // Assert
        Assert.NotNull(_document["text"]);
        Assert.Equal(0, _document["text"].AsBsonDocument.ElementCount);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Text")]
    public void Text_ShouldAddTextToSearchDocument()
    {
        var search = new BsonDocument("$search", new BsonDocument());

        search.Text();

        Assert.NotNull(search["text"]);
        Assert.Equal(0, search["text"].AsBsonDocument.ElementCount);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Query")]
    public void Query_ShouldAddQueryToBsonDocument()
    {
        // Arrange
        var query = "test";
        _document.Text();

        // Act
        _document.Query(query);

        // Assert
        Assert.Equal(query, _document["text"].AsBsonDocument["query"].AsString);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Query")]
    public void Query_ShouldThrowExceptionWhenTextIsNotCalled()
    {
        // Arrange
        var query = "test";

        // Act
        var exception = Assert.Throws<KeyNotFoundException>(() => _document.Query(query));

        // Assert
        Assert.Equal("You must call Text() before calling Query()", exception.Message);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Path")]
    public void Path_ShouldAddPathToBsonDocument()
    {
        // Arrange
        var path = "test";
        _document.Text();

        // Act
        _document.Path(path);

        // Assert
        Assert.Equal(path, _document["text"].AsBsonDocument["path"].AsBsonArray[0].AsString);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Path")]
    public void Path_ShouldThrowExceptionWhenTextIsNotCalled()
    {
        // Arrange
        var path = "test";

        // Act
        var exception = Assert.Throws<KeyNotFoundException>(() => _document.Path(path));

        // Assert
        Assert.Equal("You must call Text() before calling Path()", exception.Message);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Fuzzy")]
    public void Fuzzy_ShouldAddFuzzyToBsonDocument()
    {
        // Arrange
        var fuzzy = 1;
        _document.Text();

        // Act
        _document.Fuzzy(fuzzy);

        // Assert
        Assert.Equal(fuzzy, _document["text"].AsBsonDocument["fuzzy"].AsBsonDocument["maxEdits"].AsInt32);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Fuzzy")]
    public void Fuzzy_ShouldThrowExceptionWhenTextIsNotCalled()
    {
        // Arrange
        var fuzzy = 1;

        // Act
        var exception = Assert.Throws<KeyNotFoundException>(() => _document.Fuzzy(fuzzy));

        // Assert
        Assert.Equal("You must call Text() before calling Fuzzy()", exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(3)]
    [InlineData(10)]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Fuzzy")]
    public void Fuzzy_ShouldThrowExceptionWhenIsNeitherOneOrTwo(int value)
    {
        // Arrange
        _document.Text();
        
        // Act
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _document.Fuzzy(value));
        
        // Assert
        Assert.Equal("Fuzzy value must be either 1 or 2 (Parameter 'fuzzy')", exception.Message);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Highlight")]
    public void Highlight_ShouldAddHighlightToBsonDocument()
    {
        // Arrange
        var path = "test";
        _document
            .Text()
            .Query("test")
            .Path(path)
            .Fuzzy(1)
            .Highlight();

        // Assert
        Assert.Equal(path, _document["highlight"].AsBsonArray[0].AsString);
    }
    
    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Extensions")]
    [Trait("Extensions", "Highlight")]
    public void Highlight_ShouldThrowExceptionWhenPathIsNotCalled()
    {
        // Arrange
        _document
            .Text()
            .Query("test");

        // Act
        var exception = Assert.Throws<KeyNotFoundException>(() => _document.Highlight());

        // Assert
        Assert.Equal("You must call Path() before calling Highlight()", exception.Message);
    }
}