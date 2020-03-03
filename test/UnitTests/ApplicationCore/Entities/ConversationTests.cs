using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace UnitTests.ApplicationCore.Entities
{
    public class ConversationTests
    {
        private Conversation stubConversation;

        public ConversationTests()
        {
            stubConversation = CreateStubConversation();
        }

        [Fact]
        public void DeleteReaction_IsNotPresentInMessageReactions()
        {
            var reaction = stubConversation.Reactions.FirstOrDefault();
            stubConversation.DeleteReaction(reaction);

            Assert.DoesNotContain(reaction, reaction.Message.Reactions);
        }

        [Fact]
        public void DeleteReaction_IsNotPresentInUserReactions()
        {
            var reaction = stubConversation.Reactions.FirstOrDefault();
            stubConversation.DeleteReaction(reaction);

            Assert.DoesNotContain(reaction, reaction.User.Reactions);
        }

        [Fact]
        public void DeleteMessage_IsNotPresentInUserMessagges()
        {
            var message = stubConversation.Messages.FirstOrDefault();
            stubConversation.DeleteMessage(message);

            Assert.DoesNotContain(message, message.User.Messages);
        }

        [Fact]
        public void DeleteMessage_SumOfReferencesIsCorrect()
        {
            var message = stubConversation.Messages.FirstOrDefault();
            stubConversation.DeleteMessage(message);

            int userMessages = 0;
            foreach (var tempUser in stubConversation.Users)
            {
                userMessages += tempUser.Messages.Count;
            }
            Assert.Equal(stubConversation.Messages.Count, userMessages);
        }

        [Fact]
        public void DeleteUser_SumOfReferencesIsCorrect()
        {
            var user = stubConversation.Users.FirstOrDefault();
            stubConversation.DeleteUser(user);

            int userMessages = 0;
            foreach (var tempUser in stubConversation.Users)
            {
                userMessages += tempUser.Messages.Count;
            }
            Assert.Equal(stubConversation.Messages.Count, userMessages);

            int userReactions = 0;
            foreach (var tempUser in stubConversation.Users)
            {
                userReactions += tempUser.Reactions.Count;
            }
            Assert.Equal(stubConversation.Reactions.Count, userReactions);
        }

        private Conversation CreateStubConversation()
        {
            var conversation = new Conversation { Users = new List<User>(), Messages = new List<Message>(), Reactions = new List<Reaction>() };
            var user1 = new User { Id = 0, Name = "Andrzej", Messages = new List<Message>(), Reactions = new List<Reaction>() };
            var user2 = new User { Id = 1, Name = "Marcin", Messages = new List<Message>(), Reactions = new List<Reaction>() };
            var user3 = new User { Id = 2, Name = "Filip", Messages = new List<Message>(), Reactions = new List<Reaction>() };
            conversation.Users.AddRange(new List<User> { user1, user2, user3 });
            foreach (var user in conversation.Users)
            {
                var message = new Message { User = user, Content = "123", CreationDate = DateTime.Now, Reactions = new List<Reaction>() };
                conversation.Messages.Add(message);
                user.Messages.Add(message);
                message = new Message { User = user, Content = "123", CreationDate = DateTime.Now, Reactions = new List<Reaction>() };
                conversation.Messages.Add(message);
                user.Messages.Add(message);
            }

            var reaction = new Reaction { Message = conversation.Messages[0], ReactionType = ReactionType.Angry, User = user2 };
            conversation.Reactions.Add(reaction);
            user2.Reactions.Add(reaction);
            conversation.Messages[0].Reactions.Add(reaction);

            reaction = new Reaction { Message = conversation.Messages[3], ReactionType = ReactionType.Angry, User = user1 };
            conversation.Reactions.Add(reaction);
            user1.Reactions.Add(reaction);
            conversation.Messages[3].Reactions.Add(reaction);

            reaction = new Reaction { Message = conversation.Messages[5], ReactionType = ReactionType.Angry, User = user3 };
            conversation.Reactions.Add(reaction);
            user1.Reactions.Add(reaction);
            conversation.Messages[5].Reactions.Add(reaction);

            return conversation;
        }
    }
}
