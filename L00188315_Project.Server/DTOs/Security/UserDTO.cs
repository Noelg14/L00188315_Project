namespace L00188315_Project.Server.DTOs.Security
{
    /// <summary>
    /// DTO returned after a successful login or registration.
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Email of the logged in or registered user.
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Display name of the logged in or registered user.
        /// </summary>
        public string? DisplayName { get; set; }
        /// <summary>
        /// Token used for authentication.
        /// </summary>
        public string? Token { get; set; }
        /// <summary>
        /// User ID of the logged in or registered user.
        /// </summary>
        public string? UserId { get; set; }
    }
}
