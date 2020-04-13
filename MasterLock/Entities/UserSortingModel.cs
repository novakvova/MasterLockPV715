using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MasterLock.Entities
{
    public class UserSortingModel
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public string Value { get; set; }

    }
}
