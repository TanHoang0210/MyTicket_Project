using MYTICKET.BASE.SERVICE.Common.Validations;
using MYTICKET.WEB.DOMAIN.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.EventModule.Dtos
{
    public class CreateEventDto
    {
        /// <summary>
        /// Tên sự kiện
        /// </summary>
        [CustomMaxLength(264)]
        private string _eventName = null!;
        public string EventName
        {
            get => _eventName;
            set => _eventName = value.Trim();
        }

        /// <summary>
        /// id loại sự kiện
        /// </summary>
        public int EventTypeId { get; set; }

        /// <summary>
        /// Id nhà cung cấp sự kiện
        /// </summary>
        public int? SupplierId { get; set; }

        private string? _description;
        /// <summary>
        /// Mô tả
        /// </summary>
        [CustomMaxLength(1024)]
        public string? EventDescription
        {
            get => _description;
            set => _description = value?.Trim();
        }
        private string? _exchange;
        /// <summary>
        /// chính sách đổi trả
        /// </summary>
        [CustomMaxLength(1024)]
        public string? ExchangePolicy
        { 
            get => _exchange;
            set => _exchange = value?.Trim();
        }
        private string? _admission;

        /// <summary>
        /// Chính sách sự kiện
        /// </summary>
        [CustomMaxLength(1024)]
        public string? AdmissionPolicy 
        {
            get => _admission;
            set => _admission = value?.Trim(); 
        }
        /// <summary>
        /// ảnh sự kiện
        /// </summary>
        private string? _image;
        [CustomMaxLength(1024)]
        public string? EventImage
        { 
            get => _image;
            set => _image = value?.Trim();
        }
        
        public List<CreateEventDetailDto>? EventDetails { get; set; }
    }
}
