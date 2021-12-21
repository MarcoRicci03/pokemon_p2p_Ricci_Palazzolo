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
        CPokemon pokemonNemico;
        Gioco WPFGioco;
        int index = 0;
        public Lotta(DatiCondivisi datiConnessione, DatiCondivisiGioco datiGioco, Gioco WPFGioco)
        {
            InitializeComponent();
            this.datiConnessione = datiConnessione;
            this.datiGioco = datiGioco;
            this.WPFGioco = WPFGioco; //finestra precedente
            datiGioco.setWpfLotta(this);
            Thread aggGrafica = new Thread(datiGioco.aggGrafica);
            aggGrafica.Start();
            
        }

        private void btnMossa1_Click(object sender, RoutedEventArgs e)
        {
            datiConnessione.manda("m;" + datiGioco.pokemonAlleatoAttuale.move1.id);
        }

        private void btnMossa3_Click(object sender, RoutedEventArgs e)
        {
            datiConnessione.manda("m;" + datiGioco.pokemonAlleatoAttuale.move3.id);
        }

        private void btnMossa2_Click(object sender, RoutedEventArgs e)
        {
            datiConnessione.manda("m;" + datiGioco.pokemonAlleatoAttuale.move2.id);
        }

        private void btnMossa4_Click(object sender, RoutedEventArgs e)
        {
            datiConnessione.manda("m;" + datiGioco.pokemonAlleatoAttuale.move4.id);
        }
    }
}
