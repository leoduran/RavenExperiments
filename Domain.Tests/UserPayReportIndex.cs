using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests
{
    public class UserPayReportIndex : AbstractIndexCreationTask<PaymentEntry, UserPayReportIndex.Result>
    {
        public UserPayReportIndex()
        {
            Map = paymentEntries =>
                     from paymentEntry in paymentEntries
                     let userRate =
                        LoadDocument<UserPayRate>("userpayrates/" + paymentEntry.UserId + "/"
                                                               + paymentEntry.WorkContextId)

                     select new Result
                     {
                         UserId = paymentEntry.UserId,
                         WorkContextId = paymentEntry.WorkContextId,
                         Rate = userRate.PayRate,
                         RateRule = userRate.RateRule,
                         ReportDate = paymentEntry.ReportDate
                     };

            Stores.Add(x => x.UserId, FieldStorage.Yes);
            Stores.Add(x => x.WorkContextId, FieldStorage.Yes);
            Stores.Add(x => x.Rate, FieldStorage.Yes);
            Stores.Add(x => x.RateRule, FieldStorage.Yes);
            Stores.Add(x => x.ReportDate, FieldStorage.Yes);
        }

        public class Result
        {
            public string UserId { get; set; }
            public string WorkContextId { get; set; }
            public decimal Rate { get; set; }
            public string RateRule { get; set; }
            public DateTime ReportDate { get; set; }
        }
    }
}
