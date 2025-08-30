
using System.Text.Json.Serialization;

namespace SemanticKernel.API.Models;

public sealed class ChatModel
{

    [JsonPropertyName("input")]
    public required string Input { get; set; }
}
