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
    }
}