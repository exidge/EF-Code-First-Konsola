using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace EF_Code_First_Konsola
{
    class Program
    {
        public static class Baza
        {
            private static void deatchJezeliTrzeba(Uczestnik uczestnikObj)
            {
                var local = context.Set<Uczestnik>()
                    .Local
                    .FirstOrDefault(e => e.ID == uczestnikObj.ID);
                if (local != null)
                {
                    context.Entry(local).State = EntityState.Detached;
                }
                Uczestnik obj=new Uczestnik(){ ID = 0, Imie = "imie", Nazwisko = "nazwisko", takNocleg = true, takObiad = true, takOplata = true, dataOdjazdu = DateTime.Now, dataPrzyjazdu = DateTime.Now };
                context.Uczestnicy.Add(obj);
                context.SaveChanges();
                
            }
            public static void Dodaj(Uczestnik osoba)
            {
                context.Uczestnicy.Add(osoba);
                context.SaveChanges();
                //context.Uczestnicy.First(e => e.ID == 1);
            }
            public static void Usun(int id)
            {   
                Uczestnik doUsuniecia=new Uczestnik(){ ID=id };
                deatchJezeliTrzeba(doUsuniecia);
                context.Uczestnicy.Attach(doUsuniecia);
                context.Uczestnicy.Remove(doUsuniecia);
                context.SaveChanges();
            }
            public static void Wyswietl()
            {
                Console.Clear();
                string str = "";
                string separator = String.Format("{0,89}", str.PadLeft(89, '-'));
                Console.WriteLine("Zawartosc bazy:");
                Console.WriteLine(separator);
                Console.Write(String.Format("|{0,-2}|{1,-11}|{2,-12}|{3,-5}|{4,-6}|{5,-6}|{6,-19}|{7,-19}|","Id","Imie","Nazwisko","Obiad","Nocleg","Oplata","Data przyjazdu","Data odjazdu")+System.Environment.NewLine);
                Console.WriteLine(separator);
                var r = context.Uczestnicy.FirstOrDefault(p => p.ID == 0);
                foreach (var row in context.Uczestnicy)
                {
                    Console.Write(String.Format("|{0,2}|{1,-11}|{2,-12}|{3,-5}|{4,-6}|{5,-6}|{6,-19}|{7,-19}|", row.ID,row.Imie,row.Nazwisko,row.takObiad,row.takNocleg,row.takOplata,row.dataPrzyjazdu.ToString("dd-MM-yyyy HH:mm"),row.dataOdjazdu.ToString("dd-MM-yyyy HH:mm")) + System.Environment.NewLine);
                }

                Console.WriteLine(separator);
                Console.WriteLine("Koniec zawartosci bazy:");
            }
            public static String WyswietlJednego(int id)
            {
                string str = "";
                string separator = String.Format("{0,89}", str.PadLeft(89, '-'));
                Uczestnik doWyswietlenia = context.Uczestnicy.First(e => e.ID == id);
                Uczestnik row = doWyswietlenia;
                String ret = separator + System.Environment.NewLine;
                ret+=String.Format("|{0,2}|{1,-11}|{2,-12}|{3,-5}|{4,-6}|{5,-6}|{6,-19}|{7,-19}|", row.ID, row.Imie, row.Nazwisko, row.takObiad, row.takNocleg, row.takOplata, row.dataPrzyjazdu.ToString("dd-MM-yyyy HH:mm"), row.dataOdjazdu.ToString("dd-MM-yyyy HH:mm")) + System.Environment.NewLine;
                ret += separator;
                return ret;
            }
            public static void EdytujTest(int id)
            {
                Uczestnik doUsuniecia = context.Uczestnicy.First(e => e.ID == id);
                doUsuniecia.Imie = "Marian";
                context.Uczestnicy.Attach(doUsuniecia);
                var entry = context.Entry(doUsuniecia);
                entry.Property(e => e.Imie).IsModified = true;
                context.SaveChanges();
            }
            public static void Edytuj(Uczestnik uczestnik)
            {
                deatchJezeliTrzeba(uczestnik);
                var entry = context.Entry(uczestnik);
                entry.State = EntityState.Modified;
                //entry.Property(e => e.Imie).IsModified = true;
                context.SaveChanges();
                
            }
        }
        static string ConnString = @"Data Source = localhost\SQLEXPRESS; Integrated Security = true;";
        static UczestnicyKonferencji context;
        private static void doTest()
        {           
            Baza.Wyswietl();
            //Baza.Dodaj(new Uczestnik {
            //    ID = 0,
            //    Imie = "Marek",
            //    Nazwisko = "Kamiński",
            //    takObiad = true,
            //    takNocleg = true,
            //    takOplata = true,
            //    dataPrzyjazdu = DateTime.Now,
            //    dataOdjazdu = DateTime.Now.AddDays(2),
            //});
            //Console.WriteLine("DEBUG: SaveDone?");
            //Baza.Usun(2);
            Baza.EdytujTest(10);
            Uczestnik doModyfikacji = context.Uczestnicy.First(e => e.ID == 11);
            doModyfikacji.Imie = "Kamil2";
            doModyfikacji.Nazwisko = "Malinowski2";
            Baza.Edytuj(doModyfikacji);
            Console.WriteLine("DEBUG: Usuniecie?");
            System.Threading.Thread.Sleep(1000);
            Baza.Wyswietl();
            Console.ReadKey();
        }
        public static string errorSeparator;
        static void Main(string[] args)
        {
            Console.SetWindowSize(90, 30);
            context = new UczestnicyKonferencji(ConnString);
            string str = "";
            string separator = String.Format("{0,89}", str.PadLeft(89, '-'));
            errorSeparator = String.Format("{0,89}", str.PadLeft(89, '!'));
            Boolean czyKontyuuowac = true;
            while (czyKontyuuowac) { 
                Console.WriteLine(separator);
                Console.WriteLine("Zarzadzanie uczestnikami konferencji. Co chcesz zrobic?");
                Console.WriteLine("1. Wyswietl wszystkich uczestnikow.");
                Console.WriteLine("2. Dodaj nowego uczestnika.");
                Console.WriteLine("3. Usuń uczestnika.");
                Console.WriteLine("4. Modyfikuj dane uczestnika.");
                Console.WriteLine("5. Wyjdz z programu.");
                Console.WriteLine("Wpisz numer odpowiedniej opcji:");
                string opcjaStr=Console.ReadLine();
                int opcja = 0;
                try
                {
                    opcja=Int32.Parse(opcjaStr);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Niepoprawna opcja.");
                    continue;
                }
                switch (opcja)
                {
                    case 1:
                        Baza.Wyswietl();
                        break;
                    case 2:
                        Console.WriteLine("Wpisz dane uczestnika:");
                        Console.WriteLine("Imie:");
                        var imie=Console.ReadLine();
                        Console.WriteLine("Nazwisko:");
                        var nazwisko = Console.ReadLine();
                        Console.WriteLine("Czy zawiera Obiad?(TAK/NIE):");
                        var czyObiadStr = Console.ReadLine();
                        Boolean czyObiad;              
                        if (czyObiadStr.Equals("tak",StringComparison.OrdinalIgnoreCase))
                        {
                            czyObiad = true;
                        }
                        else if (czyObiadStr.Equals("nie", StringComparison.OrdinalIgnoreCase))
                        {
                            czyObiad = false;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine(errorSeparator);
                            Console.WriteLine("Podano bledne opcje, dostepne tylko TAK/NIE");
                            Console.WriteLine(errorSeparator);
                            break;
                        }
                        Console.WriteLine("Czy nocuje?(TAK/NIE):");
                        var czyNoclegStr = Console.ReadLine();
                        Boolean czyNocleg;
                        if (czyNoclegStr.Equals("tak", StringComparison.OrdinalIgnoreCase))
                        {
                            czyNocleg = true;
                        }
                        else if (czyNoclegStr.Equals("nie", StringComparison.OrdinalIgnoreCase))
                        {
                            czyNocleg = false;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine(errorSeparator);
                            Console.WriteLine("Podano bledne opcje, dostepne tylko TAK/NIE");
                            Console.WriteLine(errorSeparator);
                            break;
                        }
                        Console.WriteLine("Czy jest oplacone?(TAK/NIE):");
                        var czyOplataStr = Console.ReadLine();
                        Boolean czyOplata;
                        if (czyOplataStr.Equals("tak", StringComparison.OrdinalIgnoreCase))
                        {
                            czyOplata = true;
                        }
                        else if (czyOplataStr.Equals("nie", StringComparison.OrdinalIgnoreCase))
                        {
                            czyOplata = false;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine(errorSeparator);
                            Console.WriteLine("Podano bledne opcje, dostepne tylko TAK/NIE");
                            Console.WriteLine(errorSeparator);
                            break;
                        }
                        Console.WriteLine("Data przyjazdu(dzien-miesiac-rok godzina:minuta):");                     
                        var dataPrzyjazduStr = Console.ReadLine();
                        DateTime dataPrzyjazdu=new DateTime();
                        try
                        {
                            dataPrzyjazdu= DateTime.ParseExact(dataPrzyjazduStr, "dd-MM-yyyy HH:mm", null);
                        }
                        catch (FormatException)
                        {
                            Console.Clear();
                            Console.WriteLine(errorSeparator);
                            Console.WriteLine("Podano błędna data, lub format daty jest niepoprawny!");
                            Console.WriteLine(errorSeparator);
                            break;
                        }
                       
                        Console.WriteLine("Data odjazu:(dzien-miesiac-rok godzina:minuta):");
                        var dataOdjazduStr = Console.ReadLine();
                        DateTime dataOdjazdu = new DateTime();
                        try
                        {
                            dataOdjazdu = DateTime.ParseExact(dataOdjazduStr, "dd-MM-yyyy HH:mm", null);
                        }
                        catch (FormatException)
                        {
                            Console.Clear();
                            Console.WriteLine(errorSeparator);
                            Console.WriteLine("Podano błędna data, lub format daty jest niepoprawny!");
                            Console.WriteLine(errorSeparator);
                            break;
                        }                     
                        Uczestnik doDodania = new Uczestnik
                        {
                            ID = 0,
                            Imie = imie,
                            Nazwisko = nazwisko,
                            takObiad = czyObiad,
                            takNocleg = czyNocleg,
                            takOplata = czyOplata,
                            dataPrzyjazdu = dataPrzyjazdu,
                            dataOdjazdu = dataOdjazdu,
                        };
                        Baza.Dodaj(doDodania);
                        Console.Clear();
                        Console.WriteLine("Uczestnik zostal dodany do bazy");
                        break;
                    case 3:
                        Console.WriteLine("Usuwanie uczestnika. Prosze wpisać ID uczestnika do usuniecia:");
                        var idStr=Console.ReadLine();
                        int id = -1;
                        try
                        {
                            id=Int32.Parse(idStr);
                            Baza.Usun(id);
                            Console.Clear();
                            Console.WriteLine("Uzytkownik zostal usuniety");
                        }
                        catch (FormatException ex)
                        {
                            Console.Clear();
                            Console.WriteLine(errorSeparator);
                            Console.WriteLine("Tylko cyfry!");
                            Console.WriteLine(errorSeparator);
                        }
                        catch(System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex2)
                        {
                            Console.Clear();
                            Console.WriteLine(errorSeparator);
                            Console.WriteLine("Uczestnik o takim id nie istnieje w bazie.");
                            Console.WriteLine(errorSeparator);
                        }                              
                        break;
                    case 4:
                        Console.WriteLine("Wpisz ID uczestnika do edycji:");
                        var idStrED = Console.ReadLine();
                        int idED = -1;
                        Uczestnik toEdit;
                        try
                        {
                            idED = Int32.Parse(idStrED);
                            toEdit = context.Uczestnicy.FirstOrDefault(e => e.ID == idED);
                            if (toEdit == null)
                            {
                                Console.Clear();
                                Console.WriteLine(errorSeparator);
                                Console.WriteLine("Nie znaleziono użytkownika o takim id w bazie.");
                                Console.WriteLine(errorSeparator);
                                break;
                            }
                        }
                        catch (FormatException)
                        {
                            Console.Clear();
                            Console.WriteLine(errorSeparator);
                            Console.WriteLine("Należy wpisac cyfre!");
                            Console.WriteLine(errorSeparator);
                            break;
                        }
                        Console.WriteLine(Baza.WyswietlJednego(idED));
                        Console.WriteLine("Wpisz nowe dane ktore chcesz zastapic, w przypadku w ktorym maja zostac stare nacisnij tylko enter");
                        Console.WriteLine("Imie:");
                        var imieED = Console.ReadLine();
                        if (imieED.Length==0)
                        {
                            imieED = toEdit.Imie;
                        }
                        Console.WriteLine("Nazwisko:");
                        var nazwiskoED = Console.ReadLine();
                        if (nazwiskoED.Length == 0)
                        {
                            nazwiskoED = toEdit.Nazwisko;
                        }
                        Console.WriteLine("Czy zawiera Obiad?(TAK/NIE):");
                        var czyObiadStrED = Console.ReadLine();
                        Boolean czyObiadED;
                        if (czyObiadStrED.Equals("tak", StringComparison.OrdinalIgnoreCase))
                        {
                            czyObiadED = true;
                        }
                        else if (czyObiadStrED.Equals("nie", StringComparison.OrdinalIgnoreCase))
                        {
                            czyObiadED = false;
                        }
                        else if (czyObiadStrED.Length == 0)
                        {
                            czyObiadED = toEdit.takObiad;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine(errorSeparator);
                            Console.WriteLine("Podano bledne opcje, dostepne tylko TAK/NIE");
                            Console.WriteLine(errorSeparator);
                            break;
                        }
                        Console.WriteLine("Czy nocuje?(TAK/NIE):");
                        var czyNoclegStrED = Console.ReadLine();
                        Boolean czyNoclegED;
                        if (czyNoclegStrED.Equals("tak", StringComparison.OrdinalIgnoreCase))
                        {
                            czyNoclegED = true;
                        }
                        else if (czyNoclegStrED.Equals("nie", StringComparison.OrdinalIgnoreCase))
                        {
                            czyNoclegED = false;
                        }
                        else if (czyNoclegStrED.Length == 0)
                        {
                            czyNoclegED = toEdit.takNocleg;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine(errorSeparator);
                            Console.WriteLine("Podano bledne opcje, dostepne tylko TAK/NIE");
                            Console.WriteLine(errorSeparator);
                            break;
                        }
                        Console.WriteLine("Czy jest oplacone?(TAK/NIE):");
                        var czyOplataStrED = Console.ReadLine();
                        Boolean czyOplataED;
                        if (czyOplataStrED.Equals("tak", StringComparison.OrdinalIgnoreCase))
                        {
                            czyOplataED = true;
                        }
                        else if (czyOplataStrED.Equals("nie", StringComparison.OrdinalIgnoreCase))
                        {
                            czyOplataED = false;
                        }
                        else if (czyOplataStrED.Length == 0)
                        {
                            czyOplataED = toEdit.takOplata;
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine(errorSeparator);
                            Console.WriteLine("Podano bledne opcje, dostepne tylko TAK/NIE");
                            Console.WriteLine(errorSeparator);
                            break;
                        }
                        Console.WriteLine("Data przyjazdu(dzien-miesiac-rok godzina:minuta):");
                        var dataPrzyjazduStrED = Console.ReadLine();
                        DateTime dataPrzyjazduED;
                        if (dataPrzyjazduStrED.Length == 0)
                        {
                            dataPrzyjazduED = toEdit.dataPrzyjazdu;
                        }
                        else
                        {
                            try
                            {
                                dataPrzyjazduED = DateTime.ParseExact(dataPrzyjazduStrED, "dd-MM-yyyy HH:mm", null);
                            }
                            catch (FormatException)
                            {
                                Console.Clear();
                                Console.WriteLine(errorSeparator);
                                Console.WriteLine("Podano błędna data, lub format daty jest niepoprawny!");
                                Console.WriteLine(errorSeparator);
                                break;
                            }
                        }
                        Console.WriteLine("Data odjazu:(dzien-miesiac-rok godzina:minuta):");
                        var dataOdjazduStrED = Console.ReadLine();
                        DateTime dataOdjazduED;
                        if (dataOdjazduStrED.Length == 0)
                        {
                            dataOdjazduED = toEdit.dataOdjazdu;
                        }
                        else
                        {
                            try
                            {
                                dataOdjazduED = DateTime.ParseExact(dataOdjazduStrED, "dd-MM-yyyy HH:mm", null);
                  
                            }
                            catch (FormatException)
                            {
                                Console.Clear();
                                Console.WriteLine(errorSeparator);
                                Console.WriteLine("Podano błędna data, lub format daty jest niepoprawny!");
                                Console.WriteLine(errorSeparator);
                                break;
                            }
                        }
                        Uczestnik doDodaniaED = new Uczestnik
                        {
                            ID = idED,
                            Imie = imieED,
                            Nazwisko = nazwiskoED,
                            takObiad = czyObiadED,
                            takNocleg = czyNoclegED,
                            takOplata = czyOplataED,
                            dataPrzyjazdu = dataPrzyjazduED,
                            dataOdjazdu = dataOdjazduED,
                        };
                        //context.Uczestnicy.Attach(toEdit);
                        Baza.Edytuj(doDodaniaED);
                        Console.Clear();
                        Console.WriteLine("Uczestnik zostal zmodyfikowany w bazie");
                        break;
                    case 5:
                        czyKontyuuowac = false;
                        break;
                }

            }
        }
    }
}
