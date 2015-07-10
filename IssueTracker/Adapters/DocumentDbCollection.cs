using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace IssueTracker.Adapters
{
    static class DocumentDbCollection
    {
        public static async Task<DocumentCollection> Get(DocumentClient client, Database database, string collectionId)
        {
            return GetDocumentCollection(client, database, collectionId) ?? await CreateDocumentCollection(client, database, collectionId);
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
            var collectionSpec = new DocumentCollection { Id = collectionId };
            var requestOptions = new RequestOptions { OfferType = "S1" };

            return await client.CreateDocumentCollectionAsync(database.SelfLink, collectionSpec, requestOptions);
        }
    }
}