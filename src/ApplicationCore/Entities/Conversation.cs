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

        public void DeleteReaction(Reaction reaction)
        {
            Reactions.Remove(reaction);
            reaction.User.Reactions.Remove(reaction);
            reaction.Message.Reactions.Remove(reaction);
        }
    }
}