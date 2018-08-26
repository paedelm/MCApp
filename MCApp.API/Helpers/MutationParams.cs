using System;

namespace MCApp.API.Helpers
{
    public class MutationParams
    {
        public const double cMinAmount = -999999; 
        public const double cMaxAmount = 999999; 
        private const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize;}
            set { pageSize = value > maxPageSize ? maxPageSize : value;}
        }
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public double MinAmount { get; set; } = cMinAmount;
        public double MaxAmount { get; set; } = cMaxAmount;

        
    }
}