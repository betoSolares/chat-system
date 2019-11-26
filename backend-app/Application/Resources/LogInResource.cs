using System.ComponentModel.DataAnnotations;
​
namespace backend_app.Application.Resources
{
    public class LogInResource
    {
        [Required]
        [MaxLength(20)]
        public string Username { get; set; }
​
        [Required]
        public string Password { get; set; }
    }
}