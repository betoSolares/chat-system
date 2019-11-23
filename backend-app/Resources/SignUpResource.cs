using System.ComponentModel.DataAnnotations;

namespace backend_app.Resources
{
    public class SignUpResource
    {
        [Required]
        public string GivenName { get; set; }

        [Required]
        public string FamilyName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}