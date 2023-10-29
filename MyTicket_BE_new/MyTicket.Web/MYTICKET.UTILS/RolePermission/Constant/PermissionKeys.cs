namespace MYTICKET.UTILS.RolePermission.Constant
{
    /// <summary>
    /// PermissionKey
    /// </summary>
    public static class PermissionKeys
    {
        #region các loại permission
        private const string Web = "web_";
        private const string Menu = "menu_";
        private const string Tab = "tab_";
        private const string Page = "page_";
        private const string Table = "table_";
        private const string Form = "form_";
        private const string ButtonTable = "btn_table_";
        private const string ButtonForm = "btn_form_";
        private const string ButtonAction = "btn_action_";
        #endregion

        public const string Dashboard = $"{Page}dashboad";

        #region System Manager
        public const string SystemModule = $"system_";
        //role
        public const string RoleMenu = $"{SystemModule}{Menu}role_menu";
        public const string RoleTable = $"{SystemModule}{Table}role_menu_danh_sach";
        public const string RoleCreate = $"{SystemModule}{ButtonAction}role_menu_them_moi";
        public const string RoleUpdate = $"{SystemModule}{ButtonAction}role_menu_cap_nhat";
        public const string RoleDelete = $"{SystemModule}{ButtonAction}role_menu_xoa";
        //account
        public const string UserMenu = $"{SystemModule}{Menu}use_menu";
        public const string UserTable = $"{SystemModule}{Table}use_menu_danh_sach";
        public const string UserCreate = $"{SystemModule}{ButtonAction}use_menu_them_moi";
        public const string UserUpdate = $"{SystemModule}{ButtonAction}use_menu_cap_nhat";
        public const string UserDelete = $"{SystemModule}{ButtonAction}use_menu_xoa";
        public const string UserChangeStatus = $"{SystemModule}{ButtonAction}kich_hoat_or_huy";
        public const string UserSetPassword = $"{SystemModule}{ButtonAction}set_password";
        //calendar
        public const string CalendarMenu = $"{SystemModule}{Menu}calendar_menu";
        public const string CalendarTable = $"{SystemModule}{Table}calendar_menu_danh_sach";
        public const string CalendarUpdate = $"{SystemModule}{ButtonAction}calendar_menu_cap_nhat";
        #endregion
    }
}
