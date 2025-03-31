using System.Collections.Generic;
using System.Linq;

namespace MonopolyWPF.Model
{
    public class plansza
    {
        public List<Pole> Pola { get; private set; }

        public plansza()
        {
            InicjalizujPlansze();
        }

        private void InicjalizujPlansze()
        {
            Pola = new List<Pole>
            {
                
                new Pole("Start", 0, 0, "biały", TypPola.Start, 0),
                new Pole("Bulwar Południowy", 60, 2, "brązowy", TypPola.Nieruchomosc, 1),
                new Pole("Kasa Społeczna", 0, 0, "biały", TypPola.KasaSpołeczna, 2),
                new Pole("Aleje Jerozolimskie", 60, 4, "brązowy", TypPola.Nieruchomosc, 3),
                new Pole("Podatek dochodowy", 0, 200, "biały", TypPola.Podatek, 4),
                new Pole("Dworzec Zachodni", 200, 25, "biały", TypPola.Stacja, 5),
                new Pole("Ulica Konopacka", 100, 6, "niebieski", TypPola.Nieruchomosc, 6),
                new Pole("Szansa", 0, 0, "biały", TypPola.Szansa, 7),
                new Pole("Ulica Stalowa", 100, 6, "niebieski", TypPola.Nieruchomosc, 8),
                new Pole("Ulica Radzymińska", 120, 8, "niebieski", TypPola.Nieruchomosc, 9),
                new Pole("Więzienie", 0, 0, "biały", TypPola.Wiezienie, 10),
                new Pole("Ulica Marszałkowska", 140, 10, "różowy", TypPola.Nieruchomosc, 11),
                new Pole("Elektrownia", 150, 4, "biały", TypPola.Zaklad, 12),
                new Pole("Aleje Ujazdowskie", 140, 10, "różowy", TypPola.Nieruchomosc, 13),
                new Pole("Plac Trzech Krzyży", 160, 12, "różowy", TypPola.Nieruchomosc, 14),
                new Pole("Dworzec Gdański", 200, 25, "biały", TypPola.Stacja, 15),
                new Pole("Ulica Płowiecka", 180, 14, "pomarańczowy", TypPola.Nieruchomosc, 16),
                new Pole("Kasa Społeczna", 0, 0, "biały", TypPola.KasaSpołeczna, 17),
                new Pole("Ulica Grochowska", 180, 14, "pomarańczowy", TypPola.Nieruchomosc, 18),
                new Pole("Ulica Obozowa", 200, 16, "pomarańczowy", TypPola.Nieruchomosc, 19),
                new Pole("Parking", 0, 0, "biały", TypPola.Parking, 20),
                new Pole("Ulica Wolska", 220, 18, "czerwony", TypPola.Nieruchomosc, 21),
                new Pole("Szansa", 0, 0, "biały", TypPola.Szansa, 22),
                new Pole("Ulica Mickiewicza", 220, 18, "czerwony", TypPola.Nieruchomosc, 23),
                new Pole("Plac Wilsona", 240, 20, "czerwony", TypPola.Nieruchomosc, 24),
                new Pole("Dworzec Wschodni", 200, 25, "biały", TypPola.Stacja, 25),
                new Pole("Ulica Świętokrzyska", 260, 22, "żółty", TypPola.Nieruchomosc, 26),
                new Pole("Krakowskie Przedmieście", 260, 22, "żółty", TypPola.Nieruchomosc, 27),
                new Pole("Wodociągi", 150, 4, "biały", TypPola.Zaklad, 28),
                new Pole("Nowy Świat", 280, 24, "żółty", TypPola.Nieruchomosc, 29),
                new Pole("Idź do więzienia", 0, 0, "biały", TypPola.IdzDoWiezienia, 30),
                new Pole("Plac Teatralny", 300, 26, "zielony", TypPola.Nieruchomosc, 31),
                new Pole("Ulica Wiejska", 300, 26, "zielony", TypPola.Nieruchomosc, 32),
                new Pole("Kasa Społeczna", 0, 0, "biały", TypPola.KasaSpołeczna, 33),
                new Pole("Aleje Solidarności", 320, 28, "zielony", TypPola.Nieruchomosc, 34),
                new Pole("Dworzec Centralny", 200, 25, "biały", TypPola.Stacja, 35),
                new Pole("Szansa", 0, 0, "biały", TypPola.Szansa, 36),
                new Pole("Ulica Belwederska", 350, 35, "granatowy", TypPola.Nieruchomosc, 37),
                new Pole("Podatek od wzbogacenia", 0, 100, "biały", TypPola.Podatek, 38),
                new Pole("Aleje Jerozolimskie", 400, 50, "granatowy", TypPola.Nieruchomosc, 39)
            };
        }

        public Pole ZnajdzPole(int pozycja)
        {
            return Pola.FirstOrDefault(p => p.Pozycja == pozycja);
        }
    }
}
