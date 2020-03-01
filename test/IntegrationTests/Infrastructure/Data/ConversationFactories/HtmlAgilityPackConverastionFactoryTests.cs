using ApplicationCore.Entities;
using Infrastructure.Data.ConversationFactories;
using Infrastructure.Data.ConversationFactories.FileConversationFactories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace IntegrationTests.Infrastructure.Data.ConversationFactories
{
    public class HtmlAgilityPackConverastionFactoryTests
    {
        private readonly Conversation conversation;

        public HtmlAgilityPackConverastionFactoryTests()
        {
            var fakeHtmlPath = Path.Combine(GetTestFilesDirectoryPath(), "FakeHtml.html");  
            var htmlConstants = new HtmlConstants();
            var messageConstants = new MessageConstants();
            var factory = new HtmlAgilityPackConversationFactory(htmlConstants, messageConstants);
            conversation = factory.Create(fakeHtmlPath);
        }

        [Fact]
        public void FakeHtml_CorrectTitle()
        {
            Assert.Equal("Konwersacja", conversation.Title);
        }

        [Fact]
        public void FakeHtml_CorrectNumberOfMessages()
        {
            Assert.Equal(6, conversation.Messages.Count);
        }

        [Fact]
        public void FakeHtml_CorrectNumberOfUsers()
        {
            Assert.Equal(12, conversation.Users.Count);
        }

        [Fact]
        public void FakeHtml_CorrectOldestMessage()
        {
            var message = conversation.Messages.OrderBy(x => x.CreationDate).FirstOrDefault();
            Assert.Equal(2000, message.CreationDate.Year);
        }

        [Fact]
        public void FakeHtml_CorrectYoungestMessage()
        {
            var message = conversation.Messages.OrderByDescending(x => x.CreationDate).FirstOrDefault();
            var expecteDate = new DateTime(2020, 2, 29, 23, 06, 0);
            Assert.Equal(expecteDate, message.CreationDate);
        }

        [Fact]
        public void AnyHtml_SumOfUsersReactionsEqualToAllReactions()
        {
            var allReactions = conversation.Reactions.Count;
            int sumOfUsersReactions = 0;

            foreach (var user in conversation.Users)
            {
                sumOfUsersReactions += user.Reactions.Count;
            }

            Assert.Equal(allReactions, sumOfUsersReactions);
        }

        [Fact]
        public void AnyHtml_SumOfUsersMessagesEqualToAllMessages()
        {
            var allMessages = conversation.Messages.Count;
            int sumOfUsersMessages = 0;

            foreach (var user in conversation.Users)
            {
                sumOfUsersMessages += user.Messages.Count;
            }

            Assert.Equal(allMessages, sumOfUsersMessages);
        }

        [Fact]
        public void AnyHtml_SumOfMessagesReactionsEqualToAllReactions()
        {
            var allReactions = conversation.Reactions.Count;
            int sumOfMessaggesReactions = 0;

            foreach (var message in conversation.Messages)
            {
                sumOfMessaggesReactions += message.Reactions.Count;
            }

            Assert.Equal(allReactions, sumOfMessaggesReactions);
        }

        private string GetTestFilesDirectoryPath()
        {
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;
            return Path.Combine(projectDirectory, "TestFiles");
        }
    }
}
