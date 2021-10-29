using System.Collections.Generic;
using System.Threading.Tasks;
using bot.Entity;
using bot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace bot
{
    public class MessageBuilder
    {
        public static Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>> {
            {"En", new Dictionary<string, string> {
            {"locReq", "I need your location to function."},
            {"locRes", "Thank you. Location received!"},
            {"langRes", "Language successfully changed"},
            {"fajr", "Fajr"},
            {"sunrise", "Sunrise"},
            {"dhuhr", "Dhuhr"},
            {"asr", "Asr"},
            {"magrib", "Magrib"},
            {"isha", "Isha"},
            {"midNight", "Midnight"},
            {"sourse", "Source"},
            {"method", "Method"},
            {"share", "Share"},
            {"cancel", "Cancel"},
            {"today", "Today's Times"},
            {"tomorrow", "Tomorrow's Times"},
            {"tomorrow2", "1 min difference from yesterday"},
            {"reset", "Reset Location"},
            {"settings", "Settings"},
            {"menu", "Menu"},
            {"back", "Welcome Back "},
            {"zone", "TimeZone"}
            }},
            {"Ru", new Dictionary<string, string> {
            {"locReq", "Мне нужно ваше местоположение, чтобы функционировать."},
            {"locRes", "Спасибо. Местоположение получено!"},
            {"fajr", "Фаджр"},
            {"langRes", "Язык успешно изменен"},
            {"sunrise", "Восход"},
            {"dhuhr", "Зухр"},
            {"asr", "Аср"},
            {"magrib", "Магриб"},
            {"isha", "Иша"},
            {"midNight", "Полночь"},
            {"sourse", "Источник"},
            {"method", "Метод"},
            {"share", "Делиться"},
            {"cancel", "Отмена"},
            {"today", "Сегодняшние времена"},
            {"tomorrow", "Завтрашние времена"},
            {"tomorrow2", "1 мин разница со вчерашним днем"},
            {"reset", "Сбросить местоположение"},
            {"settings", "Настройки"},
            {"menu", "Меню"},
            {"back", "C возвращением "},
            {"zone", "часовой пояс"}
            }},
            {"Uz", new Dictionary<string, string> {
            {"locReq", "Bot ishlashi uchun joylashuvvingiz jonating."},
            {"locRes", "Rahmat. Joylashuv qabul qilindi!"},
            {"langRes", "Til muvoffaqqiyatli o'zgartirildi"},
            {"fajr", "Bomdod"},
            {"sunrise", "Quyosh chiqishi"},
            {"dhuhr", "Peshin"},
            {"asr", "Asr"},
            {"magrib", "Shom"},
            {"isha", "Xufton"},
            {"midNight", "Yarim tun"},
            {"sourse", "Manba"},
            {"method", "Usul"},
            {"share", "Ulashish"},
            {"cancel", "Bekor qilish"},
            {"today", "Bugungi vaqt"},
            {"tomorrow", "Ertangi vaqt"},
            {"tomorrow2", "Kechagidan 1 daqiqa farq"},
            {"reset", "Joylashuvni tiklash"},
            {"settings", "Sozlamalar"},
            {"menu", "Menyu"},
            {"back", "Qayta ko'rganimizdan hursandmiz "},
            {"zone", "Vaqt zonasi"}
            }},};
        public static async Task Handler(ITelegramBotClient client, Message message, ICacheService cache, IStorageService storage, BotUser user)
        {
            System.Console.WriteLine("hell\n");
            if(user.Latitude == 0 && user.Longitude == 0)
            {
                System.Console.WriteLine($"Location is zero\n\n {user.Language}");
                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"{dictionary[user.Language]["locReq"]}",
                    parseMode: ParseMode.Markdown,
                    replyMarkup: LocationRequestButton(user.Language)
                );
            }
            if(message.Location != null)
            {
                System.Console.WriteLine("\n\nLocation is not null\n\n");
                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    parseMode: ParseMode.Markdown,
                    text: dictionary[user.Language]["locRes"],
                    replyMarkup: MessageBuilder.MenuButton(user.Language));

                    //var user = await _storage.GetUserAsync(message.Chat.Id);
                    user.Longitude = message.Location.Longitude;
                    user.Latitude = message.Location.Latitude;
                    await storage.UpdateUserAsync(user);
            }
            if(message.Text == dictionary[user.Language]["today"])
            {
                System.Console.WriteLine("\n\ntoday\n\n");
                var b = "@muezzin_bot";
                var time = await cache.GetOrUpdatePrayerTimeAsync(message.Chat.Id, user.Longitude, user.Latitude);
                await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                parseMode: ParseMode.Markdown,
                text: @$"
***{dictionary[user.Language]["zone"]}***    {time.prayerTime.Timezone}
`{dictionary[user.Language]["fajr"]}`              `{time.prayerTime.Fajr}`
`{dictionary[user.Language]["sunrise"]}`        `{time.prayerTime.Sunrise}`
`{dictionary[user.Language]["dhuhr"]}`            `{time.prayerTime.Dhuhr}`
`{dictionary[user.Language]["asr"]}`                `{time.prayerTime.Asr}`
`{dictionary[user.Language]["magrib"]}`          `{time.prayerTime.Maghrib}`
`{dictionary[user.Language]["isha"]}`              `{time.prayerTime.Isha}`
`{dictionary[user.Language]["midNight"]}`      `{time.prayerTime.Midnight}`

`{dictionary[user.Language]["sourse"]}`    {time.prayerTime.Source}
***{dictionary[user.Language]["method"]}***    ___{time.prayerTime.CalculationMethod}___

***{b}***");
            }
            else if(message.Text == dictionary[user.Language]["tomorrow"])
            {
                System.Console.WriteLine("\n\ntomorrow\n\n");
                await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                parseMode: ParseMode.Markdown,
                text: dictionary[user.Language]["tomorrow2"]);
            }
            else if(message.Text == dictionary[user.Language]["settings"])
            {
                System.Console.WriteLine("\n\nsettigns\n\n");
                await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                parseMode: ParseMode.Markdown,
                text: $"`{dictionary[user.Language]["settings"]}`",
                replyMarkup: LanguageButton(user.Language));
            }
            else if(message.Text == dictionary[user.Language]["menu"])
            {
                System.Console.WriteLine("\n\nmenu\n\n");
                await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                parseMode: ParseMode.Markdown,
                text: $"`{dictionary[user.Language]["menu"]}`",
                replyMarkup: MenuButton(user.Language));
            }
            else if(message.Text == dictionary[user.Language]["reset"])
            {
                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: dictionary[user.Language]["locReq"],
                    parseMode: ParseMode.Markdown,
                    replyMarkup: MessageBuilder.LocationRequestButton(user.Language));
                await client.DeleteMessageAsync(
                    chatId: message.Chat.Id,
                    messageId: message.MessageId);
            }
            else if((message.Text == "En" || message.Text == "Uz" || message.Text == "Ru") && (user.Latitude != 0 && user.Longitude != 0))
            {
                System.Console.WriteLine("\n\nen uz ru\n\n");
                user.Language = message.Text;
                await storage.UpdateUserAsync(user);
                await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                parseMode: ParseMode.Markdown,
                text: $"`{dictionary[user.Language]["langRes"]}",
                replyMarkup: MenuButton(user.Language));
            }
            
            else if(user.Latitude != 0 && user.Longitude != 0)
            {
                System.Console.WriteLine("\n\nelse\n\n");
                await client.SendTextMessageAsync(
                chatId: message.Chat.Id,
                parseMode: ParseMode.Markdown,
                text: dictionary[user.Language]["menu"],
                replyMarkup: MessageBuilder.MenuButton(user.Language));
            }
        }
        public static ReplyKeyboardMarkup LocationRequestButton(string userLang)
        {
                return new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>()
                            {
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = dictionary[userLang]["share"], RequestLocation = true },
                                    new KeyboardButton(){ Text = dictionary[userLang]["cancel"] } 
                                }
                            },
                ResizeKeyboard = true};
        }
        
        public static ReplyKeyboardMarkup MenuButton(string userLang)
            => new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>()
                            {
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = dictionary[userLang]["today"]},
                                    new KeyboardButton(){ Text = dictionary[userLang]["reset"]},
                                },
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = dictionary[userLang]["tomorrow"]},
                                    new KeyboardButton(){ Text = dictionary[userLang]["settings"]}
                                }
                            },
                ResizeKeyboard = true
            };

        public static ReplyKeyboardMarkup LanguageButton(string userLang)
            => new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>()
                            {
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = "En"},
                                    new KeyboardButton(){ Text = "Ru"},
                                    new KeyboardButton(){ Text = "Uz" } 
                                },
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = dictionary[userLang]["menu"] } 
                                }
                            },
                ResizeKeyboard = true
            };
        
        public static ReplyKeyboardMarkup StartingLanguageButton()
            => new ReplyKeyboardMarkup()
            {
                Keyboard = new List<List<KeyboardButton>>()
                            {
                                new List<KeyboardButton>()
                                {
                                    new KeyboardButton(){ Text = "En"},
                                    new KeyboardButton(){ Text = "Ru"},
                                    new KeyboardButton(){ Text = "Uz" } 
                                }
                            },
                ResizeKeyboard = true
            };
    }
}