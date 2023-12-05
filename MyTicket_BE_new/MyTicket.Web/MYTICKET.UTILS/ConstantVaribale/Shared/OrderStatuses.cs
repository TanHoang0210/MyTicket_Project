using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.UTILS.ConstantVaribale.Shared
{
    public static class OrderStatuses
    {
        public const int INIT = 1;

        public const int READY_TO_PAY = 2;

        public const int PAYING = 3;

        public const int CANCEL = 4;

        public const int SUCCESS = 5;
    }
    public static class TransferStatuses
    {
        public const int INIT = 1;

        public const int READY_TO_TRANSFER = 2;

        public const int CANCEL = 3;

        public const int SUCCESS = 4;
    }
}
