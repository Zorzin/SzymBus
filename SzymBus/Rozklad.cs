using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Provider;
using Android.Text;

namespace SzymBus
{
    [Activity(Label = "Rozklad")]
    public class Rozklad : Activity
    {
        private TableLayout tl;
        private List<Funkcje.LinkItem> lista;
        private List<Funkcje.LinkItem> lista2;  
        private void Dodajlinie(int j, int licznik)
        {
            TableRow tr = new TableRow(this);
            TableLayout.LayoutParams layoutParams =
                new TableLayout.LayoutParams(TableLayout.LayoutParams.FillParent,
                    TableLayout.LayoutParams.WrapContent);
            tr.LayoutParameters = layoutParams;
            TextView textView = new TextView(this);
            textView.Text = lista2[licznik].Text;
            tr.AddView(textView);
            TextView minutaTextView = new TextView(this);
            minutaTextView.SetPadding(100, 100, 300, 100);
            minutaTextView.Text = Rozdzielminuty(lista[j].Text, lista2[licznik].Text.Length);
            tr.AddView(minutaTextView);
            tl.AddView(tr);
        }
        private string Rozdzielminuty(string caly, int dlugosc)
        {
            string wyjscie="";
            string tekst = caly.Substring(dlugosc, caly.Length - dlugosc);
            int minuty;
            for (var i = 0; i < tekst.Length; i++)
            {

                if (i + 3 <= tekst.Length)
                {
                    var test = Int32.TryParse(tekst.Substring(i, 3), out minuty);
                    if (test)
                    {
                        wyjscie = wyjscie + tekst.Substring(i, 2) + " ";
                        i++;

                    }
                    else
                    {
                        wyjscie = wyjscie + tekst.Substring(i, 3) + " ";
                        i = i + 2;
                    }
                }
                else
                {
                    wyjscie = wyjscie + tekst.Substring(i, 2) + " ";
                    i = tekst.Length;
                }
            }
            return wyjscie;
        }
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.rozkladtab);
            // Create your application here
            tl = FindViewById<TableLayout>(Resource.Id.tabela);
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            var main = Resource.Layout.rozkladtab;


            var client = new WebClient();
            client.Encoding = Encoding.UTF8;
            var strona = client.DownloadString(Funkcje.Htmllink);
            var lista3 = Funkcje.Findlegenda(strona);
            var index = lista3.IndexOf("autobus niskopod³ogowy");
            var legenda = lista3.IndexOf("legenda");
            if (legenda == -1)
            {
                legenda = lista3.IndexOf("Legenda");
            }
            if (index != -1)
            {
                lista3 = lista3.Substring(0, index);
                lista3 = lista3.Substring(0, lista3.Length - 4);
            }
            else
            {
                if (legenda != -1)
                {
                    lista3 = lista3.Substring(0, lista3.IndexOf("00"));
                }
                else
                {
                    lista3 = null;
                }
            }
            if (lista3 != null)
            {
                char[] nlista3 = lista3.ToCharArray();

                for (int k = 0; k < nlista3.Length; k++)
                {
                    if (nlista3[k] == '\n')
                    {
                        var pom = nlista3[k - 1];
                        nlista3[k - 1] = nlista3[k];
                        nlista3[k] = pom;
                    }
                }
                lista3 = String.Join("", nlista3);
            }
            strona = strona.Substring(strona.IndexOf("id=\"rozklad_1\""),
                strona.LastIndexOf("</table>") - strona.IndexOf("id=\"rozklad_1\""));
            lista = new List<Funkcje.LinkItem>();
            lista2 = new List<Funkcje.LinkItem>();

            var dzien = new string[3];




