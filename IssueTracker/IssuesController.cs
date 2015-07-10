using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IssueTracker.Services;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace IssueTracker
{
    public class IssuesController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }

    public class CreateIssueController : ApiController
    {
        public class Issue
        {
            public string CreatedBy { get; set; }    
        }

        [HttpPost, Route("Issues")]
        public HttpResponseMessage Post(Issue issue)
        {
            try
            {
                IssueCreationService.ICreateIssues repository = new IssueRepository();
                new IssueCreationService(repository).CreateIssue(issue.CreatedBy);

                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }        
    }

    public class IssueRepository : IssueCreationService.ICreateIssues
    {
        private readonly string _authKeyOrResourceToken = ConfigurationManager.AppSettings["authKey"];
        private readonly Uri _serviceEndpoint = new Uri(ConfigurationManager.AppSettings["endpoint"]);
        private const string DatabaseName = "IssueTracker";
        private const string CollectionId = "Issues";

        public void CreateIssue(IssueCreationService.Issue issue)
        {
            using (var client = new DocumentClient(_serviceEndpoint, _authKeyOrResourceToken))
            {
                var database = GetDatabase(client)
                    ?? CreateDatabase(client);
                
                var collection = GetDocumentCollection(client, database)
                    ?? CreateDocumentCollection(client, database);

                var res = client.CreateDocumentAsync(collection.SelfLink, issue).Result;
            }
        }

        private static DocumentCollection GetDocumentCollection(DocumentClient client, Database database)
        {
            return client.CreateDocumentCollectionQuery(database.SelfLink)
                .Where(c => c.Id == CollectionId)
                .AsEnumerable()
                .SingleOrDefault();
        }

        private static DocumentCollection CreateDocumentCollection(DocumentClient client, Database database)
        {
            var collectionSpec = new DocumentCollection {Id = CollectionId};
            var requestOptions = new RequestOptions {OfferType = "S1"};

            return client.CreateDocumentCollectionAsync(database.SelfLink, collectionSpec, requestOptions).Result;
        }

        private static Database GetDatabase(DocumentClient client)
        {
            return client.CreateDatabaseQuery().Where(db => db.Id == DatabaseName).AsEnumerable().SingleOrDefault();
        }

        private static ResourceResponse<Database> CreateDatabase(DocumentClient client)
        {
            return client.CreateDatabaseAsync(new Database { Id = DatabaseName }).Result;
        }
    }
}