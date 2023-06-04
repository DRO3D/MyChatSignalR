namespace MyChat.Model
{
    public class AccountsModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
