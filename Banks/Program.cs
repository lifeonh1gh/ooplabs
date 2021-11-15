using Banks.Models.Accounts;
using Banks.Models.Time;
using Banks.Repositories;
using Banks.Services;
using Spectre.Console;

namespace Banks
{
    internal static class Program
    {
        private static void Main()
        {
            var banksRep = new BanksRepository();
            var clientsRep = new ClientsRepository();
            var accountsRep = new AccountsRepository();
            var transactionsRep = new TransactionsRepository();
            var time = new FutureTime();

            var bankService = new BankService(banksRep, clientsRep, accountsRep, transactionsRep, time);
            bool alive = true;
            while (alive)
            {
#pragma warning disable 618
                var action = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Какое действие хотите выполнить[/]?")
                        .PageSize(17)
                        .AddChoices(new[]
                        {
                            "Создать банк", "Создать клиента", "Создать счет",
                            "Пополнить счет", "Снять со счета", "Перевести на другой счет",
                            "Отобразить все банки", "Отобразить всех клиентов", "Отобразить все счета",
                            "Отобразить все транзакции", "Зачисление процента на остаток",
                            "Вычет комиссии по кредитному счету",
                            "Добавить данные у клиента", "Изменить условия банка", "Проверить баланс",
                            "Управление транзакциями", "Выйти из приложения",
                        }));
                switch (action)
                {
                    case "Создать банк":
                        var ruleВ = new Rule("[red]Добавление банка[/]");
                        AnsiConsole.Render(ruleВ);
                        string bankName = AnsiConsole.Ask<string>("[green]Введите название банка[/]:");
                        int suspectLimit = AnsiConsole.Ask<int>("[green]Введите лимит для подозрительного счета[/]:");
                        int creditLimit = AnsiConsole.Ask<int>("[green]Введите лимит для кредитного счета[/]:");
                        int commission = AnsiConsole.Ask<int>("[green]Введите комиссию для кредитного счета[/]:");
                        int periodDays = AnsiConsole.Ask<int>("[green]Введите срок действия для депозита[/]:");
                        int percent = AnsiConsole.Ask<int>("[green]Введите процент на остаток[/]:");
                        bankService.CreateBank(bankName, suspectLimit, creditLimit, commission, periodDays, percent);
                        break;
                    case "Создать клиента":
                        var ruleС = new Rule("[red]Добавление клиента[/]");
                        AnsiConsole.Render(ruleС);
                        string clientName = AnsiConsole.Ask<string>("[green]Введите имя клиента[/]:");
                        string clientSurname = AnsiConsole.Ask<string>("[green]Введите фамилию клиента[/]:");
                        var passport = AnsiConsole.Prompt(
                            new TextPrompt<string>("[green]Введите паспорт клиента[/]:")
                                .AllowEmpty());
                        var address = AnsiConsole.Prompt(
                            new TextPrompt<string>("[green]Введите адресс клиента[/]:")
                                .AllowEmpty());
                        bankService.CreateClient(clientName, clientSurname, passport, address);
                        break;
                    case "Создать счет":
                        var ruleA = new Rule("[red]Добавление счета[/]");
                        AnsiConsole.Render(ruleA);
                        int bank = AnsiConsole.Ask<int>("[green]Введите Id банка, в котором создать счет[/]:");
                        int client = AnsiConsole.Ask<int>("[green]Введите Id клиента, владельца счета[/]:");
                        double sum = AnsiConsole.Ask<double>("[green]Введите сумму для создания счета[/]:");
                        var type = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[green]Выберите тип счета[/]:")
                                .PageSize(3)
                                .AddChoices(new[]
                                {
                                    "Дебетовый", "Депозит", "Кредитный",
                                }));
                        if (type == "Дебетовый")
                        {
                            bankService.CreateAccount(bank, client, AccountType.DebitAccount, sum);
                        }
                        else if (type == "Депозит")
                        {
                            bankService.CreateAccount(bank, client, AccountType.DepositAccount, sum);
                        }
                        else if (type == "Кредитный")
                        {
                            bankService.CreateAccount(bank, client, AccountType.CreditAccount, sum);
                        }

                        break;
                    case "Пополнить счет":
                        var ruleP = new Rule("[red]Транзакция пополнения[/]");
                        AnsiConsole.Render(ruleP);
                        int accountP =
                            AnsiConsole.Ask<int>("[green]Введите Id счета, который необходимо пополнить[/]:");
                        double sumP = AnsiConsole.Ask<double>("[green]Введите сумму для пополнения[/]:");
                        bankService.Put(accountP, sumP);
                        time.NextDay();
                        break;
                    case "Снять со счета":
                        var ruleW = new Rule("[red]Транзакция снятия[/]");
                        AnsiConsole.Render(ruleW);
                        int accountW =
                            AnsiConsole.Ask<int>("[green]Введите Id аккаунта, с которого необходимо снять[/]:");
                        double sumW = AnsiConsole.Ask<double>("[green]Введите сумму для снятия[/]:");
                        bankService.Withdraw(accountW, sumW);
                        time.NextDay();
                        break;
                    case "Перевести на другой счет":
                        var ruleT = new Rule("[red]Транзакция перевода[/]");
                        AnsiConsole.Render(ruleT);
                        int accountOne =
                            AnsiConsole.Ask<int>("[green]Введите Id счета, с которого необходимо перевести[/]:");
                        int accountTwo =
                            AnsiConsole.Ask<int>("[green]Введите Id счета, на который необходимо перевести[/]:");
                        double sumT = AnsiConsole.Ask<double>("[green]Введите сумму для перевода[/]:");
                        bankService.Transfer(accountOne, accountTwo, sumT);
                        time.NextDay();
                        break;
                    case "Отобразить все банки":
                        var ruleBs = new Rule("[blue]Все добавленные банки[/]");
                        AnsiConsole.Render(ruleBs);
                        var banks = banksRep.GetBanks();
                        var tableBank = new Table();
                        tableBank.AddColumns("Идентификатор", "Название", "Лимит для подозрительного счета", "Кредитный лимит", "Кредитная комиссия", "Срок действия депозита");
                        foreach (var b in banks)
                        {
                            tableBank.AddRow($"{b.Id}", $"{b.Name}", $"{b.SuspectLimit}", $"{b.CreditLimit}", $"{b.Commission}", $"{b.PeriodDays}");
                        }

                        AnsiConsole.Render(tableBank.Centered());
                        break;
                    case "Отобразить всех клиентов":
                        var ruleCs = new Rule("[blue]Все добавленные клиенты[/]");
                        AnsiConsole.Render(ruleCs);
                        var clients = clientsRep.GetClients();
                        var tableClient = new Table();
                        tableClient.AddColumns("Идентификатор", "Имя", "Фамилия", "Паспортные данные", "Адресс");
                        foreach (var c in clients)
                        {
                            tableClient.AddRow($"{c.Id}", $"{c.Name}", $"{c.Surname}", $"{c.Passport}", $"{c.Address}");
                        }

                        AnsiConsole.Render(tableClient.Centered());
                        break;
                    case "Отобразить все счета":
                        var ruleAs = new Rule("[blue]Все добавленные счета[/]");
                        AnsiConsole.Render(ruleAs);
                        var accounts = accountsRep.GetAccounts();
                        var tableAccount = new Table();
                        tableAccount.AddColumns("Идентификатор счета", "Идентификатор банка", "Идентификатор клиента");
                        foreach (var a in accounts)
                        {
                            tableAccount.AddRow($"{a.AccountId}", $"{a.BankId}", $"{a.ClientId}");
                        }

                        AnsiConsole.Render(tableAccount.Centered());
                        break;
                    case "Отобразить все транзакции":
                        var ruleTs = new Rule("[blue]Список всех транзакций[/]");
                        AnsiConsole.Render(ruleTs);
                        var transactions = transactionsRep.GetTransactions();
                        var tableTransaction = new Table();
                        tableTransaction.AddColumns("Идентификатор транзакции");
                        foreach (var t in transactions)
                        {
                            tableTransaction.AddRow($"{t.Id}");
                        }

                        AnsiConsole.Render(tableTransaction.Centered());
                        break;
                    case "Зачисление процента на остаток":
                        int accountPerc =
                            AnsiConsole.Ask<int>(
                                "[green]Введите Id аккаунта, на который зачислить процент на остаток[/]:");
                        bankService.PercentOnBalance(accountPerc);
                        bankService.Time.NextDay();
                        break;
                    case "Вычет комиссии по кредитному счету":
                        int accountCom = AnsiConsole.Ask<int>("[green]Введите Id аккаунта, для вычета комиссии[/]:");
                        bankService.CreditCommission(accountCom);
                        break;
                    case "Добавить данные у клиента":
                        var ruleD = new Rule("[red]Добавление данных клиента[/]");
                        AnsiConsole.Render(ruleD);
                        var clientData = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[green]Какие данные хотите добавить?[/]:")
                                .PageSize(3)
                                .AddChoices(new[]
                                {
                                    "Паспорт", "Адрес",
                                }));
                        if (clientData == "Паспорт")
                        {
                            int clientDataId =
                                AnsiConsole.Ask<int>("[green]Введите Id клиента, у которого добавить пасспорт[/]:");
                            string clientPassport = AnsiConsole.Ask<string>("[green]Введите паспортные данные[/]:");
                            bankService.AddPassportToClient(clientDataId, clientPassport);
                        }
                        else if (clientData == "Адрес")
                        {
                            int clientDataId2 =
                                AnsiConsole.Ask<int>("[green]Введите Id клиента, у которого добавить адресс[/]:");
                            string clientAddress = AnsiConsole.Ask<string>("[green]Введите адресс[/]:");
                            bankService.AddAddressToClient(clientDataId2, clientAddress);
                        }

