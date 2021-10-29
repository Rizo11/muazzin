using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using bot.Entity;
using bot.HttpClients;
using bot.Services;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace bot
{
    public class Handlers
    {
        private readonly ILogger<Handlers> _logger;
        private readonly IStorageService _storage;
        private readonly ICacheService _cache;

        public Handlers(
            ILogger<Handlers> logger,
            IStorageService storage, 
            ICacheService cache)
        {
            _logger = logger;
            _storage = storage;
            _cache = cache;
        }

        public Task HandleErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken ctoken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException => $"Error occured with Telegram Client: {exception.Message}",
                _ => exception.Message
            };

            _logger.LogCritical(errorMessage);

            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken ctoken)
        {
            var handler = update.Type switch
            {
                UpdateType.Message => BotOnMessageReceived(client, update.Message),
                UpdateType.EditedMessage => BotOnMessageEdited(client, update.EditedMessage),
                UpdateType.CallbackQuery => BotOnCallbackQueryReceived(client, update.CallbackQuery),
                UpdateType.InlineQuery => BotOnInlineQueryReceived(client, update.InlineQuery),
                UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(client, update.ChosenInlineResult),
                _ => UnknownUpdateHandlerAsync(client, update)
            };

            try
            {
                await handler;
            }
            catch(Exception e)
            {

            }
        }

        private async Task BotOnMessageEdited(ITelegramBotClient client, Message editedMessage)
        {
            throw new NotImplementedException();
        }

        private async Task UnknownUpdateHandlerAsync(ITelegramBotClient client, Update update)
        {
            throw new NotImplementedException();
        }

        private async Task BotOnChosenInlineResultReceived(ITelegramBotClient client, ChosenInlineResult chosenInlineResult)
        {
            throw new NotImplementedException();
        }

        private async Task BotOnInlineQueryReceived(ITelegramBotClient client, InlineQuery inlineQuery)
        {
            throw new NotImplementedException();
        }

        private async Task BotOnCallbackQueryReceived(ITelegramBotClient client, CallbackQuery callbackQuery)
        {
            throw new NotImplementedException();
        }

        private async Task BotOnMessageReceived(ITelegramBotClient client, Message message)
        {
            var user = await _storage.GetUserAsync(message.Chat.Id);
            if(message.Text == "/start")
            {
                //first time user
                if(!await _storage.ExistsAsync(message.Chat.Id))
                {
                    user = new BotUser(
                        chatId: message.Chat.Id,
                        username: message.From.Username,
                        fullname: $"{message.From.FirstName} {message.From.LastName}",
                        longitude: 0,
                        latitude: 0,
                        address: string.Empty,
                        language: string.Empty);

                    var result = await _storage.InsertUserAsync(user);

                    if(result.IsSuccess)
                    {
                        _logger.LogInformation($"New user added: {message.Chat.Id}");
                    }
                    await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    parseMode: ParseMode.Markdown,
                    text: "Language/Язык/Til",
                    replyMarkup: MessageBuilder.StartingLanguageButton());
                }
                //if user exists
                else
                {
                    // var user = await _storage.GetUserAsync(message.Chat.Id);
                    _logger.LogInformation($"{user.Username}\n{user.Language}\n{user.Latitude}");

                    await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    parseMode: ParseMode.Markdown,
                    text: MessageBuilder.dictionary[user.Language]["back"]);

                    await MessageBuilder.Handler(client, message, _cache, _storage, user);
                }
            }
            //after the start
            else
            {
                if(message.Text == "En")
                {
                    user.Language = "En";
                    await _storage.UpdateUserAsync(user);
                    await MessageBuilder.Handler(client, message, _cache, _storage, user);
                }
                else if(message.Text == "Ru")
                {
                    user.Language = "Ru";
                    await _storage.UpdateUserAsync(user);
                    await MessageBuilder.Handler(client, message, _cache, _storage, user);
                }
                else if(message.Text == "Uz")
                {    
                    user.Language = "Uz";
                    await _storage.UpdateUserAsync(user);
                    await MessageBuilder.Handler(client, message, _cache, _storage, user);
                }
                else if (message.Location != null)
                {
                    //location update
                    user.Longitude = message.Location.Longitude;
                    user.Latitude = message.Location.Latitude;
                    await _storage.UpdateUserAsync(user);

                    await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    parseMode: ParseMode.Markdown,
                    text: MessageBuilder.dictionary[user.Language]["locRes"],
                    replyMarkup: MessageBuilder.MenuButton(user.Language));
                }
                else
                {
                    _logger.LogInformation($"{user.Username}\n{user.Language}\n{user.Latitude}");
                    await MessageBuilder.Handler(client, message, _cache, _storage, user);
                }
            }
        }
    }
}