using System;
using System.Collections.Generic;
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
    internal class DocumentCollectionIdAttribute : Attribute
    {
        public readonly string CollectionId;

        public DocumentCollectionIdAttribute(string collectionId)
        {
            CollectionId = collectionId;
        }
    }

    interface IDocumentItem
    {
        [JsonProperty(PropertyName = "id")]
        string Id { get; }
    }

    class DocumentDbAdapter<T> where T : class, IDocumentItem
    {
        private readonly string _authKeyOrResourceToken = ConfigurationManager.AppSettings["authKey"];
        private readonly Uri _serviceEndpoint = new Uri(ConfigurationManager.AppSettings["endpoint"]);
        private readonly string _collectionId;
        private const string DatabaseName = "IssueTracker";

        public DocumentDbAdapter()
        {
            var tType = typeof (T);
            var attr = Attribute.GetCustomAttribute(tType, typeof(DocumentCollectionIdAttribute)) as DocumentCollectionIdAttribute;
            _collectionId = attr == null
                ? tType.Name
                : attr.CollectionId;
        }

        public async Task AddItem(T item)
        {
            await PerformFunction((client, collection) => client.CreateDocumentAsync(collection.SelfLink, item));
        }

        public async Task UpdateItem(T item)
        {
            await PerformFunction(async (client, collection) =>
            {
                var document = client.CreateDocumentQuery(collection.DocumentsLink).Where(doc => doc.Id == item.Id).AsEnumerable().FirstOrDefault();
                await client.ReplaceDocumentAsync(document.SelfLink, item);
            });
        }

        public async Task<IEnumerable<T>> ListItems()
        {
            T[] items = null;
            await PerformAction((client, collection) => items = client.CreateDocumentQuery<T>(collection.DocumentsLink).AsEnumerable().ToArray());
            return items;
        }

        private async Task PerformFunction(Func<DocumentClient, DocumentCollection, Task> actionToPerform)
        {
            using (var client = new DocumentClient(_serviceEndpoint, _authKeyOrResourceToken))
            {
                var collection = await DocumentCollection(_collectionId, client);
                await actionToPerform(client, collection);
            }
        }
        private async Task PerformAction(Action<DocumentClient, DocumentCollection> actionToPerform)
        {
            using (var client = new DocumentClient(_serviceEndpoint, _authKeyOrResourceToken))
            {
                var collection = await DocumentCollection(_collectionId, client);
                actionToPerform(client, collection);
            }
        }

        public async Task GetItem(Expression<Func<T, bool>> predicate, Func<Task> onItemNotFound, Func<T, Task> onItemFound)
        {
            var item = await GetItem(predicate);
            if (item == null)
            {
                await onItemNotFound();
            }
            else
            {
                await onItemFound(item);
            }
        }

        private async Task<T> GetItem(Expression<Func<T, bool>> predicate)
        {
            T item = null;
            await PerformAction((client, collection) => item = client.CreateDocumentQuery<T>(collection.DocumentsLink).Where(predicate).AsEnumerable().FirstOrDefault());
            return item;
        }

        private static async Task<DocumentCollection> DocumentCollection(string collectionId, DocumentClient client)
        {
            var database = await DocumentDbDatabase.Get(client, DatabaseName);
            var collection = await DocumentDbCollection.Get(client, database, collectionId);
            return collection;
        }
    }
}