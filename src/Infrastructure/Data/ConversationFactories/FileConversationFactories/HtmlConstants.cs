using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.ConversationFactories.FileConversationFactories
{
    public class HtmlConstants : IHtmlConstants
    {
        public virtual string TitleNodeXPath => "//div[@class='_3b0d']";
        public virtual string MessageNodeXPath => "//div[@class='pam _3-95 _2pi0 _2lej uiBoxWhite noborder']";

        public virtual string MessageDateNodeXPath => "div[@class='_3-94 _2lem']";

        public virtual string MessageContentNodeXPath => "div[@class='_3-96 _2let']//div//div[2]";

        public virtual string MessageUsernameNodeXPath => "div[@class='_3-96 _2pio _2lek _2lel']";
        public virtual string MessageReactionsNodeXPath => "div//ul[@class='_tqp']";
    }
}
