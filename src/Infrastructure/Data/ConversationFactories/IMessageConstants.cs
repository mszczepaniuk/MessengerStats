using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.ConversationFactories
{
    public interface IMessageConstants
    {
        public List<Regex> MessageContentFilters { get; }
        public Dictionary<string, ReactionType> EmojiReactionTypesPairs { get;}
    }
}
