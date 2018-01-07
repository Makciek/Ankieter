using System.ComponentModel.DataAnnotations.Schema;

namespace Ankieter.Models.Forms
{
    public class CreatedForm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public string FormStructure { get; set; }
    }
}