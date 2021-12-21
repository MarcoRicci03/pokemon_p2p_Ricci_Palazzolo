using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace pokemon_showdown_p2p
{
    /// <summary>
    /// Logica di interazione per Lotta.xaml
    /// </summary>
    public partial class Lotta : Window
    {
        DatiCondivisi datiConnessione;
        DatiCondivisiGioco datiGioco = new DatiCondivisiGioco();
        Gioco WPFGioco;
        int indice;
        List<CPokemonConMosse> copiaListaPokemonConMosse;
        public Lotta(DatiCondivisi datiConnessione, DatiCondivisiGioco datiGioco, Gioco WPFGioco)
        {
            InitializeComponent();
            this.datiConnessione = datiConnessione;
            this.datiGioco = datiGioco;
            this.WPFGioco = WPFGioco; //finestra precedente
            datiGioco.aggiornaLotta(this);
            copiaListaPokemonConMosse = datiGioco.listPokemonSelezionatiConMosse;
            indice = 0;
        }

        private void btnMossa1_Click(object sender, RoutedEventArgs e)
        {
            datiConnessione.manda(copiaListaPokemonConMosse[indice].move1.id);
            

            Thread riceviConnessione = new Thread(datiConnessione.ricevi);
            // Name of the thread is Mythread
            riceviConnessione.Name = "Thread per ricezione";
            riceviConnessione.Start();
            // IsBackground is the property of Thread
            // which allows thread to run in the background
            riceviConnessione.IsBackground = true;
            
        }

        private void btnMossa3_Click(object sender, RoutedEventArgs e)
        {
            datiConnessione.manda(copiaListaPokemonConMosse[indice].move3.id);


            Thread riceviConnessione = new Thread(datiConnessione.ricevi);
            // Name of the thread is Mythread
            riceviConnessione.Name = "Thread per ricezione";
            riceviConnessione.Start();
            // IsBackground is the property of Thread
            // which allows thread to run in the background
            riceviConnessione.IsBackground = true;
        }

        private void btnMossa2_Click(object sender, RoutedEventArgs e)
        {
            datiConnessione.manda(copiaListaPokemonConMosse[indice].move2.id);


            Thread riceviConnessione = new Thread(datiConnessione.ricevi);
            // Name of the thread is Mythread
            riceviConnessione.Name = "Thread per ricezione";
            riceviConnessione.Start();
            // IsBackground is the property of Thread
            // which allows thread to run in the background
            riceviConnessione.IsBackground = true;
        }

        private void btnMossa4_Click(object sender, RoutedEventArgs e)
        {
            datiConnessione.manda(copiaListaPokemonConMosse[indice].move4.id);


            Thread riceviConnessione = new Thread(datiConnessione.ricevi);
            // Name of the thread is Mythread
            riceviConnessione.Name = "Thread per ricezione";
            riceviConnessione.Start();
            // IsBackground is the property of Thread
            // which allows thread to run in the background
            riceviConnessione.IsBackground = true;
        }
    }
}
