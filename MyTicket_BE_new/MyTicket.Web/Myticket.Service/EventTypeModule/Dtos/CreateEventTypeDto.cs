using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.EventTypeModule.Dtos
{
    public class CreateEventTypeDto
    {
        /// <summary>
        /// Tên Loaị sự kiện
        /// </summary>
        private string _name = null!;
        public string Name
        {
            get => _name;
            set => _name = value.Trim();
        }
        /// <summary>
        /// Mô tả
        /// </summary>
        private string _description = null!;
        public string Description
        {
            get => _description;
            set => _description = value.Trim();
        }

        /// <summary>
        /// Anh
        /// </summary>
        private string _image = null!;
        public string Image
        {
            get => _image;
            set => _image = value.Trim();
        }
    }
}
