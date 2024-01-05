using BDProject_MarathonesApp.Models;

namespace BDProject_MarathonesApp.ViewModels
{
    public class AdminVM
    {
        public List<User> Users { get; set; }
        public List<Race> Races { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Club> Clubs { get; set; }
        public List<Participant> Participants { get; set; }
        public List<Score> Scores { get; set; }


    }
}
