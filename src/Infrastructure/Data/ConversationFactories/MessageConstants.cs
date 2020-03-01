using ApplicationCore.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure.Data.ConversationFactories
{
    public class MessageConstants : IMessageConstants
    {
        public virtual List<Regex> MessageContentFilters => new List<Regex>
        {
            new Regex(@"(.*?)( usunął członka grupy\.)$"),
            new Regex(@"^(Opuściłeś\(aś\) grupę\.)$"),
            new Regex(@"(.*?)( usunął\(ęła\) Ciebie z grupy\.)$"),
            new Regex(@"(.*?) usunął użytkownika (.*?)( z grupy\.)$"),
            new Regex(@"(.*?) nadał\(a\) nazwę grupy (.*?)(\.)$"),
            new Regex(@"(.*?)( utworzył plan\.)$"),
            new Regex(@"(.*?) zmienił godzinę planu na (.*?)(\.)$"),
            new Regex(@"(.*?) zmienił lokalizację planu na (.*?)(\.)$"),
            new Regex(@"^(Twoja odpowiedź na wydarzenie )(.*?)"),
            new Regex(@"^(Odpowiedź )(.*?) na wydarzenie (.*?)"),
            new Regex(@"(.*?) nazwał plan: (.*?)(\.)$"),
            new Regex(@"^(Utworzyłeś przypomnienie: )(.*?)(\.)$"),
            new Regex(@"(.*?) dodał (.*?)(do grupy\.)$"),
            new Regex(@"^(Wysłałeś\(aś\) link\.)$"),
            new Regex(@"^(Wysłałeś\(aś\) załącznik\.)$"),
            new Regex(@"(.*?)( wysłał załącznik\.)$"),
            new Regex(@"(.*?)( wysłał link\.)$"),
        };

        public virtual Dictionary<string, ReactionType> EmojiReactionTypesPairs => new Dictionary<string, ReactionType>
        {
            { HtmlEntity.Entitize("❤"), ReactionType.Love },
            { HtmlEntity.Entitize("😆"), ReactionType.Haha },
            { HtmlEntity.Entitize("😢"), ReactionType.Sad },
            { HtmlEntity.Entitize("😠"), ReactionType.Angry },
            { HtmlEntity.Entitize("👍"), ReactionType.Like },
            { HtmlEntity.Entitize("👎"), ReactionType.Dislike },
            { HtmlEntity.Entitize("😮"), ReactionType.Wow },
            { HtmlEntity.Entitize("😍"), ReactionType.Love },
        };
    }
}
