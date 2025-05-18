namespace L00188315_Project.Server.DTOs.Security
{
    /// <summary>
    /// DTO for user registration.
    /// </summary>
    public class RegisterDTO
    {
        /// <summary>
        /// Email of the user to be registered
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Password of the user to be registered
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Users name to be registered
        /// </summary>
        public string? Name { get; set; }
    }
}
