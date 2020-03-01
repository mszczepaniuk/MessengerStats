using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace ApplicationCore.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Message> Messages { get; set; }
        public List<Reaction> Reactions { get; set; }
    }
}