using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ClothesShopApp.Business
{
    public class SmsService
    {
        public SmsService() 
        {
            var accountSid = "AC150648c0ae405518331d9e53bc6a5d0f";
            var authToken = "7464d6e3c38b11cc880fb5669372f14b";
            TwilioClient.Init(accountSid, authToken);
        }

        public async Task SendSms(string phoneNumber, string message)
        {
            var to = new PhoneNumber("+90"+phoneNumber);
            var from = new PhoneNumber("+13045214327"); // Twilio’dan aldığınız numara
            var sms = MessageResource.Create(
                to: to,
                from: from,
                body: message
            );
        }
        public string GenerateResetCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}
