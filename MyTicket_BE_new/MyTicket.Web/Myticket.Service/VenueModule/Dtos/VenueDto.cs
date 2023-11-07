using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.VenueModule.Dtos
{
    public class VenueDto
    {
        public int Id { get; set; }

        /// <summary>
        /// Tên sân vận động
        /// </summary>
        public required string Name { get; set; }

        public string? Address { get; set; }
        public int Capacity { get; set; }
        public string? Description { get; set; }

        public string? Image { get; set; }

    }
}
