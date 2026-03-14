using System;
using Autoservis.Model;
using Autoservis.Service.Interface;
using Autoservis.Service.Repository;
using Autoservis.Service.Services;

class Program
{
    static void Main(string[] args)
    {
        ILoggerService loggerService = new LoggerService();

        IUserRepository userRepository = new UserRepository();
        IAuthService authService = new AuthService(userRepository, loggerService);

        IVehicleRepository vehicleRepository = new VehicleRepository();
        VehicleService vehicleService = new VehicleService(vehicleRepository,loggerService);

        IReceiptRepository receiptRepository = new ReceiptRepository();
        IPaymentType paymentTime;

        loggerService.Log(ErrorType.INFO, "Program je pokrenut.");

        //var now = DateTime.Now.TimeOfDay;
        var now = new TimeSpan(9, 0, 0);

        if (now >= new TimeSpan(8, 0, 0) && now < new TimeSpan(12, 0, 0))
        {
            paymentTime = new MorningPayment();
        }
        else if (now >= new TimeSpan(12, 0, 0) && now < new TimeSpan(16, 0, 0))
        {
            paymentTime = new AfternoonPayment();
        }
        else
        {
            Console.WriteLine("Autoservis trenutno ne radi.");
            loggerService.Log(ErrorType.INFO, "Autoservis trenutno ne radi. Program se zatvara.");
            return;
        }
        ServisService servisService = new ServisService(vehicleRepository, paymentTime, receiptRepository, loggerService);

        while (true)
        {
            Console.WriteLine("------------LOGIN------------");

            Console.Write("Nickname: ");
            string nickname = Console.ReadLine();
            if (string.IsNullOrEmpty(nickname))
            {
                Console.WriteLine("Korisnicko ime ne moze biti prazno.");
                loggerService.Log(ErrorType.WARNING, "Pokusaj loginovanja sa praznim nicknameom.");
                return;
            }

            User user = userRepository.GetUserByNickname(nickname);

            if (user == null)
            {
                Console.WriteLine("Ne postoji korisnik sa tim nicknamom.");
                loggerService.Log(ErrorType.ERROR, $"Pokusaj loginovanja sa nepostojecim nicknamom: {nickname}.");
                return;
            }
            bool loggedIn = false;
            while (!loggedIn)
            {
                Console.Write("Password: ");
                string password = Console.ReadLine();
                if (string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Lozinka ne moze biti prazna.");
                    loggerService.Log(ErrorType.WARNING, $"Pokusaj loginovanja sa praznom lozinkom za korisnika: {nickname}.");
                    continue;
                }
                try
                {
                    if (authService.Login(nickname, password))
                    {
                        Console.WriteLine("Uspesno loginovanje.");
                        loggedIn = true;
                    }
                    else
                    {
                        Console.WriteLine("Pogresna lozinka,pokusajte ponovo.");
                        loggerService.Log(ErrorType.WARNING, $"Pogresna lozinka za korisnika: {nickname}.");
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                    break;
                }
            }
            if (!loggedIn)
                continue;

            var loggedInUser = userRepository.GetUserByNickname(nickname);

            switch (loggedInUser.Role)
            {
                case Role.Menager:
                    MenagerMeni(vehicleService, receiptRepository, loggerService);
                    break;
                case Role.Mehanicar:
                    MehanicarMeni(vehicleService, loggedInUser, receiptRepository, servisService, loggerService);
                    break;
                default:
                    Console.WriteLine("Rola korisnika nepoznata.");
                    loggerService.Log(ErrorType.ERROR, $"Korisnik {nickname} ima nepoznatu rolu: {loggedInUser.Role}.");
                    break;
            }
            loggerService.Log(ErrorType.INFO, $"Korisnik {nickname} se odjavio.");
        }
        static void MenagerMeni(VehicleService vehicleService, IReceiptRepository receiptRepository, ILoggerService loggerService)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n------------MENAGER MENI------------");
                Console.WriteLine("1. Dodaj novo vozilo");
                Console.WriteLine("2. Prikazi sva vozila");
                Console.WriteLine("3. Prikazi sve racune za usluge");
                Console.WriteLine("4. Izlaz");
                Console.WriteLine("5. Povratak na login");

                Console.Write("Izaberite opciju: ");
                string opcija = Console.ReadLine();
                loggerService.Log(ErrorType.INFO, $"Menager je izabrao opciju: {opcija}.");

                switch (opcija)
                {
                    case "1":
                        string registration;
                        while (true)
                        {
                            Console.WriteLine("Unesite registraciju vozila:");
                            registration = Console.ReadLine();
                            if (string.IsNullOrEmpty(registration))
                            {
                                Console.WriteLine("Registracija ne moze biti prazna. Unesite ponovo:");
                                loggerService.Log(ErrorType.WARNING, "Pokusaj dodavanja vozila gde nije uneta registracija.");
                            }
                            else
                            {
                                break;
                            }
                        }
                        string model;
                        while (true)
                        {
                            Console.WriteLine("Unesite model vozila:");
                            model = Console.ReadLine();
                            if (string.IsNullOrEmpty(model))
                            {
                                Console.WriteLine("Model ne moze biti prazna. Unesite ponovo:");
                                loggerService.Log(ErrorType.WARNING, "Pokusaj dodavanja vozila gde nije unet model.");
                            }
                            else
                            {
                                break;
                            }
                        }

                        string mark;
                        while (true)
                        {
                            Console.WriteLine("Unesite marku vozila:");
                            mark = Console.ReadLine();
                            if (string.IsNullOrEmpty(mark))
                            {
                                Console.WriteLine("Marka ne moze biti prazna. Unesite ponovo:");
                                loggerService.Log(ErrorType.WARNING, "Pokusaj dodavanja vozila gde nije uneta marka.");
                            }
                            else
                            {
                                break;
                            }
                        }
                        TypeOfVehicle typeOfVehicle;
                        while (true)
                        {
                            Console.WriteLine("Unesite tip vozila (Putnicko, Teretno, Motocikl):");
                            string typeInput = Console.ReadLine();
                            if (Enum.TryParse<TypeOfVehicle>(typeInput, true, out typeOfVehicle))
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Neispravan tip vozila. Unesite ponovo:");
                                loggerService.Log(ErrorType.WARNING, $"Pokusaj dodavanja vozila sa neispravnim tipom: {typeInput}.");
                            }
                        }

                        double price;
                        while (true)
                        {
                            Console.WriteLine("Unesite cenu servisa:");
                            string priceInput = Console.ReadLine();
                            if (double.TryParse(priceInput, out price) && price >= 0)
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Neispravan unos za cenu. Unesite ponovo:");
                                loggerService.Log(ErrorType.WARNING, $"Pokusaj dodavanja vozila sa neispravnim unosom za cenu: {priceInput}.");
                            }
                        }

                        var vehicle = new Vehicle(registration, model, mark, typeOfVehicle, price);
                        vehicleService.Add(vehicle);
                        Console.WriteLine("Vozilo je uspesno dodato!");
                        loggerService.Log(ErrorType.INFO, $"Dodat novi automobil: {registration}, {model}, {mark}, {typeOfVehicle}, {price}.");
                        break;

                    case "2":
                        var vehicles = vehicleService.GetAll();
                        if (vehicles.Count == 0)
                        {
                            Console.WriteLine("Nema vozila za prikaz.");
                            loggerService.Log(ErrorType.INFO, "Menager je pokusao da prikaze vozila, ali nema vozila za prikaz.");
                            break;
                        }
                        Console.WriteLine("Sva vozila:");
                        loggerService.Log(ErrorType.INFO, "Menager prikazuje sva vozila.");
                        foreach (var v in vehicles)
                        {
                            Console.WriteLine($"Registracija: {v.Registration}, Model: {v.Model}, Marka: {v.Mark}, Tip: {v.Type}, Cena: {v.Price}");
                        }
                        break;
                    case "3":
                        var receipts = receiptRepository.GetAll();
                        if (receipts.Count == 0)
                        {
                            Console.WriteLine("Nema racuna za prikaz.");
                            loggerService.Log(ErrorType.INFO, "Menager je pokusao da prikaze racune, ali nema racuna za prikaz.");
                            break;
                        }
                        Console.WriteLine("Svi racuni:");
                        loggerService.Log(ErrorType.INFO, "Menager prikazuje sve racune.");
                        foreach (var r in receipts)
                        {
                            Console.WriteLine($"Mehanicar: {r.MechanicName}, Datum: {r.Date}, Total: {r.Total}");
                        }
                        break;
                    case "4":
                        loggerService.Log(ErrorType.INFO, "Menager je izabrao da izadje iz programa.");
                        Environment.Exit(0);
                        break;
                    case "5":
                        Console.WriteLine("Povratak na login...");
                        return;
                    default:
                        Console.WriteLine("Ta opcija nije navedena!");
                        loggerService.Log(ErrorType.WARNING, $"Menager je uneo nepostojecu opciju: {opcija}.");
                        break;
                }

            }
        }

        static void MehanicarMeni(VehicleService vehicleService, User loggedInUser, IReceiptRepository receiptRepository, ServisService servisService, ILoggerService loggerService)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n------------MEHANICAR MENI------------");
                Console.WriteLine("1. Prikazi sva neservisirana vozila");
                Console.WriteLine("2. Zavrsi za izdavanjem racuna i servisiranjem vozila.");
                Console.WriteLine("3. Izlaz");
                Console.WriteLine("4. Povratak na login");

                Console.Write("Izaberite opciju: ");
                string opcija = Console.ReadLine();
                loggerService.Log(ErrorType.INFO, $"Mehanicar {loggedInUser.Nickname} je izabrao opciju: {opcija}.");

                switch (opcija)
                {

                    case "1":
                        var vehicles = vehicleService.GetUnserviced();
                        if (vehicles.Count == 0)
                        {
                            Console.WriteLine("Nema vozila koja su neservisirana.");
                            loggerService.Log(ErrorType.WARNING, $"Mehanicar {loggedInUser.Nickname} je pokusao da prikaze neservisirana vozila, ali nema neservisiranih vozila za prikaz.");
                            break;
                        }
                        Console.WriteLine("Sva neservisirana vozila:");
                        loggerService.Log(ErrorType.INFO, $"Mehanicar {loggedInUser.Nickname} prikazuje sva neservisirana vozila.");
                        foreach (var v in vehicles)
                        {
                            Console.WriteLine($"Registracija: {v.Registration}, Model: {v.Model}, Marka: {v.Mark}, Tip: {v.Type}, Cena: {v.Price}");
                        }
                        break;
                    case "2":
                        string registration;
                        while (true)
                        {
                            Console.WriteLine("Unesite registraciju vozila kojeg zelite da servisirate:");
                            registration = Console.ReadLine();
                            if (string.IsNullOrEmpty(registration))
                            {
                                Console.WriteLine("Registracija ne moze biti prazna. Unesite ponovo:");
                                loggerService.Log(ErrorType.INFO, $"Mehanicar {loggedInUser.Nickname} zeli servisirati vozilo sa registracijom: {registration}.");
                            }
                            else
                            {
                                break;
                            }
                        }

                        var vehicle = vehicleService.GetUnserviced().FirstOrDefault(v => v.Registration == registration);
                        if (vehicle == null)
                        {
                            Console.WriteLine("Nema neservisiranog vozila sa tom registracijom ili je vozilo sa tom registracijom vec servisirano.");
                            loggerService.Log(ErrorType.INFO, $"Mehanicar {loggedInUser.Nickname} je pokusao da servisira vozilo sa registracijom: {registration}, ali vozilo nije pronadjeno.");
                            break;
                        }

                        servisService.FinishServis(vehicle.Id, loggedInUser.Name);
                        Console.WriteLine("Servis izvrsen i racun izdat");
                        break;

                    case "3":
                        loggerService.Log(ErrorType.INFO, $"Mehanicar {loggedInUser.Nickname} je izabrao da izadje iz programa.");
                        Environment.Exit(0);
                        break;
                    case "4":
                        Console.WriteLine("Povratak na login...");
                        loggerService.Log(ErrorType.INFO, $"Mehanicar {loggedInUser.Nickname} se vraca na login.");
                        return;
                    default:
                        Console.WriteLine("Ta opcija nije navedena!");
                        loggerService.Log(ErrorType.WARNING, $"Mehanicar {loggedInUser.Nickname} je uneo nepostojecu opciju: {opcija}.");
                        break;
                }

            }
        }
    }
}