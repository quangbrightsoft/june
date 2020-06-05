using System;

namespace Brightsoft.Data.Entities
{
    public class Entity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
        
    }
}