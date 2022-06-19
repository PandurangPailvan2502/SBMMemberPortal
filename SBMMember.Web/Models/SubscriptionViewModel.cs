using System.Collections.Generic;

namespace SBMMember.Web.Models
{
    public class SubscriptionViewModel
    {

        public int Id { get; set; }
        public int Charges { get; set; }

        public List<SubscriptionViewModel> SubCharges { get; set; }
    }
}
