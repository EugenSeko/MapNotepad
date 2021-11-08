using SQLite;

namespace MapNotepad.Model
{
    [Table("Pins")]
   public class PinModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public bool IsFavorite { get; set; }
        public string UserId { get; set; } // external key


    }
}
