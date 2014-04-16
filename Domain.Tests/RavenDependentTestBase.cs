using NUnit.Framework;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests
{
    public abstract class RavenDependentTestBase
    {
        private static EmbeddableDocumentStore sEmbeddedStore;
        //private static DocumentStore sWrapperStore;

        [SetUp]
        public void Setup()
        {
            ResetDatabase();
            CreateIndexes(sEmbeddedStore);
        }

        /// <summary>
        /// The embedded database is very fast, but is not compatible with the Authorization or
        /// Unique Constraints bundle. Use this database store if the code under test does 
        /// not require authorization or unique constraints storage/checking.
        /// </summary>
        internal static EmbeddableDocumentStore StoreEmbedded
        {
            get
            {
                //EnsureStores();
                return sEmbeddedStore;
            }
        }

        ///// <summary>
        ///// The wrapper database around the embedded database is slower, but does provide support 
        ///// for the Authorization and Unique Constraints bundles. Use this database store if the 
        ///// code under test requires authorization or unique constraints storage/checking.
        ///// </summary>
        //internal static DocumentStore StoreWrapper
        //{
        //    get
        //    {
        //        //				EnsureStores();
        //        return sWrapperStore;
        //    }
        //}

        protected abstract void CreateIndexes(IDocumentStore currentStore);
        private static EmbeddableDocumentStore StartNewEmbeddableDocumentStore()
        {
            //int port = GetRandomUnusedPort();
            var documentStore =
                new EmbeddableDocumentStore
                {
                    RunInMemory = true,
                    //UseEmbeddedHttpServer = true,
                    //NOTE: by specifying localhost, and using an "ephemeral" port number,
                    // the HTTPListener used by RavenDB does not require a URL Reservation 
                    // so we don't have to go through a UAC prompt to set a reservation.
                    //Configuration = { Port = port, HostName = "localhost" },
                };
            return documentStore;
        }

        ///// <summary>
        ///// Find an unused "ephemeral" port number
        ///// </summary>
        ///// <returns></returns>
        //private static int GetRandomUnusedPort()
        //{
        //    // When you ask for port number 0, the OS assigns an unused ephemeral port number to you.
        //    var listener = new TcpListener(IPAddress.Any, 0);
        //    listener.Start();
        //    var port = ((IPEndPoint)listener.LocalEndpoint).Port;
        //    listener.Stop();
        //    return port;
        //}

        internal static void ResetDatabase()
        {
            // Let's explain the strangeness of these unit tests:
            //  We have to create two document stores here because the EmbeddableDocumentStore does not support 
            //  the Authorization bundle, so we create the embedded document store and then create a second
            //  document store and connect it to the Url of the first store so it can access the 
            //  Authorization bundle's extension methods.
            sEmbeddedStore = StartNewEmbeddableDocumentStore();
            DocumentStorePreparer.ConfigureDocumentStore(sEmbeddedStore);
            //sEmbeddedStore.SessionCreatedInternal += operations => operations.MaxNumberOfRequestsPerSession = 300;

            //sWrapperStore = new DocumentStore { Url = sEmbeddedStore.Configuration.ServerUrl };
            //DocumentStorePreparer.ConfigureDocumentStore(sWrapperStore);
            //sWrapperStore.SessionCreatedInternal += operations => operations.MaxNumberOfRequestsPerSession = 300;

            //new AllDocumentsById().Execute(sEmbeddedStore);

            //// Put in the core documents. TODO: refactor to use the same initialization code as the running system.
            //using (var session = StoreEmbedded.OpenSession())
            //{
            //    new RavenDbRepository<ProCoderSystem>(session).Add(ProCoderSystem.Create());
            //    new RavenDbRepository<WorkContextMutex>(session).Add(
            //        WorkContextMutex.Create(new MockDateTimeClock(new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc))));
            //    session.SaveChanges();
            //}
            //StoreEmbedded.WaitForIndexes();
        }
    }
    //public static class SessionTestinExtension
    //{
    //    /// <summary>
    //    /// Waits for all of the indexes in the store to become non-stale.
    //    /// </summary>
    //    /// <param name="store"></param>
    //    public static void WaitForIndexes(this IDocumentStore store)
    //    {
    //        //https://github.com/ravendb/ravendb/blob/master/Raven.Tests.Helpers/RavenTestBase.cs
    //        //Assert.True(SpinWait.SpinUntil(() => store.DatabaseCommands.GetStatistics().StaleIndexes.Length == 0, TimeSpan.FromSeconds(20)), "Waited too long for indexing to complete.");
    //    }
    //}

    //public class AllDocumentsById : AbstractIndexCreationTask
    //{
    //    public override IndexDefinition CreateIndexDefinition()
    //    {
    //        return
    //            new IndexDefinition
    //            {
    //                Name = "AllDocumentsById",
    //                Map = "from doc in docs let DocId = doc[\"@metadata\"][\"@id\"] select new {DocId};"
    //            };
    //    }
    //}

}
