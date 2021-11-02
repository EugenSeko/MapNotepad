using SQLite;

namespace MapNotepad.Model
{
    [Table("Pins")]
   public class PinModel : IEntityBase
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public bool IsFavorite { get; set; }
        public int UserId { get; set; } // external key


    }
}
