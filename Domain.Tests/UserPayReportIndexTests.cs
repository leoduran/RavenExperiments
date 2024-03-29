﻿using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;
using Raven.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.Tests
{
    public class UserPayReportIndexTests : RavenTestBase
    {
        [Fact]
        public void CanCreateDocument()
        {
            using (var store = NewDocumentStore())
            {
                new UserPayReportIndex().Execute(store);

                using (var session = store.OpenSession())
                {
                    var userPayRates = new List<UserPayRate>
                    {
                        new UserPayRate { UserId = "users/1", WorkContextId = "workcontexts/1", EffectiveDate = new DateTime(2012, 1, 1), PayRate = 1m, RateRule = "r1" }
                    };

                    userPayRates.ForEach(session.Store);

                    var paymentEntries = new List<PaymentEntry>
                    {
                        new PaymentEntry { UserId = "users/1", WorkContextId = "workcontexts/1", WorkItemId = "workitems/1", PaymentReason = "payroll", ReportDate = new DateTime(2014, 2, 1) },
                        new PaymentEntry { UserId = "users/1", WorkContextId = "workcontexts/1", WorkItemId = "workitems/2", PaymentReason = "payroll", ReportDate = new DateTime(2014, 2, 1) },
                        new PaymentEntry { UserId = "users/1", WorkContextId = "workcontexts/1", WorkItemId = "workitems/3", PaymentReason = "payroll", ReportDate = new DateTime(2014, 2, 1) },
                    };

                    paymentEntries.ForEach(session.Store);

                    session.SaveChanges();
                }

                Etag etag;
                using (var session = store.OpenSession())
                {
                    session.Advanced.UseOptimisticConcurrency = true;

                    var userPayRate = session.Load<UserPayRate>("userpayrates/users/1/workcontexts/1");
                    userPayRate.PayRate = 2m;

                    session.SaveChanges();

                    etag = session.Advanced.GetEtagFor(userPayRate);
                }

                List<UserPayReportIndex.Result> userPayReportEntries;

                using (var session = store.OpenSession())
                {
                    RavenQueryStatistics stats;
                    userPayReportEntries = session
                       .Query<UserPayReportIndex.Result, UserPayReportIndex>()
                       .Statistics(out stats)
                       .Customize(c => c.WaitForNonStaleResultsAsOf(etag))
                       .ProjectFromIndexFieldsInto<UserPayReportIndex.Result>()
                       .ToList();

                    Console.WriteLine(etag);
                    Console.WriteLine(stats.IndexEtag);

                    var userPayRate = session.Load<UserPayRate>("userpayrates/users/1/workcontexts/1");
                    Console.WriteLine(userPayRate.Etag + " - " + userPayRate.PayRate);
                }

                var upatedPaymentEntries = userPayReportEntries;

                // Assert
                Assert.True(upatedPaymentEntries.Count() == 3, "The count was incorrect");
                Assert.True(upatedPaymentEntries[0].Rate == 2, "The pay rate was incorrect");
            }
        }
    }
}
