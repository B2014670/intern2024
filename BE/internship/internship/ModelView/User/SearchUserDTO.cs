using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTO.User
{
    public class SearchUserDTO
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int? RowOfPage { get; set; }
        public int? Page { get; set; }
    }
}
