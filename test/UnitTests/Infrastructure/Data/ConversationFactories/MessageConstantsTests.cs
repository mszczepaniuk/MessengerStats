using ApplicationCore.Entities;
using HtmlAgilityPack;
using Infrastructure.Data.ConversationFactories;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTests
{
    public class MessageConstantsTests
    {
        private MessageConstants constants;

        public MessageConstantsTests()
        {
            constants = new MessageConstants();
        }

        [Theory]
        [InlineData("Opuściłeś(aś) grupę.")]
        [InlineData("User1 usunął członka grupy.")]
        [InlineData("User1 User1 usunął członka grupy.")]
        [InlineData("User usunął(ęła) Ciebie z grupy.")]
        [InlineData("User User usunął(ęła) Ciebie z grupy.")]
        [InlineData("User1 usunął użytkownika User200 z grupy.")]
        [InlineData("User User usunął użytkownika User2 User2 z grupy.")]
        [InlineData("User User nadał(a) nazwę grupy Nowa Nazwa Grupy.")]
        [InlineData("User User utworzył plan.")]
        [InlineData("User1 zmienił godzinę planu na pt., 28 lut o 18:22.")]
        [InlineData("User1 zmienił lokalizację planu na lokalizacja.")]
        [InlineData("User1 dodał User200 do grupy.")]
        [InlineData("Twoja odpowiedź na wydarzenie Plan: Wezmę udział")]
        [InlineData("Odpowiedź User1 na wydarzenie Plan: Nie mogę wziąć udziału")]
        [InlineData("User2 nazwał plan: Plan.")]
        [InlineData("Utworzyłeś przypomnienie: Plan.")]
        [InlineData("Wysłałeś(aś) link.")]
        [InlineData("Wysłałeś(aś) załącznik.")]
        [InlineData("User1 wysłał załącznik.")]
        [InlineData("User1 wysłał link.")]
        public void MessageContentFilters_IsMatch(string messageContent)
        {
            bool messageCaught = false;
            foreach (var regex in constants.MessageContentFilters)
            {
                if (messageCaught = regex.IsMatch(messageContent))
                {
                    break;
                }
            }

            Assert.True(messageCaught);
        }

        [Theory]
        [InlineData("Generic message")]
        [InlineData("User1usunął użytkownika User200 z grupy.")]
        [InlineData("User1 zmienił lokalizację planu na lokalizacja")]
        [InlineData("User User utworzył plan")]
        [InlineData("User1 zmienił godzinę planu na pt., 28 lut o 18:22")]
        [InlineData("User User utworzył nowy plan.")]
        [InlineData("User1 dodał User200 do tej grupy.")]
        [InlineData("User1dodał User200 do grupy.")]
        [InlineData("User2 nazwał plan: Plan")]
        [InlineData("Utworzyłeś przypomnienie: Plan")]
        [InlineData("Czy Wysłałeś(aś) link.")]
        [InlineData("Czy Wysłałeś(aś) załącznik.")]
        [InlineData("Wysłałeś(aś) link. ?")]
        [InlineData("Wysłałeś(aś) załącznik. ?")]
        [InlineData("Ty Opuściłeś(aś) grupę.")]
        [InlineData("Opuściłeś(aś) grupę. ?")]
        public void MessageContentFilters_IsNotMatch(string messageContent)
        {
            bool messageCaught = false;
            foreach (var regex in constants.MessageContentFilters)
            {
                if (messageCaught = regex.IsMatch(messageContent))
                {
                    break;
                }
            }

            Assert.False(messageCaught);
        }
    }
}