using System;
using IssueTracker.Adapters;

namespace IssueTracker.Services.Models
{
    [DocumentCollectionId("Issues")]
    public class Issue : IDocumentItem
    {
        public string Id { get; private set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public int IssueId { get; set; }
    }
}