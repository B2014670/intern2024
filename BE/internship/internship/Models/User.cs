using System.ComponentModel.DataAnnotations;

namespace internship.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public required bool Status { get; set; }
        public DateTime RegisteredTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
