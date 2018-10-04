using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JWT.Models
{
    public class Country
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string Name { get; set; }

        public List<State> States { get; set; }

        public Country()
        {
            States = new List<State>();
        }
    }
}
