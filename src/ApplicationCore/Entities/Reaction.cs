using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Reaction
    {
        public Message Message { get; set; }
        public User User { get; set; }
        public ReactionType ReactionType { get; set; }
    }
}