            lista = Funkcje.Findkierunek(strona);
            lista2 = Funkcje.Findtbody(strona);
            dzien[0] = lista[0].Text;
            int j = 1;
            int j2 = 0;
            int j3 = 0;
            int licznik = 0;
            int licznik2 = 0;
            int licznik3 = 0;
            bool ktory=false;
            int petla = -1;
            foreach (var x in lista)
            {
                bool par = Regex.IsMatch(x.Text, @"^[a-zA-Z\s]");
                if (petla == -1)
                {
                    petla = 1;
                    par = false;
                }
                if (par)
                {
                    if (!ktory)
                    {
                        j2 = petla;
                        licznik2 = petla-2;
                        ktory = true;
                        dzien[1] = lista[petla].Text;
                    }
                    else
                    {
                        j3 = petla;
                        licznik3 = petla-3;
                        dzien[2] = lista[petla-1].Text;
                    }
                }
                petla++;
            }
            ActionBar.Tab tab = ActionBar.NewTab();
            tab.SetText(dzien[0]);
            tab.TabSelected += (sender, args) =>
            {
                SetContentView(main);
                tl = FindViewById<TableLayout>(Resource.Id.tabela);
                tl.RemoveAllViews();
                int j1 = j;
                int licznik1 = licznik;
                int nastepny;
                while (Int32.Parse(lista2[licznik1].Text) < Int32.Parse(lista2[licznik1 + 1].Text))
                {
                    if (licznik1 < lista2.Count)
                    {
                        Dodajlinie(j1,licznik1);
                        j1++;
                        licznik1++;
                    }
                    bool czyliczba = Int32.TryParse(lista2[licznik1 + 1].Text, out nastepny);
                    if (!czyliczba)
                    {
                        break;
                    }

                }
                Dodajlinie(j1, licznik1);
                j1++;
                licznik1++;
                dzien[1] = lista[j1++].Text;
            };
           ActionBar.AddTab(tab);
            
            ActionBar.Tab tab2 = ActionBar.NewTab();
            tab2.SetText(dzien[1]);
            tab2.TabSelected += (sender, args) =>
            {
                SetContentView(main);
                tl = FindViewById<TableLayout>(Resource.Id.tabela);
                tl.RemoveAllViews();
                int jj = j2;
                int licznikl = licznik2;
                int nastepny;
                if (licznikl < lista2.Count)
                {
                    while (Int32.Parse(lista2[licznikl].Text) < Int32.Parse(lista2[licznikl + 1].Text))
                    {
                        if (licznikl < lista2.Count)
                        {
                            Dodajlinie(jj, licznikl);
                            jj++;
                            licznikl++;

                        }
                        bool czyliczba = Int32.TryParse(lista2[licznikl + 1].Text, out nastepny);
                        if (!czyliczba)
                        {
                            break;
                        }

                    }
                }
                if (licznikl < lista2.Count)
                {
                    Dodajlinie(jj, licznikl);
                    jj++;
                    licznikl++;
                }

            };
            ActionBar.AddTab(tab2);

            ActionBar.Tab tab3 = ActionBar.NewTab();
            tab3.SetText(dzien[2]);
            tab3.TabSelected += (sender, args) =>
            {
                SetContentView(main);
                tl = FindViewById<TableLayout>(Resource.Id.tabela);
                tl.RemoveAllViews();
                int jjj = j3;
                int licznikll = licznik3;
                int nastepny;
                while (licznikll<lista2.Count)
                {
                    Dodajlinie(jjj, licznikll);
                    jjj++;
                    licznikll++;
                    if (licznikll + 1 < lista2.Count)
                    {
                        bool czyliczba = Int32.TryParse(lista2[licznikll + 1].Text, out nastepny);
                        if (!czyliczba)
                        {
                            break;
                        }
                    }
                }
                

            };
            ActionBar.AddTab(tab3);

            
            ActionBar.Tab tab4 = ActionBar.NewTab();
            tab4.SetText("Legenda");
            tab4.TabSelected += (sender, args) =>
            {
                SetContentView(Resource.Layout.Legenda);
                tl.RemoveAllViews();
                TextView textView = FindViewById<TextView>(Resource.Id.LegendaText);
                textView.SetHorizontallyScrolling(false);
                textView.SetRawInputType(InputTypes.TextFlagMultiLine);
                textView.Text = lista3;
                textView.Gravity=GravityFlags.CenterVertical;

            };
            ActionBar.AddTab(tab4);
        }
    }
}