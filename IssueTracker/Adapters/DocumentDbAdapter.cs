using System;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;

namespace IssueTracker.Adapters
{
    interface IDocumentItem
    {
        [JsonProperty(PropertyName = "id")]
        string Id { get; }
    }

    class DocumentDbAdapter<T> where T : IDocumentItem
    {
        private readonly string _authKeyOrResourceToken = ConfigurationManager.AppSettings["authKey"];
        private readonly Uri _serviceEndpoint = new Uri(ConfigurationManager.AppSettings["endpoint"]);
        private const string DatabaseName = "IssueTracker";

        public async Task AddItem(T item, string collectionId)
        {
            using (var client = new DocumentClient(_serviceEndpoint, _authKeyOrResourceToken))
            {
                var collection = await DocumentCollection(collectionId, client);
                await client.CreateDocumentAsync(collection.SelfLink, item);
            }
        }

        public async Task<T> GetItem(string collectionId, Expression<Func<T, bool>> predicate)
        {
            T item;
            using (var client = new DocumentClient(_serviceEndpoint, _authKeyOrResourceToken))
            {
                var collection = await DocumentCollection(collectionId, client);
                item =
                    client
                        .CreateDocumentQuery<T>(collection.DocumentsLink)
                        .Where(predicate)
                        .AsEnumerable().FirstOrDefault();
            }
            return item;
        }

        public async Task UpdateItem(string collectionId, T item)
        {
            using (var client = new DocumentClient(_serviceEndpoint, _authKeyOrResourceToken))
            {
                var collection = await DocumentCollection(collectionId, client);
                var document = client.CreateDocumentQuery(collection.DocumentsLink).Where(doc => doc.Id == item.Id).AsEnumerable().FirstOrDefault();
                await client.ReplaceDocumentAsync(document.SelfLink, item);
            }
        }

        private static async Task<DocumentCollection> DocumentCollection(string collectionId, DocumentClient client)
        {
            var database = await DocumentDbDatabase.Get(client, DatabaseName);
            var collection = await DocumentDbCollection.Get(client, database, collectionId);
            return collection;
        }
    }
}