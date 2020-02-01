using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MeetingCentreService.Models.Entities
{
    public class AccessoriesCategory
    {
        [Key]
        public long CategoryId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Display { get { return this.HasIcon ? this.Icon : this.Name; } }
        public bool HasIcon { get { return this.Icon != null; } }
        private IEnumerable<Accessory> Accessories { get; set; }
    }
}
