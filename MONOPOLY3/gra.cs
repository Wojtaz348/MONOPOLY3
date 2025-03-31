using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace MonopolyWPF.Model
{
    public class gra : INotifyPropertyChanged
    {
        private Random _random = new Random();
        private Gracz _aktualnyGracz;
        private int _aktualnyRzutKoscia;
        private string _komunikat;
        private bool _grajLokalnie;

        public plansza plansza { get; }
        public ObservableCollection<Gracz> Gracze { get; }

        public Gracz AktualnyGracz
        {
            get => _aktualnyGracz;
            set
            {
                _aktualnyGracz = value;
                OnPropertyChanged();

                
                foreach (var gracz in Gracze)
                {
                    gracz.AktywnyGracz = (gracz == value);
                }
            }
        }

        public int AktualnyRzutKoscia
        {
            get => _aktualnyRzutKoscia;
            set
            {
                _aktualnyRzutKoscia = value;
                OnPropertyChanged();
            }
        }

        public string Komunikat
        {
            get => _komunikat;
            set
            {
                _komunikat = value;
                OnPropertyChanged();
            }
        }

        public bool GrajLokalnie
        {
            get => _grajLokalnie;
            set
            {
                _grajLokalnie = value;
                OnPropertyChanged();
            }
        }

        public gra(bool grajLokalnie = true)
        {
            plansza = new plansza();
            Gracze = new ObservableCollection<Gracz>();
            GrajLokalnie = grajLokalnie;
            Komunikat = "Witaj w grze Monopoly!";
        }

        public void DodajGracza(Gracz gracz)
        {
            if (Gracze.Count < 8) 
            {
                Gracze.Add(gracz);
                if (Gracze.Count == 1)
                {
                    AktualnyGracz = gracz;
                }
                Komunikat = $"Dodano gracza: {gracz.Nazwa}";
            }
            else
            {
                Komunikat = "Osiągnięto maksymalną liczbę graczy!";
            }
        }

        public void UsunGracza(Gracz gracz)
        {
            if (Gracze.Contains(gracz))
            {
                
                foreach (var pole in gracz.PosiadanePola.ToList())
                {
                    pole.Wlasciciel = null;
                    pole.LiczbaDomkow = 0;
                    gracz.PosiadanePola.Remove(pole);
                }

                Gracze.Remove(gracz);

                if (AktualnyGracz == gracz && Gracze.Count > 0)
                {
                    NastepnyGracz();
                }

                Komunikat = $"Usunięto gracza: {gracz.Nazwa}";
            }
        }

        public void RozpocznijGre()
        {
            if (Gracze.Count >= 2)
            {
                AktualnyGracz = Gracze[0];
                Komunikat = $"Gra rozpoczęta! Tura gracza: {AktualnyGracz.Nazwa}";
            }
            else
            {
                Komunikat = "Potrzeba co najmniej 2 graczy, aby rozpocząć grę!";
            }
        }

        public int RzucKoscia()
        {
            int wynik = _random.Next(1, 7) + _random.Next(1, 7);
            AktualnyRzutKoscia = wynik;
            return wynik;
        }

        public void WykonajRuch()
        {
            if (AktualnyGracz == null)
            {
                Komunikat = "Gra nie została jeszcze rozpoczęta!";
                return;
            }

            if (AktualnyGracz.WWiezieniu)
            {
                ObslugaWiezienia();
                return;
            }

            int liczbaOczek = RzucKoscia();
            AktualnyGracz.PrzesunGracza(liczbaOczek);

            Pole aktualnePole = plansza.ZnajdzPole(AktualnyGracz.AktualnePole);
            Komunikat = $"{AktualnyGracz.Nazwa} wyrzucił {liczbaOczek} i wylądował na polu {aktualnePole.Nazwa}";

            ObslugaPola(aktualnePole);
        }

        private void ObslugaWiezienia()
        {
            int rzut = RzucKoscia();
            AktualnyGracz.TurWWiezieniu++;

            if (rzut % 2 == 0) 
            {
                AktualnyGracz.WWiezieniu = false;
                AktualnyGracz.TurWWiezieniu = 0;
                AktualnyGracz.PrzesunGracza(rzut);

                Pole aktualnePole = plansza.ZnajdzPole(AktualnyGracz.AktualnePole);
                Komunikat = $"{AktualnyGracz.Nazwa} wyrzucił {rzut}, wychodzi z więzienia i ląduje na polu {aktualnePole.Nazwa}";

                ObslugaPola(aktualnePole);
            }
            else if (AktualnyGracz.TurWWiezieniu >= 3)
            {
                AktualnyGracz.Pieniadze -= 50; 
                AktualnyGracz.WWiezieniu = false;
                AktualnyGracz.TurWWiezieniu = 0;
                AktualnyGracz.PrzesunGracza(rzut);

                Pole aktualnePole = plansza.ZnajdzPole(AktualnyGracz.AktualnePole);
                Komunikat = $"{AktualnyGracz.Nazwa} płaci 50$ i wychodzi z więzienia, wyrzucił {rzut} i ląduje na polu {aktualnePole.Nazwa}";

                ObslugaPola(aktualnePole);
            }
            else
            {
                Komunikat = $"{AktualnyGracz.Nazwa} wyrzucił {rzut} i pozostaje w więzieniu (tura {AktualnyGracz.TurWWiezieniu}/3)";
                NastepnyGracz();
            }
        }

        private void ObslugaPola(Pole pole)
        {
            switch (pole.Typ)
            {
                case TypPola.Start:
                    Komunikat += ". Gracz jest na polu Start.";
                    break;

                case TypPola.Nieruchomosc:
                case TypPola.Stacja:
                case TypPola.Zaklad:
                    if (pole.Wlasciciel == null)
                    {
                        if (AktualnyGracz.Pieniadze >= pole.Cena)
                        {
                            
                            Komunikat += $". Możesz kupić to pole za {pole.Cena}$.";
                        }
                        else
                        {
                            Komunikat += $". Nie masz wystarczająco pieniędzy, aby kupić to pole ({pole.Cena}$).";
                        }
                    }
                    else if (pole.Wlasciciel != AktualnyGracz)
                    {
                        int czynsz = pole.ObliczCzynsz();
                        AktualnyGracz.Pieniadze -= czynsz;
                        pole.Wlasciciel.Pieniadze += czynsz;
                        Komunikat += $". Płacisz {czynsz}$ czynszu dla {pole.Wlasciciel.Nazwa}.";
                        SprawdzBankructwo();
                    }
                    break;

                case TypPola.Podatek:
                    AktualnyGracz.Pieniadze -= pole.Czynsz;
                    Komunikat += $". Płacisz {pole.Czynsz}$ podatku.";
                    SprawdzBankructwo();
                    break;

                case TypPola.Wiezienie:
                    Komunikat += ". Jesteś tylko z wizytą w więzieniu.";
                    break;

                case TypPola.IdzDoWiezienia:
                    AktualnyGracz.AktualnePole = 10; 
                    AktualnyGracz.WWiezieniu = true;
                    AktualnyGracz.TurWWiezieniu = 0;
                    Komunikat += ". Idziesz do więzienia!";
                    break;

                case TypPola.Parking:
                    Komunikat += ". Jesteś na darmowym parkingu.";
                    break;

                case TypPola.Szansa:
                    ObslugaSzansy();
                    break;

                case TypPola.KasaSpołeczna:
                    ObslugaKasySpolecznej();
                    break;
            }

            
            SprawdzBankructwo();

            if (Gracze.Count > 1 && AktualnyGracz != null)
            {
                
                if (pole.Typ != TypPola.IdzDoWiezienia)
                {
                    NastepnyGracz();
                }
            }
        }

        private void ObslugaSzansy()
        {
            int los = _random.Next(0, 5);
            switch (los)
            {
                case 0:
                    AktualnyGracz.Pieniadze += 100;
                    Komunikat += ". Szansa: Wygrywasz 100$ w konkursie piękności!";
                    break;
                case 1:
                    AktualnyGracz.Pieniadze -= 150;
                    Komunikat += ". Szansa: Płacisz 150$ za naprawę samochodu!";
                    break;
                case 2:
                    AktualnyGracz.PrzesunGracza(3);
                    Komunikat += ". Szansa: Przesuwasz się o 3 pola do przodu!";
                    ObslugaPola(plansza.ZnajdzPole(AktualnyGracz.AktualnePole));
                    return; 
                case 3:
                    AktualnyGracz.AktualnePole = 0; 
                    AktualnyGracz.Pieniadze += 200;
                    Komunikat += ". Szansa: Idziesz na Start i otrzymujesz 200$!";
                    break;
                case 4:
                    AktualnyGracz.AktualnePole = 10; 
                    AktualnyGracz.WWiezieniu = true;
                    AktualnyGracz.TurWWiezieniu = 0;
                    Komunikat += ". Szansa: Idziesz do więzienia!";
                    break;
            }

            SprawdzBankructwo();
        }

        private void ObslugaKasySpolecznej()
        {
            int los = _random.Next(0, 5);
            switch (los)
            {
                case 0:
                    AktualnyGracz.Pieniadze += 200;
                    Komunikat += ". Kasa Społeczna: Otrzymujesz zwrot podatku 200$!";
                    break;
                case 1:
                    AktualnyGracz.Pieniadze -= 100;
                    Komunikat += ". Kasa Społeczna: Płacisz 100$ za wizytę u lekarza!";
                    break;
                case 2:
                    foreach (var gracz in Gracze)
                    {
                        if (gracz != AktualnyGracz)
                        {
                            gracz.Pieniadze += 50;
                            AktualnyGracz.Pieniadze -= 50;
                        }
                    }
                    Komunikat += ". Kasa Społeczna: Urządzasz przyjęcie! Płacisz każdemu graczowi 50$!";
                    break;
                case 3:
                    AktualnyGracz.Pieniadze += 50;
                    Komunikat += ". Kasa Społeczna: To Twoje urodziny! Otrzymujesz 50$!";
                    break;
                case 4:
                    int kwota = Gracze.Count * 10;
                    AktualnyGracz.Pieniadze += kwota;
                    Komunikat += $". Kasa Społeczna: Zbierasz datki na fundację! Otrzymujesz {kwota}$!";
                    break;
            }

            SprawdzBankructwo();
        }

        public void KupPole()
        {
            Pole pole = plansza.ZnajdzPole(AktualnyGracz.AktualnePole);

            if (pole.Wlasciciel == null &&
                (pole.Typ == TypPola.Nieruchomosc || pole.Typ == TypPola.Stacja || pole.Typ == TypPola.Zaklad))
            {
                if (AktualnyGracz.Pieniadze >= pole.Cena)
                {
                    AktualnyGracz.Pieniadze -= pole.Cena;
                    pole.Wlasciciel = AktualnyGracz;
                    AktualnyGracz.PosiadanePola.Add(pole);
                    Komunikat = $"{AktualnyGracz.Nazwa} kupił pole {pole.Nazwa} za {pole.Cena}$.";
                }
                else
                {
                    Komunikat = $"Nie masz wystarczająco pieniędzy, aby kupić to pole!";
                }
            }
        }

        public void KupDomek()
        {
            Pole pole = plansza.ZnajdzPole(AktualnyGracz.AktualnePole);

            if (pole.Wlasciciel == AktualnyGracz && pole.Typ == TypPola.Nieruchomosc && pole.LiczbaDomkow < 5)
            {
                int cenaDomku = pole.Cena / 2;

                if (AktualnyGracz.Pieniadze >= cenaDomku)
                {
                    AktualnyGracz.Pieniadze -= cenaDomku;
                    pole.LiczbaDomkow++;

                    if (pole.LiczbaDomkow == 5)
                    {
                        Komunikat = $"{AktualnyGracz.Nazwa} kupił hotel na polu {pole.Nazwa} za {cenaDomku}$.";
                    }
                    else
                    {
                        Komunikat = $"{AktualnyGracz.Nazwa} kupił domek na polu {pole.Nazwa} za {cenaDomku}$. Łącznie domków: {pole.LiczbaDomkow}";
                    }
                }
                else
                {
                    Komunikat = "Nie masz wystarczająco pieniędzy, aby kupić domek!";
                }
            }
        }

        public void NastepnyGracz()
        {
            if (Gracze.Count <= 1)
            {
                ZakonczGre();
                return;
            }

            int aktualnyIndeks = Gracze.IndexOf(AktualnyGracz);
            int nastepnyIndeks = (aktualnyIndeks + 1) % Gracze.Count;
            AktualnyGracz = Gracze[nastepnyIndeks];

            Komunikat = $"Tura gracza: {AktualnyGracz.Nazwa}";
        }

        private void SprawdzBankructwo()
        {
            if (AktualnyGracz.Pieniadze < 0)
            {
                Komunikat += $" {AktualnyGracz.Nazwa} zbankrutował!";
                UsunGracza(AktualnyGracz);

                if (Gracze.Count <= 1)
                {
                    ZakonczGre();
                }
            }
        }

        private void ZakonczGre()
        {
            if (Gracze.Count == 1)
            {
                Komunikat = $"Koniec gry! Zwycięzcą jest {Gracze[0].Nazwa}!";
            }
            else
            {
                Komunikat = "Koniec gry! Nie ma więcej graczy.";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
        public string SerializujStan()
        {
            StringBuilder sb = new StringBuilder();

            
            sb.AppendLine($"AktualnyGracz:{(AktualnyGracz != null ? AktualnyGracz.ID.ToString() : "null")}");

           
            sb.AppendLine($"LiczbaGraczy:{Gracze.Count}");
            foreach (var gracz in Gracze)
            {
                sb.AppendLine($"Gracz:{gracz.ID}|{gracz.Nazwa}|{gracz.Pieniadze}|{gracz.AktualnePole}|{gracz.WWiezieniu}|{gracz.TurWWiezieniu}|{gracz.Kolor}");
            }

            
            foreach (var pole in plansza.Pola)
            {
                string wlascicielId = pole.Wlasciciel != null ? pole.Wlasciciel.ID.ToString() : "null";
                sb.AppendLine($"Pole:{pole.Pozycja}|{wlascicielId}|{pole.LiczbaDomkow}");
            }

            
            sb.AppendLine($"Komunikat:{Komunikat}");

            return sb.ToString();
        }

        
        public void DeserializujStan(string stan)
        {
            var linie = stan.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, Gracz> mapowanieGraczy = new Dictionary<string, Gracz>();

            foreach (var linia in linie)
            {
                string[] czesci = linia.Split(':');
                if (czesci.Length < 2) continue;

                string typ = czesci[0];
                string wartosci = czesci[1];

                switch (typ)
                {
                    case "LiczbaGraczy":
                       
                        break;

                    case "Gracz":
                        string[] daneGracza = wartosci.Split('|');
                        if (daneGracza.Length >= 7)
                        {
                            string id = daneGracza[0];
                            Gracz gracz = Gracze.FirstOrDefault(g => g.ID.ToString() == id);

                            if (gracz == null)
                            {
                                gracz = new Gracz(daneGracza[1], daneGracza[6]);
                                Gracze.Add(gracz);
                            }
                            else
                            {
                                gracz.Nazwa = daneGracza[1];
                                gracz.Kolor = daneGracza[6];
                            }

                            gracz.Pieniadze = int.Parse(daneGracza[2]);
                            gracz.AktualnePole = int.Parse(daneGracza[3]);
                            gracz.WWiezieniu = bool.Parse(daneGracza[4]);
                            gracz.TurWWiezieniu = int.Parse(daneGracza[5]);

                            mapowanieGraczy[id] = gracz;
                        }
                        break;

                    case "AktualnyGracz":
                        if (wartosci != "null" && mapowanieGraczy.ContainsKey(wartosci))
                        {
                            AktualnyGracz = mapowanieGraczy[wartosci];
                        }
                        break;

                    case "Pole":
                        string[] danePola = wartosci.Split('|');
                        if (danePola.Length >= 3)
                        {
                            int pozycja = int.Parse(danePola[0]);
                            Pole pole = plansza.ZnajdzPole(pozycja);

                            if (danePola[1] != "null" && mapowanieGraczy.ContainsKey(danePola[1]))
                            {
                                pole.Wlasciciel = mapowanieGraczy[danePola[1]];
                                if (!mapowanieGraczy[danePola[1]].PosiadanePola.Contains(pole))
                                {
                                    mapowanieGraczy[danePola[1]].PosiadanePola.Add(pole);
                                }
                            }
                            else
                            {
                                pole.Wlasciciel = null;
                            }

                            pole.LiczbaDomkow = int.Parse(danePola[2]);
                        }
                        break;

                    case "Komunikat":
                        Komunikat = wartosci;
                        break;
                }
            }
        }
    }
}