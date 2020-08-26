using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Pharmacies.DataStore.Models;

namespace Pharmacies.Models.Base
{
    public abstract class Entity<T> : IEntity<T>
    {
        [NotMapped] public List<string> IgnoreOnUpdate { get; } = new List<string>();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity),
            Column("Id", Order = 0),
            Key]
        public T Id { get; set; }

    }
}
