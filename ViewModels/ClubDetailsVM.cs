using BDProject_MarathonesApp.Models;

namespace BDProject_MarathonesApp.ViewModels
{
    public class ClubDetailsVM
    {
        public Club Club { get; set; }
        public List<Club> Clubs { get; set; }
        public int UserId { get; set; }
    }
}
