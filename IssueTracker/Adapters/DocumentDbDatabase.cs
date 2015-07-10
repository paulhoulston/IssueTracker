using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace IssueTracker.Adapters
{
    static class DocumentDbDatabase
    {
        public static async Task<Database> Get(DocumentClient client, string databaseName)
        {
            return GetDatabase(client, databaseName) ?? await CreateDatabase(client, databaseName);
        }

        private static Database GetDatabase(DocumentClient client, string databaseName)
        {
            return client.CreateDatabaseQuery().Where(db => db.Id == databaseName).AsEnumerable().SingleOrDefault();
        }

        private static async Task<ResourceResponse<Database>> CreateDatabase(DocumentClient client, string databaseName)
        {
            return await client.CreateDatabaseAsync(new Database { Id = databaseName });
        }

    }
}