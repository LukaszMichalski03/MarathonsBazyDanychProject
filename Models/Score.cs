namespace BDProject_MarathonesApp.Models
{
    public class Score
    {
        public int Id { get; set; }
        public Participant? Runner { get; set; }
        public TimeSpan Finish_Time { get; set; }
        public int Place {  get; set; }
        public Race? Race { get; set; }
    }
}
