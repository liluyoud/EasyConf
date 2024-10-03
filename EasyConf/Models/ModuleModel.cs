using System.Text.Json.Serialization;

namespace EasyConf.Models;

public class ModuleModel
{
    [JsonPropertyName("group")]
    public string? Group { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("personal")]
    public bool Personal { get; set; }

    [JsonPropertyName("selected")]
    public bool Selected { get; set; }

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }
}
