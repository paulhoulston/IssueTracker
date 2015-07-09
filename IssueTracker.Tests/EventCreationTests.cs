using System;
using NUnit.Framework;

namespace IssueTracker.Tests
{
    internal class Given_I_want_to_create_an_issue
    {
        private class When_I_create_an_issue : IssueCreationService.ICreateIssues
        {
            private readonly DateTime _testStartTime = DateTime.UtcNow;
            private IssueCreationService.Issue _issue;
            private const string CreatedBy = "Paul Houslton";

            public When_I_create_an_issue()
            {
                new IssueCreationService(this).CreateIssue(CreatedBy);
            }

            [Test]
            public void Then_the_issue_is_created()
            {
                Assert.IsNotNull(_issue);
            }

            [Test]
            public void And_the_creator_is_assigned_to_the_event()
            {
                Assert.AreEqual(CreatedBy, _issue.CreatedBy);
            }

            [Test]
            public void And_the_current_UTC_timestamp_is_added_to_the_event()
            {
                Assert.GreaterOrEqual(_issue.CreatedTime, _testStartTime);
            }

            [Test]
            public void And_the_ID_of_the_newly_created_event_is_returned()
            {
                Assert.AreEqual(1, _issue.Id);
            }

            public void CreateIssue(IssueCreationService.Issue issue)
            {
                _issue = issue;
            }
        }
    }

    internal class IssueCreationService
    {
        private readonly ICreateIssues _issueCreator;

        public interface ICreateIssues
        {
            void CreateIssue(Issue issue);
        }

        public class Issue
        {
            public string CreatedBy { get; set; }
            public DateTime CreatedTime { get; set; }
            public int Id { get; set; }
        }

        public IssueCreationService(ICreateIssues issueCreator)
        {
            _issueCreator = issueCreator;
        }

        public void CreateIssue(string createdBy)
        {
            _issueCreator.CreateIssue(new Issue{
               CreatedBy  = createdBy,
               CreatedTime = DateTime.UtcNow});
        }
    }
}