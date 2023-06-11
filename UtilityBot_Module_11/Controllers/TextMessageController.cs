using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UtilityBot_Module_11.Services;
using UtilityBot_Module_11.Models;

namespace UtilityBot_Module_11.Controllers
{
    internal class TextMessageController
    {

        private readonly ITelegramBotClient _telegramClient;
        private readonly ITextMessageHandler _textMessageHandler;
        private readonly IStorage _memoryStorage;

        public TextMessageController(ITelegramBotClient telegramBotClient, ITextMessageHandler textMessageHandler, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _textMessageHandler = textMessageHandler;
            _memoryStorage = memoryStorage;
        }
        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":
                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Посчитать количество символов" , "processMode1"),
                        InlineKeyboardButton.WithCallbackData($" Вычислить сумму чисел" , "processMode2")
                    });

                    // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $@"<b>  Бот может посчитать количество символов в сообщении " +
                        $@"или может вычислить сумму чисел.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}Отправьте сообщение.{Environment.NewLine}",
                        cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;

                default:
                    string userProcessMode = _memoryStorage.GetSession(message.Chat.Id).ProcessMode; // Здесь получим данные из сессии пользователя
                    var result = _textMessageHandler.Process(message.Text, userProcessMode); // Запустим обработку
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, result, cancellationToken: ct);
                    break;
            }
        }
    }
}
