namespace internship.ModelView.User
{
    public class UpdateUserDTO
    {
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
        public string Password { get; set; }           
    }
}
