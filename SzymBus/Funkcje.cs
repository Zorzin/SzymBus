using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RegexOptions = Java.Util.Regex.RegexOptions;

namespace SzymBus
{

    class Przystanek
    {
        private string nr_przystanku;
        private int pozycja;

        public Przystanek(string nr_przystanku, int pozycja)
        {
            this.nr_przystanku = nr_przystanku;
            this.pozycja = pozycja;
        }

        public string NrPrzystanku
        {
            get { return nr_przystanku; }
            set { nr_przystanku = value; }
        }

        public int Pozycja
        {
            get { return pozycja; }
            set { pozycja = value; }
        }
    }

    class Funkcje
    {

        private static List<string> listaprzystankow;
        private static List<string> nrprzystanku;
        private static string htmlkierunek;
        private static string htmllink;
        private static string wynik1;
        private static string wynik2;

        public static string Htmllink
        {
            get { return htmllink; }
            set { htmllink = value; }
        }
        public static List<string> Listaprzystankow
        {
            get { return listaprzystankow; }
            set { listaprzystankow = value; }
        }

        public static List<string> Nrprzystanku
        {
            get { return nrprzystanku; }
            set { nrprzystanku = value; }
        }

        public static string Htmlkierunek
        {
            get { return htmlkierunek; }
            set { htmlkierunek = value; }
        }

        public static string Wynik1
        {
            get { return wynik1; }
            set { wynik1 = value; }
        }

        public static string Wynik2
        {
            get { return wynik2; }
            set { wynik2 = value; }
        }
        public struct LinkItem
        {
            public string Href;
            public string Text;

            public override string ToString()
            {
                return Href + "\n\t" + Text;
            }
        }

        public static List<LinkItem> Findlink(string file)
        {
            List<LinkItem> list = new List<LinkItem>();

            // 1.
            // Find all matches in file.
            MatchCollection m1 = Regex.Matches(file, @"(<a.*?>.*?</a>)",System.Text.RegularExpressions.RegexOptions.Singleline);

            // 2.
            // Loop over each match.
            foreach (Match m in m1)
            {
                string value = m.Groups[1].Value;
                LinkItem i = new LinkItem();

                // 3.
                // Get href attribute.
                Match m2 = Regex.Match(value, @"href=\""(.*?)\""", System.Text.RegularExpressions.RegexOptions.Singleline);
                if (m2.Success)
                {
                    i.Href = m2.Groups[1].Value;
                }

                // 4.
                // Remove inner tags from text.
                string t = Regex.Replace(value, @"\s*<.*?>\s*", "", System.Text.RegularExpressions.RegexOptions.Singleline);
                i.Text = t;

                list.Add(i);
            }
            return list;
        }

        public static List<LinkItem> Findkierunek(string file)
        {
            List<LinkItem> list = new List<LinkItem>();

            // 1.
            // Find all matches in file.
            MatchCollection m1 = Regex.Matches(file, @"(<td.*?>.*?</td>)", System.Text.RegularExpressions.RegexOptions.Singleline);

            // 2.
            // Loop over each match.
            foreach (Match m in m1)
            {
                string value = m.Groups[1].Value;
                LinkItem i = new LinkItem();

                // 3.
                // Get href attribute.
                Match m2 = Regex.Match(value, @"kierunek:\""(.*?)\""", System.Text.RegularExpressions.RegexOptions.Singleline);
                if (m2.Success)
                {
                    i.Href = m2.Groups[1].Value;
                }

                // 4.
                // Remove inner tags from text.
                string t = Regex.Replace(value, @"\s*<.*?>\s*", "", System.Text.RegularExpressions.RegexOptions.Singleline);
                i.Text = t;

                list.Add(i);
            }
            return list;
        }

        public static List<LinkItem> Findtbody(string file)
        {
            List<LinkItem> list = new List<LinkItem>();

            // 1.
            // Find all matches in file.
            MatchCollection m1 = Regex.Matches(file, @"(<span class=\""godzina\"">.*?</span>)", System.Text.RegularExpressions.RegexOptions.Singleline);

            // 2.
            // Loop over each match.
            foreach (Match m in m1)
            {
                string value = m.Groups[1].Value;
                LinkItem i = new LinkItem();

                // 3.
                // Get class attribute.
                Match m2 = Regex.Match(value, @"\""(.*?)\""", System.Text.RegularExpressions.RegexOptions.Singleline);
                if (m2.Success)
                {
                    i.Href = m2.Groups[1].Value;
                }

                // 4.
                // Remove inner tags from text.
                string t = Regex.Replace(value, @"\s*<.*?>\s*", "", System.Text.RegularExpressions.RegexOptions.Singleline);
                i.Text = t;

                list.Add(i);
            }
            return list;
        }

        public static string Findlegenda(string file)
        {


            // 1.
            // Find all matches in file.
            MatchCollection m1 = Regex.Matches(file, @"(<div id=\""legenda\"".*?>.*?</div>)", System.Text.RegularExpressions.RegexOptions.Singleline);

            // 2.
            // Loop over each match.
            foreach (Match m in m1)
            {
                string value = m.Groups[1].Value;
                LinkItem i = new LinkItem();

                // 3.
                // Get class attribute.
                Match m2 = Regex.Match(value, @"\""(.*?)\""", System.Text.RegularExpressions.RegexOptions.Singleline);
                if (m2.Success)
                {
                    i.Href = m2.Groups[1].Value;
                }

                // 4.
                // Remove inner tags from text.
                string t = Regex.Replace(value, @"\s*<.*?>\s*", "", System.Text.RegularExpressions.RegexOptions.Singleline);
                i.Text = t;

                file = i.Text;
            }
            return file;
        }
    }
}