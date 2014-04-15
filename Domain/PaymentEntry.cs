using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class PaymentEntry
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        public string WorkContextId { get; set; }

        public string WorkItemId { get; set; }

        public DateTime ReportDate { get; set; }

        public string PaymentReason { get; set; }
    }
}
