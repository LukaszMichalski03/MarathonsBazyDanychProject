namespace BDProject_MarathonesApp.Models
{
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Address? Address { get; set; }
    }
}
