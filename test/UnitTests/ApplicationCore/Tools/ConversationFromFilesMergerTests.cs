using ApplicationCore.Entities;
using ApplicationCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTests.ApplicationCore.Tools
{
    public class ConversationFromFilesMergerTests
    {
        private ConversationFromFilesMerger merger;

        public ConversationFromFilesMergerTests()
        {
            merger = new ConversationFromFilesMerger();
        }

        [Fact]
        public void Merge_FirstConversationIsASubsetOfSecondConversations()
        {
            var firstConversation = CreateBaseConversation(DateTime.Now);
            var secondConversation = CreateBaseConversation(DateTime.Now);
            var user = new User { Id = 3, Name = "Wojtek", Messages = new List<Message>(), Reactions = new List<Reaction>() };
            secondConversation.Users.Add(user);
            var message1 = new Message { User = user, Content = "test", CreationDate = DateTime.Now, Reactions = new List<Reaction>() };
            user.Messages.Insert(0, message1);
            secondConversation.Messages.Insert(0, message1);
            var message2 = new Message { User = user, Content = "test", CreationDate = DateTime.Now.AddYears(-2), Reactions = new List<Reaction>() };
            user.Messages.Add(message2);
            secondConversation.Messages.Add(message2);

            var result1 = merger.Merge(new List<Conversation> { firstConversation, secondConversation });

            Assert.Equal(secondConversation.Messages.Count, result1.Messages.Count);
            Assert.Equal(secondConversation.Users.Count, result1.Users.Count);
        }

        [Fact]
        public void Merge_NoOverlap()
        {
            var firstConversation = CreateBaseConversation(DateTime.Now);
            var secondConversation = CreateBaseConversation(DateTime.Now.AddYears(-2));
            var initialNumberOfMessages = firstConversation.Messages.Count + secondConversation.Messages.Count;
            var initialNumberOfReactions = firstConversation.Reactions.Count + secondConversation.Reactions.Count;
            var initialNumberOfUsers = firstConversation.Users.Count;

            var result1 = merger.Merge(new List<Conversation> { firstConversation, secondConversation });

            Assert.Equal(initialNumberOfUsers, result1.Users.Count);
            Assert.Equal(initialNumberOfMessages, result1.Messages.Count);
            Assert.Equal(initialNumberOfReactions, result1.Reactions.Count);
            Assert.False(DoesConversationContainsRepeatingNames(result1));
        }

        [Fact]
        public void Merge_Overlap()
        {
            var firstConversation = CreateBaseConversation(DateTime.Now);
            var secondConversation = CreateBaseConversation(DateTime.Now);

            secondConversation.DeleteMessage(secondConversation.Messages.FirstOrDefault());
            var user = new User { Id = 3, Name = "Wojtek", Messages = new List<Message>(), Reactions = new List<Reaction>() };
            secondConversation.Users.Add(user);
            var message = new Message { User = user, Content = "test", CreationDate = DateTime.Now.AddYears(-2), Reactions = new List<Reaction>() };
            user.Messages.Add(message);
            secondConversation.Messages.Add(message);
            var reaction = new Reaction { Message = firstConversation.Messages.First(), ReactionType = ReactionType.Dislike, User = user };
            firstConversation.Messages.First().Reactions.Add(reaction);
            firstConversation.Reactions.Add(reaction);
            user.Reactions.Add(reaction);


            var expectedNumberOfMessages = 22;
            var expectedNumberOfReactions = 11;
            var expectedNumberOfUsers = 4;

            var result1 = merger.Merge(new List<Conversation> { firstConversation, secondConversation });

            Assert.Equal(expectedNumberOfUsers, result1.Users.Count);
            Assert.Equal(expectedNumberOfMessages, result1.Messages.Count);
            Assert.Equal(expectedNumberOfReactions, result1.Reactions.Count);
            Assert.False(DoesConversationContainsRepeatingNames(result1));
        }


        private Conversation CreateBaseConversation(DateTime dateOfFirstMessage)
        {
            var conversation = new Conversation { Users = new List<User>(), Messages = new List<Message>(), Reactions = new List<Reaction>() };
            var user1 = new User { Id = 0, Name = "Andrzej", Messages = new List<Message>(), Reactions = new List<Reaction>() };
            var user2 = new User { Id = 1, Name = "Marcin", Messages = new List<Message>(), Reactions = new List<Reaction>() };
            var user3 = new User { Id = 2, Name = "Filip", Messages = new List<Message>(), Reactions = new List<Reaction>() };
            conversation.Users.AddRange(new List<User> { user1, user2, user3 });

            for (int i = 0; i < 21; i++)
            {
                var userIndex = i % conversation.Users.Count;
                var message = new Message { User = conversation.Users[userIndex], Content = $"{i}", CreationDate = dateOfFirstMessage.AddDays(-2 * (i + 1)), Reactions = new List<Reaction>() };
                conversation.Messages.Add(message);
                conversation.Users[userIndex].Messages.Add(message);
            }

            for (int i = 0; i < 10; i++)
            {
                var userIndex = i % conversation.Users.Count;
                var reaction = new Reaction { Message = conversation.Messages[i + 1], ReactionType = ReactionType.Angry, User = conversation.Users[userIndex] };
                conversation.Reactions.Add(reaction);
                conversation.Users[userIndex].Reactions.Add(reaction);
                conversation.Messages[i + 1].Reactions.Add(reaction);
            }

            return conversation;
        }

        private bool DoesConversationContainsRepeatingNames(Conversation conversation)
        {
            for (int i = 0; i < conversation.Users.Count - 1; i++)
            {
                for (int j = i + 1; j < conversation.Users.Count; j++)
                {
                    if (i == j) { continue; }
                    if (conversation.Users[i].Name == conversation.Users[j].Name)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
