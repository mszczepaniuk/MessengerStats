using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Conversation
    {
        public string Title { get; set; }
        public List<User> Users { get; set; }
        public List<Message> Messages { get; set; }
        public List<Reaction> Reactions { get; set; }

        /// <summary>
        /// Fully deletes message and it's reactions from conversation.
        /// </summary>
        /// <param name="message">Message to delete.</param>
        public void DeleteMessage(Message message)
        {
            Messages.Remove(message);
            message.User.Messages.Remove(message);
            foreach (var reaction in message.Reactions)
            {
                Reactions.Remove(reaction);
                reaction.User.Reactions.Remove(reaction);
            }
        }

        /// <summary>
        /// Fully deletes user and it's reactions and messages from conversation.
        /// </summary>
        /// <param name="message">Message to delete.</param>
        public void DeleteUser(User user)
        {
            Users.Remove(user);
            foreach (var message in user.Messages)
            {
                Messages.Remove(message);
                foreach (var reaction in message.Reactions)
                {
                    Reactions.Remove(reaction);
                    reaction.User.Reactions.Remove(reaction);
                }
            }
            foreach (var reaction in user.Reactions)
            {
                Reactions.Remove(reaction);
                reaction.Message.Reactions.Remove(reaction);
            }
        }

        /// <summary>
        /// Fully deletes reaction from conversation.
        /// </summary>
        /// <param name="message">Reaction to delete.</param>
        public void DeleteReaction(Reaction reaction)
        {
            Reactions.Remove(reaction);
            reaction.User.Reactions.Remove(reaction);
            reaction.Message.Reactions.Remove(reaction);
        }
    }
}