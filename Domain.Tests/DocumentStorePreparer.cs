using Raven.Client;
using Raven.Client.Document;
using Raven.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests
{
    public static class DocumentStorePreparer
    {
        public static void ConfigureDocumentStore(DocumentStoreBase documentStore)
        {
            documentStore.Initialize();

            //documentStore.DisableAggressiveCaching();
            //documentStore.Conventions.CustomizeJsonSerializer = serializer =>
            //{
            //    serializer.Converters.Add(new ReadOnlyCollectionConverter());
            //    var polymorphicContractResolver = new PolymorphicContractResolver(new DefaultRavenContractResolver(true));
            //    polymorphicContractResolver.AddDiscriminator(typeof(IReadOnlyCustomField), o => typeof(CustomField));
            //    serializer.ContractResolver = polymorphicContractResolver;
            //};

            //documentStore.RegisterListener(new DocumentConversionListener())
            //                         .RegisterListener(new UniqueConstraintsStoreListener());
            //documentStore.RegisterListener(new DocumentStoreListener());

        }

        //public static void EnsureAndEnableSqlReplication(DocumentStoreBase documentStore)
        //{
        //    EnsureSqlReplicationBundleIsActive(documentStore);
        //    var resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
        //    var replicationFiles =
        //        resourceNames
        //            .Where(
        //                n =>
        //                n.ToLowerInvariant().Contains(".Resources".ToLowerInvariant())
        //                && n.EndsWith(".json", StringComparison.InvariantCultureIgnoreCase))
        //            .ToList();
        //    if (!replicationFiles.Any())
        //    {
        //        throw new InvalidOperationException("Unable to load SqlReplication json files: missing resources");
        //    }

        //    using (var session = documentStore.OpenSession())
        //    {
        //        foreach (var path in replicationFiles)
        //        {
        //            AddSqlReplicationDocIfMissing(session, path);
        //        }

        //        session.SaveChanges();
        //    }
        //}

        //private static void AddSqlReplicationDocIfMissing(IDocumentSession session, string replicationDocResourcePath)
        //{
        //    var nameEndsAt = replicationDocResourcePath.LastIndexOf('.');
        //    var nameStartsAt = replicationDocResourcePath.LastIndexOf('.', nameEndsAt - 1);
        //    string filename = replicationDocResourcePath.Substring(nameStartsAt, nameEndsAt - nameStartsAt);
        //    var docId = "Raven/SqlReplication/Configuration/" + filename;

        //    var doc = session.Load<RavenJObject>(docId);
        //    if (doc == null)
        //    {
        //        using (
        //            var rdr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(replicationDocResourcePath)))
        //        {
        //            var fileContents = rdr.ReadToEnd();
        //            doc = RavenJObject.Parse(fileContents);
        //        }
        //        session.Store(doc, docId);
        //        session.Advanced.GetMetadataFor(doc)["Raven-Entity-Name"] = "SqlReplicationConfigs";
        //    }
        //}

        //private static void EnsureSqlReplicationBundleIsActive(DocumentStoreBase documentStore)
        //{
        //    string databaseName = ((DocumentStore)documentStore).DefaultDatabase;
        //    string rootUrl = documentStore.Url;

        //    var docStore = new DocumentStore { Url = rootUrl };
        //    docStore.Initialize();
        //    docStore.DatabaseCommands.EnsureDatabaseExists(databaseName);
        //    using (var session = docStore.OpenSession())
        //    {
        //        session.Advanced.UseOptimisticConcurrency = true;
        //        var dbDoc = session.Load<RavenJObject>("Raven/Databases/" + databaseName);
        //        var settings = dbDoc["Settings"].Value<RavenJObject>();
        //        var activeBundles = settings["Raven/ActiveBundles"] ?? "";
        //        var bundles = activeBundles.Value<string>()
        //            .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
        //            .Select(x => x.Trim())
        //            .ToArray();
        //        if (!bundles.Contains("SqlReplication"))
        //        {
        //            var newActiveBundles = string.Join(
        //                ";",
        //                bundles.Concat(new[] { "SqlReplication" }).ToArray());
        //            settings["Raven/ActiveBundles"] = newActiveBundles;
        //            session.Store(dbDoc);
        //            session.SaveChanges();
        //        }
        //    }
        //}

    }
}
