using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ConsoleApp19
{
    class Program
    {
        private static string apiKey = "3yNgNJqg3XitDAnjLP2ujPYVGsKMzilR58wiPgjz";
        private static bool highLevelSelected = false;
        private static bool mediumLevelSelected = false;
        private static bool economicLevelSelected = false;
        private static bool isHouseSelected = false;
        private static bool isGarageSelected = false;

        static void Main(string[] args)
        {
            var client = new TelegramBotClient("7113826476:AAFrnnEzaD3INsx8khxacYZESucKC4coqWs");

            client.StartReceiving(Update, Error);

            Console.ReadLine();
        }

        async static Task Update(ITelegramBotClient botclient, Update update, CancellationToken token)
        {
            var message = update.Message;
            if (message != null)
            {
                Console.WriteLine($"{message.Chat.FirstName} | {message.Text}");
                if (message.Text != null)
                {
                    if (message.Text.ToLower().Contains("/start") || message.Text.ToLower().Contains("повернутися на початок"))
                    {
                        await botclient.SendTextMessageAsync(message.Chat.Id, "Вітаю! Цей бот створений для пошуку нерухомості у Львові! Натисніть кнопку, щоб розпочати.", replyMarkup: new ReplyKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                new KeyboardButton("Розпочати"),
                                new KeyboardButton("Інформація по ID"),
                                new KeyboardButton("Повернутися на початок")
                            },
                             new[]
                            {
                                new KeyboardButton("Середня ціна")
                            }
                        })
                        {
                            ResizeKeyboard = true
                        });
                        return;
                    }
                    if (message.Text.ToLower().Contains("розпочати"))
                    {
                        await botclient.SendTextMessageAsync(message.Chat.Id, "Вас цікавить будинок, квартира чи гараж?", replyMarkup: new ReplyKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                new KeyboardButton("Будинок"),
                                new KeyboardButton("Квартира"),
                                new KeyboardButton("Гараж")
                            },
                            new[]
                            {
                                new KeyboardButton("Повернутися на початок")
                            }
                        })
                        {
                            ResizeKeyboard = true
                        });
                        return;
                    }
                    if (message.Text.ToLower().Contains("будинок"))
                    {
                        isHouseSelected = true;
                        isGarageSelected = false;
                        await botclient.SendTextMessageAsync(message.Chat.Id, "Який у вас грошовий діапазон?", replyMarkup: new ReplyKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                new KeyboardButton("Високий"),
                                new KeyboardButton("Середній"),
                                new KeyboardButton("Економ")
                            },
                            new[]
                            {
                                new KeyboardButton("Повернутися на початок")
                            }
                        })
                        {
                            ResizeKeyboard = true
                        });
                        return;
                    }
                    if (message.Text.ToLower().Contains("квартира"))
                    {
                        isHouseSelected = false;
                        isGarageSelected = false;
                        await botclient.SendTextMessageAsync(message.Chat.Id, "Який у вас грошовий діапазон?", replyMarkup: new ReplyKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                new KeyboardButton("Високий"),
                                new KeyboardButton("Середній"),
                                new KeyboardButton("Економ")
                            },
                            new[]
                            {
                                new KeyboardButton("Повернутися на початок")
                            }
                        })
                        {
                            ResizeKeyboard = true
                        });
                        return;
                    }
                    if (message.Text.ToLower().Contains("гараж"))
                    {
                        isHouseSelected = false;
                        isGarageSelected = true;
                        await botclient.SendTextMessageAsync(message.Chat.Id, "Який у вас грошовий діапазон?", replyMarkup: new ReplyKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                new KeyboardButton("Високий"),
                                new KeyboardButton("Середній"),
                                new KeyboardButton("Економ")
                            },
                            new[]
                            {
                                new KeyboardButton("Повернутися на початок")
                            }
                        })
                        {
                            ResizeKeyboard = true
                        });
                        return;
                    }
                    if (message.Text.ToLower().Contains("інформація по id"))
                    {
                        await botclient.SendTextMessageAsync(message.Chat.Id, "Будь ласка, введіть ID оголошення.");
                        return;
                    }
                    if (long.TryParse(message.Text, out long realtyId))
                    {
                        await GetRealtyInfoById(botclient, message.Chat.Id, realtyId);
                        return;
                    }
                    if (message.Text.ToLower().Contains("високий") || message.Text.ToLower().Contains("середній") || message.Text.ToLower().Contains("економ"))
                    {
                        highLevelSelected = message.Text.ToLower().Contains("високий");
                        mediumLevelSelected = message.Text.ToLower().Contains("середній");
                        economicLevelSelected = message.Text.ToLower().Contains("економ");
                        await botclient.SendTextMessageAsync(message.Chat.Id, "Який район Львова бажаєте обрати?", replyMarkup: new ReplyKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                new KeyboardButton("Галицький"),
                                new KeyboardButton("Личаківський"),
                                new KeyboardButton("Сихівський")
                            },
                            new[]
                            {
                                new KeyboardButton("Франківський"),
                                new KeyboardButton("Шевченківський"),
                                new KeyboardButton("Залізничний")
                            },
                            new[]
                            {
                                new KeyboardButton("Повернутися на початок")
                            }
                        })
                        {
                            ResizeKeyboard = true
                        });
                        return;
                    }
                    if (message.Text.ToLower().Contains("галицький") || message.Text.ToLower().Contains("личаківський") || message.Text.ToLower().Contains("сихівський") || message.Text.ToLower().Contains("франківський") || message.Text.ToLower().Contains("шевченківський") || message.Text.ToLower().Contains("залізничний"))
                    {
                        string selectedDistrict = message.Text;
                        await botclient.SendTextMessageAsync(message.Chat.Id, $"Ви обрали район: {selectedDistrict}. Шукаємо {(isHouseSelected ? "будинки" : isGarageSelected ? "гаражі" : "квартири")}...");

                        string districtCode = GetDistrictCode(selectedDistrict);

                        if (isHouseSelected)
                        {
                            await SearchForHouses(botclient, message.Chat.Id, districtCode);
                        }
                        else if (isGarageSelected)
                        {
                            await SearchForGarages(botclient, message.Chat.Id, districtCode);
                        }
                        else
                        {
                            await SearchForApartments(botclient, message.Chat.Id, districtCode);
                        }
                        return;
                    }
                    if (message.Text.ToLower().Contains("середня ціна"))
                    {
                        await GetAveragePrice(botclient, message.Chat.Id);
                        return;
                    }
                    else
                    {
                        await botclient.SendTextMessageAsync(message.Chat.Id, "Вітаю, який у вас грошовий діапазон?", replyMarkup: new ReplyKeyboardMarkup(new[]
                        {
                            new[]
                            {
                                new KeyboardButton("Високий"),
                                new KeyboardButton("Середній"),
                                new KeyboardButton("Економ")
                            },
                            new[]
                            {
                                new KeyboardButton("Повернутися на початок")
                            }
                        })
                        {
                            ResizeKeyboard = true
                        });
                        return;
                    }
                }
                if (message.Photo != null)
                {
                    await botclient.SendTextMessageAsync(message.Chat.Id, "На жаль, у нас немає пошуку по фото.");
                    return;
                }
            }
        }

        private static async Task SearchForHouses(ITelegramBotClient botclient, long chatId, string districtCode)
        {
            string apiUrl = "https://developers.ria.com/dom/search";
            string queryParams = $"?api_key={"3yNgNJqg3XitDAnjLP2ujPYVGsKMzilR58wiPgjz"}&category=4&city_id=5&district_id={districtCode}&operation=1";

            if (highLevelSelected)
            {
                queryParams += "&price_from=100000&price_cur=1";
            }
            else if (mediumLevelSelected)
            {
                queryParams += "&price_from=60000&price_cur=1";
            }
            else if (economicLevelSelected)
            {
                queryParams += "&price_from=10000&price_cur=1";
            }

            Console.WriteLine($"Requesting houses with URL: {apiUrl + queryParams}"); // Debug logging

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiUrl + queryParams);
                var content = await response.Content.ReadAsStringAsync();

                var json = JObject.Parse(content);
                var items = json["items"];

                if (items != null && items.Count() > 0)
                {
                    long firstHouseId = items.First.Value<long>();
                    string houseUrl = $"https://dom.ria.com/uk/realty-{firstHouseId}.html";
                    await botclient.SendTextMessageAsync(chatId, $"Посилання на будинок: {houseUrl}");
                }
                else
                {
                    await botclient.SendTextMessageAsync(chatId, "На жаль, будинків не знайдено.");
                }
            }
        }

        private static async Task SearchForApartments(ITelegramBotClient botclient, long chatId, string districtCode)
        {
            string apiUrl = "https://developers.ria.com/dom/search";
            string queryParams = $"?api_key={"3yNgNJqg3XitDAnjLP2ujPYVGsKMzilR58wiPgjz"}&category=1&city_id=5&district_id={districtCode}&operation=1";

            if (highLevelSelected)
            {
                queryParams += "&price_from=100000&price_cur=1";
            }
            else if (mediumLevelSelected)
            {
                queryParams += "&price_from=60000&price_cur=1";
            }
            else if (economicLevelSelected)
            {
                queryParams += "&price_from=10000&price_cur=1";
            }

            Console.WriteLine($"Requesting apartments with URL: {apiUrl + queryParams}"); // Debug logging

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiUrl + queryParams);
                var content = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"API Response: {content}"); // Debug logging

                var json = JObject.Parse(content);
                var items = json["items"];

                if (items != null && items.Count() > 0)
                {
                    var apartmentIds = items.Take(2).Select(item => item.Value<long>()).ToList();
                    if (apartmentIds.Count == 1)
                    {
                        string apartmentUrl = $"https://dom.ria.com/uk/realty-{apartmentIds[0]}.html";
                        await botclient.SendTextMessageAsync(chatId, $"Посилання на квартиру: {apartmentUrl}");
                    }
                    else if (apartmentIds.Count >= 2)
                    {
                        string apartmentUrl1 = $"https://dom.ria.com/uk/realty-{apartmentIds[0]}.html";
                        string apartmentUrl2 = $"https://dom.ria.com/uk/realty-{apartmentIds[1]}.html";
                        await botclient.SendTextMessageAsync(chatId, $"Посилання на квартиру 1: {apartmentUrl1}\nПосилання на квартиру 2: {apartmentUrl2}");
                    }
                }
                else
                {
                    await botclient.SendTextMessageAsync(chatId, "На жаль, квартир не знайдено.");
                }
            }
        }

        private static async Task SearchForGarages(ITelegramBotClient botclient, long chatId, string districtCode)
        {
            string apiUrl = "https://developers.ria.com/dom/search";
            string queryParams = $"?api_key={"3yNgNJqg3XitDAnjLP2ujPYVGsKMzilR58wiPgjz"}&category=30&city_id=5&district_id={districtCode}&operation=1  ";

            if (highLevelSelected)
            {
                queryParams += "&price_from=500&price_cur=3";
            }
            else if (mediumLevelSelected)
            {
                queryParams += "&price_from=100&price_cur=3";
            }
            else if (economicLevelSelected)
            {
                queryParams += "&price_from=50&price_cur=3";
            }

            Console.WriteLine($"Requesting garages with URL: {apiUrl + queryParams}"); // Debug logging

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiUrl + queryParams);
                var content = await response.Content.ReadAsStringAsync();

                var json = JObject.Parse(content);
                var items = json["items"];

                if (items != null && items.Count() > 0)
                {
                    var garageIds = items.Take(2).Select(item => item.Value<long>()).ToList();
                    if (garageIds.Count == 1)
                    {
                        string garageUrl = $"https://dom.ria.com/uk/realty-{garageIds[0]}.html";
                        await botclient.SendTextMessageAsync(chatId, $"Посилання на гараж: {garageUrl}");
                    }
                    else if (garageIds.Count >= 2)
                    {
                        string garageUrl1 = $"https://dom.ria.com/uk/realty-{garageIds[0]}.html";
                        string garageUrl2 = $"https://dom.ria.com/uk/realty-{garageIds[1]}.html";
                        await botclient.SendTextMessageAsync(chatId, $"Посилання на гараж 1: {garageUrl1}\nПосилання на гараж 2: {garageUrl2}");
                    }
                }
                else
                {
                    await botclient.SendTextMessageAsync(chatId, "На жаль, гаражів не знайдено.");
                }
            }
        }

        private static async Task GetRealtyInfoById(ITelegramBotClient botclient, long chatId, long realtyId)
        {
            string apiUrl = $"https://developers.ria.com/dom/info/{realtyId}?api_key={"3yNgNJqg3XitDAnjLP2ujPYVGsKMzilR58wiPgjz"}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiUrl);
                var json = await response.Content.ReadAsStringAsync();
                var result = JObject.Parse(json);

                string realtyInfo = $"ID: {realtyId}\n" +
                                    $"Тип: {result["realty_type_name"]}\n" +
                                    $"Ціна: {result["price"]} {result["currency_type"]}\n" +
                                    $"Опис: {result["description"]}\n" +
                                    $"Адреса: {result["address"]}\n" +
                                    $"Контакт: {result["contact"]}";

                await botclient.SendTextMessageAsync(chatId, realtyInfo);
            }
        }

        private static async Task GetAveragePrice(ITelegramBotClient botclient, long chatId)
        {
            string apiUrl = "https://developers.ria.com/dom/average_price";
            string queryParams = $"?api_key={"3yNgNJqg3XitDAnjLP2ujPYVGsKMzilR58wiPgjz"}&category=1&sub_category=2&operation=1&state_id=1&city_id=1&date_from=2023-06&date_to=2023-07";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiUrl + queryParams);
                var json = await response.Content.ReadAsStringAsync();
                var result = JObject.Parse(json);

                string averagePriceInfo = "Середня ціна по районах Львова:\n\n";

                foreach (var district in result)
                {
                    averagePriceInfo += $"{district.Key}: {district.Value} USD\n";
                }

                await botclient.SendTextMessageAsync(chatId, averagePriceInfo);
            }
        }

        private static Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine($"Помилка: {exception.Message}");
            return Task.CompletedTask;
        }

        private static string GetDistrictCode(string district)
        {
            switch (district.ToLower())
            {
                case "галицький":
                    return "20169";
                case "личаківський":
                    return "17764";
                case "сихівський":
                    return "17769";
                case "франківський":
                    return "17781";
                case "шевченківський":
                    return "15771";
                case "залізничний":
                    return "17765";
                default:
                    return "0";
            }
        }
    }
}
