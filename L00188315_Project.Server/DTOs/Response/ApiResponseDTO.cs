using System.Text.Json.Serialization;

namespace L00188315_Project.Server.DTOs.Response
{
    public class ApiResponseDTO<T>
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // dont write if no data.
        public T Data { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // dont write if no data.
        public string? Message { get; set; }
        public bool? Success { get; set; }
    }
}
