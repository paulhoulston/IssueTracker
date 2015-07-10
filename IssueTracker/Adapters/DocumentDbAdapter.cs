using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace IssueTracker.Adapters
{
    class DocumentDbAdapter<T> where T : class
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
                var database = await DocumentDbDatabase.Get(client, DatabaseName);
                var collection = await DocumentDbCollection.Get(client, database, collectionId);

                await actionToPerform(client, collection);
            }
        }
    }
}