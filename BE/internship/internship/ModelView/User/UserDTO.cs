using System.ComponentModel.DataAnnotations;

namespace internship.ModelView.User
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Role { get; set; }
        public required bool Status { get; set; }
        public DateTime RegisteredTime { get; set; }
        public DateTime UpdatedTime { get; set; }
    }
}
