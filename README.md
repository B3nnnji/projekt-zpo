System Zarządzania Rezerwacjami w Hotelu

Jest to aplikacja konsolowa w C# symulująca prosty system do zarządzania rezerwacjami w hotelu.

Funkcjonalności

-   Zarządzanie różnymi typami pomieszczeń (pokoje hotelowe, sale konferencyjne).
-   Wyświetlanie listy wszystkich pomieszczeń w hotelu.
-   Sprawdzanie dostępności pomieszczeń w wybranym terminie.
-   Wykonywanie operacji specyficznych dla danego typu pomieszczenia (np. modyfikacja ceny).
-   Uruchamianie zadań w tle (symulacja sprzątania) bez blokowania interfejsu użytkownika.
-   Dynamiczna analiza właściwości obiektów w czasie działania programu.

Użyte Technologie

-   .NET 8
-   C#

Zaimplementowane Koncepty Programistyczne

1.  Klasy: `Pomieszczenie`, `PokojHotelowy`, `SalaKonferencyjna`, `SystemRezerwacji<T>`, `NarzedziaHotelowe`, `Program`.
2.  Konstruktory: W klasach `Pomieszczenie`, `PokojHotelowy` i `SalaKonferencyjna` służą do inicjalizacji obiektów.
3.  Właściwości / Indeksatory:
    -   Właściwości: `Numer`, `CenaZaNoc`, `LiczbaLozek` itp. hermetyzują dane.
    -   Indeksator: W klasie `SystemRezerwacji<T>` pozwala na wyszukiwanie pomieszczenia po jego numerze (`system[101]`).
4.  Statyczne: Klasa `NarzedziaHotelowe` i jej metoda `WyswietlSzczegolyObiektu()` są statyczne.
5.  Dziedziczenie: Klasy `PokojHotelowy` i `SalaKonferencyjna` dziedziczą po klasie `Pomieszczenie`.
6.  Polimorfizm: Metoda `WyswietlInformacje()` jest nadpisywana w klasach pochodnych, co widać w opcji menu "Wyświetl wszystkie pomieszczenia".
7.  Interfejsy / Abstrakcja:
    -   Interfejs: `IRezerwowalny` definiuje wspólny kontrakt.
    -   Abstrakcja: `Pomieszczenie` to klasa abstrakcyjna z abstrakcyjną metodą `PobierzTypPomieszczenia()`.
8.  Typy ogólne / Kolekcje: Klasa `SystemRezerwacji<T>` jest generyczna i używa `List<T>`.
9.  Delegacje / Zdarzenia: `SystemRezerwacji<T>` definiuje zdarzenie `PomieszczenieDodane`, które jest obsługiwane w klasie `Program`.
10. Przeciążanie operatorów: Operator `++` został przeciążony dla klasy `PokojHotelowy` w celu modyfikacji ceny.
11. Programowanie asynchroniczne: Metoda `SymulujSprzatanie()` używa `async`/`await` i jest wywoływana w sposób nieblokujący (`_ = SymulujSprzatanie()`).
12. Refleksja: Metoda `NarzedziaHotelowe.WyswietlSzczegolyObiektu()` używa refleksji do analizy obiektów.

Jak Uruchomić Projekt

1.  Upewnij się, że masz zainstalowany .NET 8 SDK.
2.  Otwórz terminal w głównym folderze projektu.
3.  Uruchom aplikację za pomocą polecenia:
    ```bash
    dotnet run
    ```
4.  Postępuj zgodnie z instrukcjami wyświetlanymi w menu konsoli, aby zarządzać systemem hotelowym.
