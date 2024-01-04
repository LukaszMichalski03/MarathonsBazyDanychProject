namespace BDProject_MarathonesApp.Models
{
    public class Participant
    {
        public int Id { get; set; }
        public Race Race { get; set; }
        public int StartingNumber { get; set; }
        public User User { get; set; }
    }
}
