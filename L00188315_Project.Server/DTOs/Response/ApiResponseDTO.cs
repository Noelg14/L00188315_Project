using System.Text.Json.Serialization;

namespace L00188315_Project.Server.DTOs.Response
{
    /// <summary>
    /// Response DTO for API calls
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponseDTO<T>
    {
        /// <summary>
        /// The Data to be returned in the response
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // dont write if no data.
        public T? Data { get; set; }

        /// <summary>
        /// A message to be returned in the response - For example, an error.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // dont write if no data.
        public string? Message { get; set; }
        /// <summary>
        /// Was the operation successful?
        /// </summary>
        public bool? Success { get; set; }
    }
}
