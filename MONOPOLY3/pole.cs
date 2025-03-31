
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MonopolyWPF.Model
{
    public enum TypPola
    {
        Start,
        Nieruchomosc,
        Stacja,
        Zaklad,
        Szansa,
        KasaSpołeczna,
        Podatek,
        Wiezienie,
        Parking,
        IdzDoWiezienia
    }

    public class Pole : INotifyPropertyChanged
    {
        private string _nazwa;
        private int _cena;
        private int _czynsz;
        private string _kolor;
        private Gracz _wlasciciel;
        private TypPola _typ;
        private int _pozycja;
        private int _liczbaDomkow;

        public string Nazwa
        {
            get => _nazwa;
            set
            {
                _nazwa = value;
                OnPropertyChanged();
            }
        }

        public int Cena
        {
            get => _cena;
            set
            {
                _cena = value;
                OnPropertyChanged();
            }
        }

        public int Czynsz
        {
            get => _czynsz;
            set
            {
                _czynsz = value;
                OnPropertyChanged();
            }
        }

        public string Kolor
        {
            get => _kolor;
            set
            {
                _kolor = value;
                OnPropertyChanged();
            }
        }

        public Gracz Wlasciciel
        {
            get => _wlasciciel;
            set
            {
                _wlasciciel = value;
                OnPropertyChanged();
            }
        }

        public TypPola Typ
        {
            get => _typ;
            set
            {
                _typ = value;
                OnPropertyChanged();
            }
        }

        public int Pozycja
        {
            get => _pozycja;
            set
            {
                _pozycja = value;
                OnPropertyChanged();
            }
        }

        public int LiczbaDomkow
        {
            get => _liczbaDomkow;
            set
            {
                _liczbaDomkow = value;
                OnPropertyChanged();
            }
        }

        public Pole(string nazwa, int cena, int czynsz, string kolor, TypPola typ, int pozycja)
        {
            _nazwa = nazwa;
            _cena = cena;
            _czynsz = czynsz;
            _kolor = kolor;
            _typ = typ;
            _pozycja = pozycja;
            _liczbaDomkow = 0;
        }

        public int ObliczCzynsz()
        {
            if (Typ == TypPola.Nieruchomosc)
            {
                
                return Czynsz * (1 + LiczbaDomkow);
            }
            else if (Typ == TypPola.Stacja)
            {
                
                int liczbaStacji = 0;
                foreach (var pole in Wlasciciel.PosiadanePola)
                {
                    if (pole.Typ == TypPola.Stacja)
                    {
                        liczbaStacji++;
                    }
                }
                return Czynsz * liczbaStacji;
            }
            else if (Typ == TypPola.Zaklad)
            {
                
                int liczbaZakladow = 0;
                foreach (var pole in Wlasciciel.PosiadanePola)
                {
                    if (pole.Typ == TypPola.Zaklad)
                    {
                        liczbaZakladow++;
                    }
                }
                return Czynsz * liczbaZakladow;
            }

            return Czynsz;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
