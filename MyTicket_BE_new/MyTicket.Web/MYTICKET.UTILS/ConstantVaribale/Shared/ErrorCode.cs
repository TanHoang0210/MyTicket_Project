namespace MYTICKET.UTILS.ConstantVariables.Shared
{
    public enum ErrorCode
    {
        System = 1,
        BadRequest = 400,
        NotFound = 404,
        InternalServerError = 500,

        //user 1xxx
        UsernameOrPasswordIncorrect = 1000,
        UserNotFound = 1001,
        RoleNotFound = 1002,
        UserIsDeactive = 1003,
        InvalidUserType = 1004,
        UserOldPasswordIncorrect = 1005,
        UserNotHavePermission = 1006,

        //customer manager 2xxx
        CustomerNotFound = 2000,
        RestaurantNotFound = 2001,
        DeliveryAddressNotFound = 2002,
        GroupCustomerNotFound = 2003,
        BusinessCustomerIsBeingUsed = 2004,

        //Image && File 3xxx
        FileNotFound = 3000,
        FileMaxLength = 3001,
        FileExtention = 3002,

        //venue manager 4xxx
        VenueNotFound = 4002,


        #region Order 5xxx
        //Order
        OrderNotFound = 5000,
        OrderDetailNotFound = 5001,
        OrderDetailIsNotEmpty = 5002,
        OrderCannotCancel = 5003,
        OrderCannotOnDayOff = 5004,
        OrderCannotDelete = 5005,
        OrderDateCannotNull = 5006,
        #endregion

        //ShippingProvider 6xxx
        ShippingProviderNotFound = 6000,
        ShippingProviderIsExists= 6001,

        //Document 7xxx
        DocumentNotFound = 7000,

        //Event 800
        EventDetailNotFound = 8000,
        EventNotFound = 8001,

        TicketNotFound = 9000,
        TicketInvalid = 9001,
    }
}
