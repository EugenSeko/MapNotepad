using SQLite;

namespace MapNotepad.Model
{
    [Table("Users")]
   public class UserModel:IEntityBase
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        [Unique]
        public string Email { get; set; } // the key
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
