using System;

namespace Ankieter.Models.Views.Forms
{
    public class FormViewModel
    {
        public int Id { get; set; }
        public string MongoId { get; set; }
        public string Name { get; set; }
        public DateTime Modification { get; set; }
    }
}