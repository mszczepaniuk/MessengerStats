using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ApplicationCore.Tools
{
    public class ConversationMerger : IConversationMerger
    {
        protected Conversation firstConversation;
        protected Conversation secondConversation;
        protected DateTime overlapEarlierDate;
        protected DateTime overlapLaterDate;

        /// <summary>
        /// Merges all provided conversations into the first one in the list. Might modify provided conversations.
        /// </summary>
        /// <param name="conversations">Conversations to merge.</param>
        /// <returns>Merged conversation</returns>
        public virtual Conversation Merge(List<Conversation> conversations)
        {
            if (conversations.Count < 2)
            {
                throw new ArgumentException("At least 2 conversations are required in merging process.");
            }
            firstConversation = conversations.FirstOrDefault();

            for (int i = 1; i < conversations.Count; i++)
            {
                secondConversation = conversations[i];
                firstConversation = MergeTwoConversations();
            }

            return firstConversation;
        }

        protected virtual Conversation MergeTwoConversations()
        {
            if (IsSecondConversationASubsetOfFirstConversation())
            {
                return firstConversation;
            }

            if (IsFirstConversationASubsetOfSecondConversation())
            {
                return secondConversation;
            }

            if (IsThereAnOverlap())
            {
                ProcessOverlappedMessages();
            }

            EnsureFirstConversationIsYounger();
            firstConversation.Users.AddRange(secondConversation.Users);
            firstConversation.Messages.AddRange(secondConversation.Messages);
            firstConversation.Reactions.AddRange(secondConversation.Reactions);

            List<User[]> usersToMerge = GetUsersToMergeFromFirstConversation();

            foreach (var userPair in usersToMerge)
            {
                MergeUsers(userPair);
            }

            return firstConversation;
        }

        protected virtual void ProcessOverlappedMessages()
        {
            var overlappedMessages = secondConversation.Messages.Where(x => x.CreationDate > overlapLaterDate && x.CreationDate < overlapEarlierDate).ToList();

            foreach (var message in overlappedMessages)
            {
                secondConversation.DeleteMessage(message);
            }
        }

        private void MergeUsers(User[] userPair)
        {
            foreach (var reaction in userPair[1].Reactions)
            {
                reaction.User = userPair[0];
                userPair[0].Reactions.Add(reaction);
            }

            foreach (var message in userPair[1].Messages)
            {
                message.User = userPair[0];
                userPair[0].Messages.Add(message);
            }

            firstConversation.Users.Remove(userPair[1]);
        }

        private List<User[]> GetUsersToMergeFromFirstConversation()
        {
            var userPairs = new List<User[]>();
            for (int i = 0; i < firstConversation.Users.Count - 1; i++)
            {
                for (int j = i + 1; j < firstConversation.Users.Count; j++)
                {
                    if (firstConversation.Users[i].Name == firstConversation.Users[j].Name)
                    {
                        userPairs.Add(new User[] { firstConversation.Users[i], firstConversation.Users[j] });
                    }
                }
            }
            return userPairs;
        }

        private void EnsureFirstConversationIsYounger()
        {
            if (DateTime.Compare(GetDateOfFirstMessageInFirstConversation(), GetDateOfFirstMessageInSecondConversation()) > 0)
            {
                var tempConv = firstConversation;
                firstConversation = secondConversation;
                secondConversation = tempConv;
            }
        }

        private bool IsThereAnOverlap()
        {
            if ((DateTime.Compare(GetDateOfFirstMessageInFirstConversation(), GetDateOfFirstMessageInSecondConversation()) <= 0) &&
                (DateTime.Compare(GetDateOfFirstMessageInSecondConversation(), GetDateOfLastMessageInFirstConversation()) <= 0) &&
                (DateTime.Compare(GetDateOfLastMessageInFirstConversation(), GetDateOfLastMessageInSecondConversation()) <= 0))
            {
                overlapEarlierDate = GetDateOfFirstMessageInSecondConversation();
                overlapLaterDate = GetDateOfLastMessageInFirstConversation();
                return true;
            }
            else if ((DateTime.Compare(GetDateOfFirstMessageInFirstConversation(), GetDateOfFirstMessageInSecondConversation()) >= 0) &&
                     (DateTime.Compare(GetDateOfFirstMessageInSecondConversation(), GetDateOfLastMessageInFirstConversation()) >= 0) &&
                     (DateTime.Compare(GetDateOfLastMessageInFirstConversation(), GetDateOfLastMessageInSecondConversation()) >= 0))
            {
                overlapEarlierDate = GetDateOfFirstMessageInFirstConversation();
                overlapLaterDate = GetDateOfLastMessageInSecondConversation();
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsSecondConversationASubsetOfFirstConversation()
        {
            if ((DateTime.Compare(GetDateOfFirstMessageInFirstConversation(), GetDateOfFirstMessageInSecondConversation()) >= 0) &&
                (DateTime.Compare(GetDateOfLastMessageInFirstConversation(), GetDateOfLastMessageInSecondConversation()) <= 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsFirstConversationASubsetOfSecondConversation()
        {
            if ((DateTime.Compare(GetDateOfFirstMessageInFirstConversation(), GetDateOfFirstMessageInSecondConversation()) <= 0) &&
                (DateTime.Compare(GetDateOfLastMessageInFirstConversation(), GetDateOfLastMessageInSecondConversation()) >= 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private DateTime GetDateOfFirstMessageInFirstConversation()
        {
            return firstConversation.Messages.FirstOrDefault().CreationDate;
        }

        private DateTime GetDateOfLastMessageInFirstConversation()
        {
            return firstConversation.Messages.Last().CreationDate;
        }

        private DateTime GetDateOfFirstMessageInSecondConversation()
        {
            return secondConversation.Messages.FirstOrDefault().CreationDate;
        }

        private DateTime GetDateOfLastMessageInSecondConversation()
        {
            return secondConversation.Messages.Last().CreationDate;
        }
    }
}