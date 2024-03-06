namespace FieldAPI.Models
{
    public class Detail
    {
        public int Id { get; set; }
        public int FieldId { get; set; }
        public string Value { get; set; }
        public Field Field { get; set; }
    }
}
