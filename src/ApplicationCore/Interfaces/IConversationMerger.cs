using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Interfaces
{
    public interface IConversationMerger
    {
        /// <summary>
        /// Merges conversations created by separate files i.e. html splitted by facebook.
        /// Messages in conversations should be sorted from youngest to oldest.
        /// </summary>
        /// <param name="conversations">Conversations to merge.</param>
        /// <returns>Merged conversation</returns>
        public Conversation Merge(List<Conversation> conversations);
    }
}
