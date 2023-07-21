using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Mass.Models;

public class SearchOptions
{
    public int Limit { get; set; } = 20;
    public int Skip { get; set; } = 0;
    public string[] Fields { get; set; } = Array.Empty<string>();

    [MinLength(1), MaxLength(2)] public int Fuzzy { get; set; } = 1;
    public string[] Highlight => BuildHighlight();

    private string[] BuildHighlight()
    {
        return Fields.Select(field => $"{field}").ToArray();
    }

}