using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MeetingCentreService.Models.Entities
{
    /// <summary>
    /// Category of Accessories
    /// </summary>
    public class AccessoriesCategory
    {
        /// <summary>
        /// Database primary key
        /// </summary>
        [Key]
        public long CategoryId { get; set; }
        /// <summary>
        /// Category name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Optional Category icon
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// When available, the icon, otherwise the name
        /// </summary>
        public string Display { get { return this.HasIcon ? this.Icon : this.Name; } }
        /// <summary>
        /// Whether this Category has an icon
        /// </summary>
        public bool HasIcon { get { return this.Icon != null; } }
        /// <summary>
        /// Database foreign key reference for Accessories
        /// </summary>
        private IEnumerable<Accessory> Accessories { get; set; }
    }
}
