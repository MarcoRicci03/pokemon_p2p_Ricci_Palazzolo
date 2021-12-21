using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Logica di interazione per Gioco.xaml
    /// </summary>
    public partial class Gioco : Window
    {
        public static Window selectPokemon;
        DatiCondivisi dati;
        DatiCondivisiGioco datiGioco;
        public Gioco(DatiCondivisi dati, DatiCondivisiGioco datiGioco)
        {
            InitializeComponent();
            this.dati = dati;
            this.datiGioco = datiGioco;
        }

        private void btnSelectPokemon_Click(object sender, RoutedEventArgs e)
        {
            selectPokemon = new selectPokemon(dati, datiGioco, this);
            selectPokemon.Show();
            this.Hide();
        }

        private void btnLotta_Click(object sender, RoutedEventArgs e)
        {
            datiGioco.assegnaMosse();
            Lotta fLotta = new Lotta();
            fLotta.Show();
            this.Hide();
        }
    }
}
