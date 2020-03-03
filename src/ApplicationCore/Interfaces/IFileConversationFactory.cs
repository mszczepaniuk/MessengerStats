using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ApplicationCore.Interfaces
{
    public interface IFileConversationFactory
    {
        public Conversation Create(string filePath);
        public Conversation Create(FileStream file);
    }
}
