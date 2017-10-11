using System;

namespace Ankieter.Models
{
    public abstract class BaseEntiity
    {
        public int Id { get; protected set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}