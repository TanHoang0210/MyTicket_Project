using MYTICKET.UTILS.RolePermission.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.UTILS.RolePermission
{
    public static class PermissionConfig
    {
        public static readonly Dictionary<string, PermissionContent> Configs = new()
        {
            { PermissionKeys.Dashboard, new(nameof(PermissionKeys.Dashboard), PermissionIcons.IconHome)},
            #region System Manager
            { PermissionKeys.SystemModule, new(nameof(PermissionKeys.SystemModule), PermissionIcons.IconUser)},
            //
            { PermissionKeys.RoleMenu, new(nameof(PermissionKeys.RoleMenu), PermissionIcons.IconDefault, PermissionKeys.SystemModule)},
            { PermissionKeys.RoleTable, new(nameof(PermissionKeys.RoleTable), PermissionIcons.IconDefault, PermissionKeys.RoleMenu) },
            { PermissionKeys.RoleCreate, new(nameof(PermissionKeys.RoleCreate), PermissionIcons.IconDefault, PermissionKeys.RoleMenu) },
            { PermissionKeys.RoleUpdate, new(nameof(PermissionKeys.RoleUpdate), PermissionIcons.IconDefault, PermissionKeys.RoleMenu) },
            { PermissionKeys.RoleDelete , new(nameof(PermissionKeys.RoleDelete), PermissionIcons.IconDefault, PermissionKeys.RoleMenu) },
            //
            { PermissionKeys.UserMenu, new(nameof(PermissionKeys.UserMenu), PermissionIcons.IconDefault, PermissionKeys.SystemModule)},
            { PermissionKeys.UserTable, new(nameof(PermissionKeys.UserTable), PermissionIcons.IconDefault, PermissionKeys.UserMenu) },
            { PermissionKeys.UserCreate, new(nameof(PermissionKeys.UserCreate), PermissionIcons.IconDefault, PermissionKeys.UserMenu) },
            { PermissionKeys.UserUpdate , new(nameof(PermissionKeys.UserUpdate), PermissionIcons.IconDefault, PermissionKeys.UserMenu) },
            { PermissionKeys.UserDelete , new(nameof(PermissionKeys.UserDelete), PermissionIcons.IconDefault, PermissionKeys.UserMenu) },
            { PermissionKeys.UserChangeStatus, new(nameof(PermissionKeys.UserChangeStatus), PermissionIcons.IconDefault, PermissionKeys.UserMenu) },

              //
            { PermissionKeys.CalendarMenu, new(nameof(PermissionKeys.CalendarMenu), PermissionIcons.IconDefault, PermissionKeys.SystemModule)},
            { PermissionKeys.CalendarTable, new(nameof(PermissionKeys.CalendarTable), PermissionIcons.IconDefault, PermissionKeys.CalendarMenu) },
            { PermissionKeys.CalendarUpdate , new(nameof(PermissionKeys.CalendarUpdate), PermissionIcons.IconDefault, PermissionKeys.CalendarMenu) },
           
            #endregion
        };
    }
}
