using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.ConversationFactories.FileConversationFactories
{
    public interface IHtmlConstants
    {
        /// <summary>
        /// Absolute XPath to title node.
        /// </summary>
        public string TitleNodeXPath { get; }
        /// <summary>
        /// Absolute XPath to message nodes.
        /// </summary>
        public string MessageNodeXPath { get; }
        /// <summary>
        /// XPath to date node relative to message node.
        /// </summary>
        public string MessageDateNodeXPath { get; }
        /// <summary>
        /// XPath to content node relative to message node.
        /// </summary>
        public string MessageContentNodeXPath { get; }
        /// <summary>
        /// XPath to username node relative to message node.
        /// </summary>
        public string MessageUsernameNodeXPath { get; }
        /// <summary>
        /// XPath to reactions node relative to message node.
        /// </summary>
        public string MessageReactionsNodeXPath { get; }
    }
}
