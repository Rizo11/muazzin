using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace bot
{
    public class MessageBuilder
    {
        public static ReplyKeyboardMarkup LocationRequestButton()
            => new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>()
                            {
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = "Share", RequestLocation = true },
                                    new KeyboardButton(){ Text = "Cancel" } 
                                }
                            },
                ResizeKeyboard = true
            };
        
        public static ReplyKeyboardMarkup MenuButton()
            => new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>()
                            {
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = "Bugun vaqt"},
                                    new KeyboardButton(){ Text = "Location o'zgartirish"},
                                },
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = "Ertangi vaqt"},
                                    new KeyboardButton(){ Text = "Sozlamalar"}
                                }
                            },
                ResizeKeyboard = true
            };

        public static ReplyKeyboardMarkup Language()
            => new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>()
                            {
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = "Eng"},
                                    new KeyboardButton(){ Text = "Uz" } 
                                },
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = "Menu" } 
                                }
                            },
                ResizeKeyboard = true
            };
    }
}