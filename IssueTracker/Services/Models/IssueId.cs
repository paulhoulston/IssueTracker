using IssueTracker.Adapters;

namespace IssueTracker.Services.Models
{
    [DocumentCollectionId("IssueIds")]
    public class IssueId : IDocumentItem
    {
        public string Id { get; set; }
        public int CurrentIssueId { get; set; }
    }
}