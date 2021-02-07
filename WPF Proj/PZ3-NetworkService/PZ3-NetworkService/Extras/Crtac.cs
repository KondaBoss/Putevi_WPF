using PZ3_NetworkService.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace PZ3_NetworkService.Extras
{
    public static class Crtac
    {
        #region Konstante

        private const double _maxSirina = 570;
        private const double _maxVisina = 330;
        private const double _margina1 = 50;
        private const double _margina2 = 10;
        private const double _margina3 = 10;
        private const double _margina4 = 30;
        private const double _duzinaCrtice = 5;
        private const double _duzinaStrelice = 10;
        private const double _sirinaYTeksta = 33;
        private const double _sirinaXTeksta = 43;
        private const double _marginaTeksta = 3;
        private const double _textXOffset = 0;
        private const double _textYOffset = 8;

        #endregion

        #region Svojstva

        public static double X0 { get { return _margina1; } }

        public static double XEnd { get { return _maxSirina - _margina3; } }

        public static double XLimit { get { return XEnd - 2 * _duzinaStrelice; } }

        public static double Y0 { get { return _maxVisina - _margina4; } }

        public static double YEnd { get { return _margina2; } }

        public static double YLimit { get { return YEnd - 2 * _duzinaStrelice; } }
        public static double TextXPozicijaYOsa { get { return X0 - _sirinaYTeksta - _marginaTeksta - _duzinaCrtice; } }
        public static double TextYPozicijaXOsa { get { return Y0 + _textXOffset + _marginaTeksta + _duzinaCrtice; } }

        #endregion

        #region Metode

        public static void IscrtajGrafik(ObservableCollection<GraphObjectModel> kolekcija, ObservableCollection<StanjeModel> promene)
        {
            DopuniPodatke(promene);

            kolekcija.Clear();
            kolekcija.Add(NewLine(X0 - _duzinaCrtice, Y0, XEnd, Y0));
            kolekcija.Add(NewLine(XEnd, Y0, XEnd - _duzinaStrelice, Y0 - _duzinaCrtice));
            kolekcija.Add(NewLine(XEnd, Y0, XEnd - _duzinaStrelice, Y0 + _duzinaCrtice));
            kolekcija.Add(NewLine(X(1), Y0, X(1), Y0 + _duzinaCrtice));
            kolekcija.Add(NewLine(X(2), Y0, X(2), Y0 + _duzinaCrtice));
            kolekcija.Add(NewLine(X(3), Y0, X(3), Y0 + _duzinaCrtice));
            kolekcija.Add(NewLine(X(4), Y0, X(4), Y0 + _duzinaCrtice));
            kolekcija.Add(NewLine(X0, Y0 + _duzinaCrtice, X0, YEnd));
            kolekcija.Add(NewLine(X0, YEnd, X0 - _duzinaCrtice, YEnd + _duzinaStrelice));
            kolekcija.Add(NewLine(X0, YEnd, X0 + _duzinaCrtice, YEnd + _duzinaStrelice));
            kolekcija.Add(NewLine(X0, Y(5000), X0 - _duzinaCrtice, Y(5000)));
            kolekcija.Add(NewLine(X0, Y(10000), X0 - _duzinaCrtice, Y(10000)));
            kolekcija.Add(NewLine(X0, Y(15000), X0 - _duzinaCrtice, Y(15000)));
            kolekcija.Add(NewLine(X0, Y(20000), X0 - _duzinaCrtice, Y(20000)));
            kolekcija.Add(NewYText("5000", TextXPozicijaYOsa, Y(5000) - _textYOffset));
            kolekcija.Add(NewYText("10000", TextXPozicijaYOsa, Y(10000) - _textYOffset));
            kolekcija.Add(NewYText("15000", TextXPozicijaYOsa, Y(15000) - _textYOffset));
            kolekcija.Add(NewYText("20000", TextXPozicijaYOsa, Y(20000) - _textYOffset));

            var pozicija = 0;
            var prethodnoStanje = promene[0];
            foreach (var stanje in promene.OrderBy(p => p.Vreme))
            {
                kolekcija.Add(NewXText(stanje.Vreme.ToString("hh:mm:ss"), X(pozicija) - _sirinaXTeksta / 2, TextYPozicijaXOsa));
                if(pozicija > 0)
                {
                    kolekcija.Add(NewLine(X(pozicija - 1), Y(prethodnoStanje.BrojVozila), X(pozicija), Y(stanje.BrojVozila), 1, Brushes.DarkBlue));
                }
                prethodnoStanje = stanje;
                pozicija++;
            }
        }

        private static void DopuniPodatke(ObservableCollection<StanjeModel> promene)
        {
            if (promene.Count < 5)
            {
                var now = DateTime.Now;
                for (int i = promene.Count; i < 5; i++)
                {
                    promene.Add(new StanjeModel() { ID = 0, Vreme = now, IDPuta = 0, BrojPuta = string.Empty, BrojVozila = 0 });
                }
            }
        }

        public static LineModel NewLine(double x1, double y1, double x2, double y2, double strokeThickness = 1)
        {
            return NewLine(x1, y1, x2, y2, strokeThickness, Brushes.Black);
        }

        public static LineModel NewLine(double x1, double y1, double x2, double y2, double strokeThickness, Brush brush)
        {
            return new LineModel() { X1 = x1, Y1 = y1, X2 = x2, Y2 = y2, StrokeThickness = strokeThickness, Stroke = brush };
        }

        public static TextModel NewYText(string text, double x, double y)
        {
            return new TextModel() { Text = text, Margin = new Thickness(x, y, 0, 0), Width = _sirinaYTeksta, TextAlignment = TextAlignment.Right };
        }

        public static TextModel NewXText(string text, double x, double y)
        {
            return new TextModel() { Text = text, Margin = new Thickness(x, y, 0, 0), Width = _sirinaXTeksta, TextAlignment = TextAlignment.Center };
        }

        public static double X(int brTacke)
        {
            return X0 + brTacke * (XLimit - X0) / 4;
        }

        public static double Y(double vrednost)
        {
            return -0.0135 * vrednost + 300;
        }

        #endregion
    }
}
