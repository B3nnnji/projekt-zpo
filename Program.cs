using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HotelManager
{
    public interface IRezerwowalny
    {
        bool SprawdzDostepnosc(DateTime data);
        void WyswietlInformacje();
    }

    public abstract class Pomieszczenie : IRezerwowalny
    {
        public int Numer { get; }
        public int Pietro { get; protected set; }
        public decimal CenaZaNoc { get; set; }

        protected List<DateTime> terminyZajete = new List<DateTime>();

        public Pomieszczenie(int numer, int pietro, decimal cenaZaNoc)
        {
            Numer = numer;
            Pietro = pietro;
            CenaZaNoc = cenaZaNoc;
            Console.WriteLine($"Utworzono pomieszczenie nr {Numer} na piętrze {Pietro}.");
        }

        public bool Zarezerwuj(DateTime data)
        {
            if (SprawdzDostepnosc(data))
            {
                terminyZajete.Add(data.Date);
                return true;
            }
            return false;
        }

        public bool SprawdzDostepnosc(DateTime data)
        {
            return !terminyZajete.Contains(data.Date);
        }

        public virtual void WyswietlInformacje()
        {
            Console.WriteLine($"Pomieszczenie nr {Numer}, Piętro: {Pietro}, Cena: {CenaZaNoc:C}");
        }

        public abstract string PobierzTypPomieszczenia();
    }

    public class PokojHotelowy : Pomieszczenie
    {
        public int LiczbaLozek { get; set; }

        public PokojHotelowy(int numer, int pietro, decimal cenaZaNoc, int liczbaLozek)
            : base(numer, pietro, cenaZaNoc)
        {
            LiczbaLozek = liczbaLozek;
        }

        public override void WyswietlInformacje()
        {
            base.WyswietlInformacje();
            Console.WriteLine($"  Typ: Pokój hotelowy, Liczba łóżek: {LiczbaLozek}");
        }

        public override string PobierzTypPomieszczenia()
        {
            return "Pokój hotelowy";
        }

        public static PokojHotelowy operator ++(PokojHotelowy pokoj)
        {
            pokoj.CenaZaNoc *= 1.1m;
            return pokoj;
        }
    }

    public class SalaKonferencyjna : Pomieszczenie
    {
        public int Pojemnosc { get; set; }

        public SalaKonferencyjna(int numer, int pietro, decimal cenaZaNoc, int pojemnosc)
            : base(numer, pietro, cenaZaNoc)
        {
            Pojemnosc = pojemnosc;
        }

        public override void WyswietlInformacje()
        {
            base.WyswietlInformacje();
            Console.WriteLine($"  Typ: Sala konferencyjna, Pojemność: {Pojemnosc} osób");
        }

        public override string PobierzTypPomieszczenia()
        {
            return "Sala konferencyjna";
        }
    }

    public class SystemRezerwacji<T> where T : Pomieszczenie
    {
        private readonly List<T> listaPomieszczen = new List<T>();

        public T this[int numerPomieszczenia]
        {
            get { return listaPomieszczen.FirstOrDefault(p => p.Numer == numerPomieszczenia); }
        }

        public void DodajPomieszczenie(T pomieszczenie)
        {
            listaPomieszczen.Add(pomieszczenie);
            OnPomieszczenieDodane(pomieszczenie);
        }

        public IEnumerable<T> ZnajdzDostepne(DateTime data)
        {
            return listaPomieszczen.Where(p => p.SprawdzDostepnosc(data));
        }
        
        public IEnumerable<T> PobierzWszystkie()
        {
            return listaPomieszczen;
        }

        public delegate void PomieszczenieDodaneEventHandler(object sender, T pomieszczenie);
        public event PomieszczenieDodaneEventHandler PomieszczenieDodane;

        protected virtual void OnPomieszczenieDodane(T pomieszczenie)
        {
            PomieszczenieDodane?.Invoke(this, pomieszczenie);
        }
    }

    public static class NarzedziaHotelowe
    {
        public static void WyswietlSzczegolyObiektu(object obj)
        {
            Console.WriteLine($"\n--- Analiza obiektu typu '{obj.GetType().Name}' (Refleksja) ---");
            foreach (var prop in obj.GetType().GetProperties())
            {
                Console.WriteLine($"  Właściwość: {prop.Name}, Wartość: {prop.GetValue(obj)}");
            }
            Console.WriteLine("----------------------------------------------------");
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Inicjalizacja systemu rezerwacji hotelu...");
            var system = new SystemRezerwacji<Pomieszczenie>();

            system.PomieszczenieDodane += (sender, p) => {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[ZDARZENIE] Dodano nowe pomieszczenie: {p.PobierzTypPomieszczenia()} nr {p.Numer}");
                Console.ResetColor();
            };

            var pokoj1 = new PokojHotelowy(101, 1, 250, 2);
            var sala1 = new SalaKonferencyjna(10, 0, 1200, 50);
            var pokoj2 = new PokojHotelowy(202, 2, 350, 3);

            system.DodajPomieszczenie(pokoj1);
            system.DodajPomieszczenie(sala1);
            system.DodajPomieszczenie(pokoj2);

            pokoj1.Zarezerwuj(DateTime.Now.AddDays(1));
            pokoj1.Zarezerwuj(DateTime.Now.AddDays(2));
            sala1.Zarezerwuj(DateTime.Now.AddDays(5));
            Console.WriteLine("\n[SYSTEM] Dodano przykładowe rezerwacje w celu demonstracji.");
            Console.WriteLine($"[DEV-INFO] Aby przetestować, sprawdź dostępność dla daty: {DateTime.Now.AddDays(1):yyyy-MM-dd}");

            Console.WriteLine("\nNaciśnij dowolny klawisz, aby rozpocząć interakcję...");
            Console.ReadKey();
            Console.Clear();

            await UruchomMenu(system);
        }

        static async Task UruchomMenu(SystemRezerwacji<Pomieszczenie> system)
        {
            while (true)
            {
                Console.WriteLine("\n--- MENU GŁÓWNE ---");
                Console.WriteLine("1. Wyświetl wszystkie pomieszczenia");
                Console.WriteLine("2. Sprawdź dostępność w danym dniu");
                Console.WriteLine("3. Zwiększ cenę pokoju 101 (przeciążanie operatorów)");
                Console.WriteLine("4. Analizuj obiekt (refleksja)");
                Console.WriteLine("5. Uruchom nocne sprzątanie (async)");
                Console.WriteLine("0. Wyjdź");
                Console.Write("Wybierz opcję: ");

                string wybor = Console.ReadLine();
                switch (wybor)
                {
                    case "1":
                        Console.WriteLine("\n--- Lista wszystkich pomieszczeń ---");
                        foreach (var p in system.PobierzWszystkie()) p.WyswietlInformacje();
                        break;
                    case "2":
                        Console.Write("Podaj datę (RRRR-MM-DD) do sprawdzenia: ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime data))
                        {
                            Console.WriteLine($"\n--- Dostępne pomieszczenia w dniu {data:d} ---");
                            var dostepne = system.ZnajdzDostepne(data);
                            if (!dostepne.Any()) Console.WriteLine("Brak dostępnych pomieszczeń w tym terminie.");
                            else foreach (var p in dostepne) p.WyswietlInformacje();
                        }
                        else Console.WriteLine("Nieprawidłowy format daty.");
                        break;
                    case "3":
                        var pokoj101 = system[101] as PokojHotelowy;
                        if (pokoj101 != null)
                        {
                            pokoj101++;
                            Console.WriteLine($"Cena pokoju 101 została zwiększona. Nowa cena: {pokoj101.CenaZaNoc:C}");
                        }
                        break;
                    case "4":
                        Console.Write("Podaj numer pomieszczenia do analizy: ");
                        if (int.TryParse(Console.ReadLine(), out int numer))
                        {
                            var p = system[numer];
                            if (p != null) NarzedziaHotelowe.WyswietlSzczegolyObiektu(p);
                            else Console.WriteLine("Nie ma takiego pomieszczenia.");
                        }
                        else Console.WriteLine("Nieprawidłowy numer.");
                        break;
                    case "5":
                        _ = SymulujSprzatanie();
                        break;
                    case "0":
                        Console.WriteLine("Zamykanie systemu...");
                        return;
                    default:
                        Console.WriteLine("Nieprawidłowa opcja.");
                        break;
                }
            }
        }

        static async Task SymulujSprzatanie()
        {
            Console.WriteLine("\nRozpoczynam nocne sprzątanie wszystkich pomieszczeń...");
            await Task.Delay(3000);
            Console.WriteLine("Sprzątanie zakończone pomyślnie.");
        }
    }
}
