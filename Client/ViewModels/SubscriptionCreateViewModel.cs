using BLL.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Client.ViewModels
{
    public class SubscriptionCreateViewModel
    {
        public string UserId { get; set; }
        public int UserBalance { get; set; }
        public Publisher Publisher { get; set; }
        public DateTime ExpirationDate { get; set; }
        [Required(ErrorMessage = "Please select the subscription period")]
        public string SubscriptionPeriod { get; set; }
        public int TotalPrice
        {
            get
            {
                return Publisher.MonthlySubscriptionPrice * CalcModelPeriod(SubscriptionPeriod);
            }
        }
        private int CalcModelPeriod(string subscriptionPeriod)
        {
            int coeff;

            switch (subscriptionPeriod)
            {
                case "month":
                    coeff = 1;
                    break;
                case "quarter":
                    coeff = 3;
                    break;
                case "half":
                    coeff = 6;
                    break;
                case "year":
                    coeff = 12;
                    break;
                default:
                    coeff = 1;
                    break;
            }

            return coeff;
        }
    }
}