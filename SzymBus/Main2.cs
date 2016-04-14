using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    [Activity(Label = "Main2")]
    public class Main2 : Activity
    {
        private List<string> samekierunkiList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main2);
            // Create your application here

            ListView kierunekListView = FindViewById<ListView>(Resource.Id.kierunekListView);


            kierunekListView.Adapter = null;
            // var kierunek = LiniaComboBox.SelectedItem;   // przekazac z poprzedniego okna
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            var htmlkierunek = webClient.DownloadString("http://www.komunikacja.bialystok.pl/?page=lista_przystankow&nr=" + Funkcje.Wynik1 + "&rozklad=");
            htmlkierunek = htmlkierunek.Substring(htmlkierunek.IndexOf("<div id=\"lista_przystankow\">"), htmlkierunek.LastIndexOf("</div><!--lista_przystankow-->") - htmlkierunek.IndexOf("<div id=\"lista_przystankow\">"));
            List<Funkcje.LinkItem> kierunki = new List<Funkcje.LinkItem>();
            kierunki = Funkcje.Findkierunek(htmlkierunek);
            List<string> listaprzystankow = null;
            listaprzystankow = new List<string>();
            string pomoc;
            foreach (var x in kierunki)
            {
                pomoc = Regex.Replace(x.ToString(), @"\t|\n|\r", "");
                listaprzystankow.Add(pomoc);
            }
             samekierunkiList= new List<string>();
            foreach (var x in listaprzystankow)
            {
                bool test1 = x.Substring(0, 8).Equals("kierunek");
                if (test1)
                {
                    samekierunkiList.Add(x);
                    //kierunekListView.Items.Add(x);
                }
            }
            ArrayAdapter<string> ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, samekierunkiList);
            Funkcje.Htmlkierunek = htmlkierunek;
            Funkcje.Listaprzystankow = listaprzystankow;
            kierunekListView.Adapter = ListAdapter;
            kierunekListView.ItemClick += KierunekListViewOnItemClick;
        }

        private void KierunekListViewOnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Funkcje.Wynik2 = samekierunkiList[e.Position];
            StartActivity(typeof(Main3));
        }
    }
}