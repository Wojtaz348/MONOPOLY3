using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace MonopolyWPF
{
    public class PozycjaGraczaXConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int pole && values[1] is double szerokosc)
            {
                // Oblicz położenie X na podstawie numeru pola
                double pojedynczaSzerokosc = szerokosc / 11; // 11 pól w poziomie (9 + 2 narożniki)

                // Start w lewym dolnym rogu (pole 0)
                if (pole >= 0 && pole <= 10)
                {
                    // Lewy brzeg planszy - ruch od dołu do góry
                    if (pole == 0)
                        return 0;
                    else if (pole == 10)
                        return 0;
                    else
                        return 0;
                }
                else if (pole >= 11 && pole <= 19)
                {
                    // Dolny brzeg planszy - ruch od prawej do lewej
                    double offsetX = pojedynczaSzerokosc + (19 - pole) * pojedynczaSzerokosc;
                    return offsetX;
                }
                else if (pole >= 20 && pole <= 30)
                {
                    // Prawy brzeg planszy - ruch od dołu do góry
                    if (pole == 20)
                        return szerokosc - pojedynczaSzerokosc;
                    else if (pole == 30)
                        return szerokosc - pojedynczaSzerokosc;
                    else
                        return szerokosc - pojedynczaSzerokosc;
                }
                else // pole >= 31 && pole <= 39
                {
                    // Górny brzeg planszy - ruch od prawej do lewej
                    double offsetX = pojedynczaSzerokosc + (pole - 31) * pojedynczaSzerokosc;
                    return offsetX;
                }
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PozycjaGraczaYConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int pole && values[1] is double wysokosc)
            {
                // Oblicz położenie Y na podstawie numeru pola
                double pojedynczaWysokosc = wysokosc / 11; // 11 pól w pionie (9 + 2 narożniki)

                // Start w lewym dolnym rogu (pole 0)
                if (pole >= 0 && pole <= 10)
                {
                    // Lewy brzeg planszy - ruch od dołu do góry
                    if (pole == 0)
                        return wysokosc - pojedynczaWysokosc;
                    else if (pole == 10)
                        return pojedynczaWysokosc;
                    else
                        return wysokosc - pojedynczaWysokosc - (pole * pojedynczaWysokosc);
                }
                else if (pole >= 11 && pole <= 19)
                {
                    // Dolny brzeg planszy - ruch od prawej do lewej
                    return wysokosc - pojedynczaWysokosc;
                }
                else if (pole >= 20 && pole <= 30)
                {
                    // Prawy brzeg planszy - ruch od dołu do góry
                    if (pole == 20)
                        return wysokosc - pojedynczaWysokosc;
                    else if (pole == 30)
                        return pojedynczaWysokosc;
                    else
                        return wysokosc - pojedynczaWysokosc - ((pole - 20) * pojedynczaWysokosc);
                }
                else // pole >= 31 && pole <= 39
                {
                    // Górny brzeg planszy - ruch od prawej do lewej
                    return pojedynczaWysokosc;
                }
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isActive && isActive)
            {
                return FontWeights.Bold;
            }
            return FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}