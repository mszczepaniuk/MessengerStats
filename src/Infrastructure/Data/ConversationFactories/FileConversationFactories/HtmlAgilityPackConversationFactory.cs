using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.ConversationFactories.FileConversationFactories
{
    public class HtmlAgilityPackConversationFactory : IFileConversationFactory
    {
        protected readonly IHtmlConstants htmlConstants;
        protected readonly IMessageConstants messageConstants;
        protected Conversation conversation = new Conversation();
        protected HtmlDocument htmlDocument = new HtmlDocument();

        public HtmlAgilityPackConversationFactory(
            IHtmlConstants htmlConstants,
            IMessageConstants messageConstants)
        {
            this.htmlConstants = htmlConstants;
            this.messageConstants = messageConstants;
            InitializeConversationProperties();
        }

        public virtual Conversation Create(string filePath)
        {
            htmlDocument.Load(filePath);
            SetTitle();
            var nodes = htmlDocument.DocumentNode.SelectNodes(htmlConstants.MessageNodeXPath);
            foreach (var node in nodes)
            {
                ProccessNode(node);
            }
            return conversation;
        }

        public virtual Conversation Create(List<string> filePaths)
        {
            for (int i = 0; i < filePaths.Count; i++)
            {
                htmlDocument.Load(filePaths[i]);
                if (i == 0) { SetTitle(); }
                var nodes = htmlDocument.DocumentNode.SelectNodes(htmlConstants.MessageNodeXPath);
                foreach (var node in nodes)
                {
                    ProccessNode(node);
                }
            }
            return conversation;
        }

        protected virtual void SetTitle()
        {
            var titleNode = htmlDocument.DocumentNode.SelectSingleNode(htmlConstants.TitleNodeXPath);
            var title = titleNode != null ? titleNode.InnerText : "Nie znaleziono";
            conversation.Title = title;
        }

        protected virtual void ProccessNode(HtmlNode node)
        {
            var nodeAuthor = GetAuthor(node);
            var nodeMessage = GetMessage(node);
            if (nodeAuthor == null || nodeMessage == null) { return; }

            nodeMessage.User = nodeAuthor;
            nodeAuthor.Messages.Add(nodeMessage);

            var nodeReactions = GetReactions(node);

            nodeMessage.Reactions = nodeReactions;
            nodeReactions.ForEach(x => x.Message = nodeMessage);
        }

        private User GetAuthor(HtmlNode node)
        {
            var nodeUsername = node.SelectSingleNode(htmlConstants.MessageUsernameNodeXPath);
            if (nodeUsername == null) { return null; }

            var username = string.IsNullOrEmpty(nodeUsername.InnerText) ? "Anonimowy Użytkownik" : nodeUsername.InnerText;
            var user = GetUserByUsername(username);

            return user;
        }

        private Message GetMessage(HtmlNode node)
        {
            var contentNode = node.SelectSingleNode(htmlConstants.MessageContentNodeXPath);
            if (contentNode == null || string.IsNullOrEmpty(contentNode.InnerText))
            {
                return null;
            }
            foreach (var regexFilter in messageConstants.MessageContentFilters)
            {
                if (regexFilter.IsMatch(contentNode.InnerText)) { return null; }
            }
            var content = contentNode.InnerText;

            var dateNode = node.SelectSingleNode(htmlConstants.MessageDateNodeXPath);
            if (dateNode == null || string.IsNullOrEmpty(dateNode.InnerText))
            {
                return null;
            }
            DateTime date;
            if (!DateTime.TryParse(dateNode.InnerText, out date))
            {
                return null;
            }

            var message = new Message
            {
                Content = content,
                Reactions = new List<Reaction>(),
                CreationDate = date
            };
            conversation.Messages.Add(message);

            return message;
        }

        private List<Reaction> GetReactions(HtmlNode node)
        {
            var reactionsNode = node.SelectSingleNode(htmlConstants.MessageReactionsNodeXPath);
            var reactionsList = new List<Reaction>();

            if (reactionsNode == null) { return reactionsList; }
            var reactionNodes = reactionsNode.SelectNodes("li");

            for (int i = 0; i < reactionNodes.Count; i++)
            {
                var nodeText = HtmlEntity.Entitize(reactionNodes[i].InnerText);
                ReactionType? reactionType = null;
                string username = null;
                foreach (var key in messageConstants.EmojiReactionTypesPairs.Keys)
                {
                    if (nodeText.Contains(key))
                    {
                        reactionType = messageConstants.EmojiReactionTypesPairs[key];
                        username = HtmlEntity.DeEntitize(nodeText.Replace(key, ""));
                        break;
                    }
                }

                if (reactionType == null) { continue; }
                if (username == null) { continue; }
                var user = GetUserByUsername(username);

                var reaction = new Reaction
                {
                    User = user,
                    ReactionType = reactionType.Value
                };

                reactionsList.Add(reaction);
                user.Reactions.Add(reaction);
            }

            conversation.Reactions.AddRange(reactionsList);
            return reactionsList;
        }

        private User GetUserByUsername(string username)
        {
            var user = conversation.Users.Where(x => x.Name == username).FirstOrDefault();

            if (user == null)
            {
                user = new User { Id = conversation.Users.Count, Name = username, Messages = new List<Message>(), Reactions = new List<Reaction>() };
                conversation.Users.Add(user);
            }
            return user;
        }

        private void InitializeConversationProperties()
        {
            conversation.Messages = new List<Message>();
            conversation.Reactions = new List<Reaction>();
            conversation.Users = new List<User>();
        }
    }
}