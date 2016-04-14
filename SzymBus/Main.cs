using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace SzymBus
{
    [Activity(Label = "Main", MainLauncher = true)]
    public class Main : Activity
    {
        private List<string> lista;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            // Create your application here

            ListView liniaListView = FindViewById<ListView>(Resource.Id.liniaListView);

            // pobieranie html
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string html = client.DownloadString("http://www.komunikacja.bialystok.pl/?page=rozklad_jazdy");

            string linie = html.Substring(html.IndexOf("<!-- START box: tabela_linii -->"),
                html.LastIndexOf("<!-- END box: tabela_linii -->") - html.IndexOf("<!-- START box: tabela_linii -->"));
            //koniec pobierania html
            //tworzenie listy na html i oddzielanie potrzebnej czesci
            List<Funkcje.LinkItem> numbers = new List<Funkcje.LinkItem>();
            numbers = Funkcje.Findlink(linie);
            lista = new List<string>();
            //koniec oddzielania
            //dodawanie do listy nr linii
            foreach (var x in numbers)
            {
                lista.Add(x.ToString().Substring(x.ToString().IndexOf("rozklad=") + 10));
            }
            //dodawanie to listview nr linii
            ArrayAdapter<string> ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, lista);
            liniaListView.Adapter = ListAdapter;
            liniaListView.ItemClick += LiniaListViewOnItemClick;

        }

        private void LiniaListViewOnItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Funkcje.Wynik1 = lista[e.Position];
            StartActivity(typeof(Main2));
        }
    }
}