using MYTICKET.BASE.SERVICE.Common.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.AuthModule.Dtos.UserDto
{
    public class ChangePasswordDto
    {
        private string _oldPassword;
        [CustomMaxLength(128)]
        [CustomRequired(AllowEmptyStrings = false)]
        public string OldPassword
        {
            get => _oldPassword;
            set => _oldPassword = value?.Trim();
        }

        private string _newPassword;
        [CustomMaxLength(128)]
        [CustomRequired(AllowEmptyStrings = false)]
        public string NewPassword
        {
            get => _newPassword;
            set => _newPassword = value?.Trim();
        }
    }
}
