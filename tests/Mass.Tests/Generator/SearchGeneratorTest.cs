using Mass.Generator;
using MongoDB.Bson;

namespace Mass.Tests.Generator;

[Trait("Category", "Unit")]
[Trait("Category", "Generator")]
public class SearchGeneratorTest
{
    private readonly SearchGenerator _searchGenerator;

    public SearchGeneratorTest()
    {
        _searchGenerator = new SearchGenerator();
    }


    [Fact]
    [Trait("Category", "Unit")]
    [Trait("Category", "Generator")]
    [Trait("Generator", "BuildSearch")]
    public void Should_BuildSearchPipeline()
    {
        var indexName = "index";
        var searchTerm = "test";
        var pipeline = _searchGenerator.BuildSearch(indexName, searchTerm);

        Assert.NotNull(pipeline);
    }
}