using System;
using Autoservis.Model;
using Autoservis.Service.Interface;
using Autoservis.Service.Logic;

class Program
{
    static void Main(string[] args)
    {
        IUserRepository userRepository = new UserRepository();
        IAuthService authService = new AuthService(userRepository);


        Console.WriteLine("------------LOGIN------------");

        Console.Write("Nickname: ");
        string nickname = Console.ReadLine();
        if (string.IsNullOrEmpty(nickname))
        {
            Console.WriteLine("Korisnicko ime ne moze biti prazno.");
            return;
        }

        User user = userRepository.GetUserByNickname(nickname);
        if (user == null)
        {
            Console.WriteLine("Ne postoji korisnik sa tim nicknamom.");
            return;
        }
        while (true)
        {
            Console.Write("Password: ");
            string password = Console.ReadLine();
            if (string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Lozinka ne moze biti prazna.");
                continue;
            }
            try
            {
                if (authService.Login(nickname, password))
                {
                    Console.WriteLine("Uspesno loginovanje.");


                    if (user.Role == Role.Menager)
                    {
                        MenagerMeni();
                    }
                    else if (user.Role == Role.Mehanicar)
                    {
                        MehanicarMeni();
                    }

                    break;
                }
                else
                {
                    Console.WriteLine("Pogresna lozinka,pokusajte ponovo.");    
                }
            }catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
                break;
            }
        }
        static void MenagerMeni()
        {
            Console.WriteLine("------------MENAGER MENI------------");
        }

        static void MehanicarMeni()
        {
            Console.WriteLine("------------MEHANICAR MENI------------");
        }
    }
}