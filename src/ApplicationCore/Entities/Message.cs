using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ApplicationCore.Entities
{
    public class Message
    {
        public User User { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Reaction> Reactions { get; set; }
    }
}