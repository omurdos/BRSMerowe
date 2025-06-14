using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class CashAmanPaymentProcessor
    {

        private readonly string BaseUrl = "https://quickpay.sd/cpayment/exec";
        private readonly string CreateOrderEndPoint ;
        private readonly string GetOrderStatusEndPoint;
        private readonly string CheckOutEndPoint;

        CashAmanPaymentProcessor() { 
            CreateOrderEndPoint = $"{BaseUrl}/corder";
            GetOrderStatusEndPoint = $"{BaseUrl}/getOrderStatus";
            CheckOutEndPoint = $"{BaseUrl}/chechout";
        }

    }
}
