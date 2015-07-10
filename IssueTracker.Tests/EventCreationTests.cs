using System;
using System.Threading.Tasks;
using IssueTracker.Services;
using NUnit.Framework;

namespace IssueTracker.Tests
{
    internal class Given_I_want_to_create_an_issue
    {
        private class When_I_create_an_issue : IssueCreationService.ICreateIssues
        {
            private readonly DateTime _testStartTime = DateTime.UtcNow;
            private IssueCreationService.Issue _issue;
            private const int ExpectedIssueId = 1;
            private const string CreatedBy = "Paul Houslton";

            public When_I_create_an_issue()
            {
                Task.Run(async () => await new IssueCreationService(this).CreateIssue(CreatedBy));
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
                Assert.AreEqual(ExpectedIssueId, _issue.IssueId);
            }

            public Task CreateIssue(IssueCreationService.Issue issue)
            {
                _issue = issue;
                issue.IssueId = ExpectedIssueId;

                return new Task(() => { });
            }
        }
    }
}