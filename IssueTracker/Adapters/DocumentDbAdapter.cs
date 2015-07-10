using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace IssueTracker.Adapters
{
    internal class DocumentDbAdapter<T> where T : class
    {
        private readonly string _authKeyOrResourceToken = ConfigurationManager.AppSettings["authKey"];
        private readonly Uri _serviceEndpoint = new Uri(ConfigurationManager.AppSettings["endpoint"]);
        private const string DatabaseName = "IssueTracker";

        public async Task AddItem(T item, string collectionId)
        {
            await PerformAction(collectionId, (documentClient, documentCollection) => documentClient.CreateDocumentAsync(documentCollection.SelfLink, item));
        }

        private async Task PerformAction(string collectionId, Func<DocumentClient, DocumentCollection, Task> actionToPerform)
        {
            using (var client = new DocumentClient(_serviceEndpoint, _authKeyOrResourceToken))
            {
                var database = GetDatabase(client) ?? await CreateDatabase(client);

                var collection = GetDocumentCollection(client, database, collectionId) ?? await CreateDocumentCollection(client, database, collectionId);

                await actionToPerform(client, collection);
            }
        }

        private static DocumentCollection GetDocumentCollection(DocumentClient client, Database database, string collectionId)
        {
            return client.CreateDocumentCollectionQuery(database.SelfLink)
                .Where(c => c.Id == collectionId)
                .AsEnumerable()
                .SingleOrDefault();
        }

        private static async Task<DocumentCollection> CreateDocumentCollection(DocumentClient client, Database database, string collectionId)
        {
            var collectionSpec = new DocumentCollection {Id = collectionId};
            var requestOptions = new RequestOptions {OfferType = "S1"};

            return await client.CreateDocumentCollectionAsync(database.SelfLink, collectionSpec, requestOptions);
        }

        private static Database GetDatabase(DocumentClient client)
        {
            return client.CreateDatabaseQuery().Where(db => db.Id == DatabaseName).AsEnumerable().SingleOrDefault();
        }

        private static async Task<ResourceResponse<Database>> CreateDatabase(DocumentClient client)
        {
            return await client.CreateDatabaseAsync(new Database {Id = DatabaseName});
        }
    }
}