using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SzymBus
{
    [Activity(Label = "Main3")]
    public class Main3 : Activity
    {
        private ListView przystanekListView;
        private ListView liniaListView;
        private ListView kierunekListView;
        private List<Przystanek> listaprzystankow; 
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main3);
            // Create your application here

            przystanekListView = FindViewById<ListView>(Resource.Id.przystanekListView);
            kierunekListView = FindViewById<ListView>(Resource.Id.kierunekListView);
            liniaListView = FindViewById<ListView>(Resource.Id.liniaListView);
            listaprzystankow = new List<Przystanek>();

            bool poczatek = false;
            bool poczatek2 = false;
            int i = -1;
            int j = 0;
            List<Funkcje.LinkItem> numery = new List<Funkcje.LinkItem>();
            List<string> nrList = new List<string>();
            List<string> przystanki = new List<string>();
            numery = Funkcje.Findlink(Funkcje.Htmlkierunek);
            foreach (var x in numery)
            {
                nrList.Add(x.ToString().Substring(x.ToString().IndexOf("nrp=") + 4, x.ToString().IndexOf("k=") - x.ToString().IndexOf("nrp=") - 9));
            }

            foreach (var x in Funkcje.Listaprzystankow)
            {
                if (!poczatek)
                {

                    poczatek = x.Equals(Funkcje.Wynik2);
                }
                else
                {
                    if (x.Substring(0, 8) != "kierunek")
                    {
                       // ComboBoxItem item = new ComboBoxItem();
                       // item.Content = x;
                       // item.Tag = nrList[i];
                       // PrzystanekComboBox.Items.Add(item);

                        Przystanek przystanek = new Przystanek(nrList[i],j++);
                        listaprzystankow.Add(przystanek);
                        przystanki.Add(x);

                    }
                    else
                    {
                        break;
                    }
                }
                i++;
                if (!poczatek2 && !poczatek && i > 0)
                {
                    i--;
                    poczatek2 = true;
                }
            }

            ArrayAdapter<string> ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, przystanki);
            przystanekListView.Adapter = ListAdapter;
            przystanekListView.ItemClick += PrzystanekListViewOnItemClick;


        }

        private void PrzystanekListViewOnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var wynik3 = listaprzystankow[e.Position].NrPrzystanku;
            Funkcje.Htmllink = "http://www.komunikacja.bialystok.pl/?page=przystanek&nrl=" + Funkcje.Wynik1 + "&nrp=" +
                               wynik3 + "&k=0&rozklad=";
            StartActivity(typeof (Rozklad));
            //string nazwa = Regex.Replace(nrp.Content.ToString(), @"\t|\n|\r", "");
            //string linia = Regex.Replace(LiniaComboBox.SelectedValue.ToString(), @"\t|\n|\r", "");
            //string kierunek = Regex.Replace(KierunkiComboBox.SelectedValue.ToString(), @"\t|\n|\r", "");
            // rozklad.MainLabel.Content = "Rozklad przystanku nr: " + nrp.Tag + ", " + nrp.Content + " dla linii nr: " + LiniaComboBox.SelectedValue + ", " + KierunkiComboBox.SelectedValue;
            //rozklad.MainLabel.Content = "Linia nr:" + linia + Environment.NewLine + "Przystanek nr: " + nrp.Tag +
            //                            ", " + nazwa + Environment.NewLine + kierunek;
        }
    }
}