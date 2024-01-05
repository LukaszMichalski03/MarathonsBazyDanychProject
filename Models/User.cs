namespace BDProject_MarathonesApp.Models
{
    public class User
    {
        public int Id;
        public string Name;
        public string LastName;
        public string Login;
        public string Password;
        public Address? Address;
        public Club? Club;
    }
}
