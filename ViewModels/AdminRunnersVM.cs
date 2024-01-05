using BDProject_MarathonesApp.Models;

namespace BDProject_MarathonesApp.ViewModels
{
    public class AdminRunnersVM
    {
        public int Raceid { get; set; }
        public List<Participant> Participants { get; set; }
    }
}
