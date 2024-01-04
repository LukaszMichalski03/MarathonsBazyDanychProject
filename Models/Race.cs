namespace BDProject_MarathonesApp.Models
{
    public class Race
    {
        public int Id;
        public string Name;
        public string Description;
        public double Distance;

        public DateTime Date;
        public Address? Address;
    }
}