                        break;
                    case "Изменить условия банка":
                        var ruleB = new Rule("[red]Изменение условий банка[/]");
                        AnsiConsole.Render(ruleB);
                        var bankCondition = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[green]Какой пункт условия хотите изменить?[/]:")
                                .PageSize(3)
                                .AddChoices(new[]
                                {
                                    "Комиссия", "Лимит для подозрительного счета",
                                }));
                        if (bankCondition == "Лимит для подозрительного счета")
                        {
                            int bankCId =
                                AnsiConsole.Ask<int>("[green]Введите Id банка, у которого изменить условие лимита[/]:");
                            int newLimit =
                                AnsiConsole.Ask<int>("[green]Введите новый лимит для подозрительного счета[/]:");
                            bankService.UpdateSuspectLimit(bankCId, newLimit);
                        }

                        break;
                    case "Проверить баланс":
                        var ruleCheck = new Rule("[blue]Информация о счете[/]");
                        AnsiConsole.Render(ruleCheck);
                        int accountCheck =
                            AnsiConsole.Ask<int>("[green]Введите Id аккаунта, чтобы получить информацию[/]:");
                        bankService.PercentOnBalance(accountCheck);
                        var tableCheck = new Table();
                        tableCheck.AddColumns("Сумма");
                        tableCheck.AddRow($"{bankService.CheckAccountSum(accountCheck)}");
                        AnsiConsole.Render(tableCheck.Centered());
                        break;
                    case "Управление транзакциями":
                        var ruleTrans = new Rule("[red]Управление транзакциями[/]");
                        AnsiConsole.Render(ruleTrans);
                        var trans = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[green]Какое действие хотите выполнить?[/]:")
                                .PageSize(3)
                                .AddChoices(new[]
                                {
                                    "Подтвердить транзакцию", "Отменить транзакцию",
                                }));
                        if (trans == "Подтвердить транзакцию")
                        {
                            int transId =
                                AnsiConsole.Ask<int>("[green]Введите Id транзакции, которую хотите подтвердить[/]:");
                            bankService.ExecuteTransaction(transId);
                        }

                        if (trans == "Отменить транзакцию")
                        {
                            int transId2 =
                                AnsiConsole.Ask<int>("[green]Введите Id транзакции, которую хотите отменить[/]:");
                            bankService.CancelTransaction(transId2);
                        }

                        break;
#pragma warning restore 618
                    case "Выйти из приложения":
                        alive = false;
                        continue;
                }
            }
        }
    }
}