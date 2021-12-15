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
        public Gioco(DatiCondivisi dati)
        {
            InitializeComponent();
            this.dati = dati;
        }

        private void btnSelectPokemon_Click(object sender, RoutedEventArgs e)
        {
            selectPokemon = new selectPokemon(dati, this);
            selectPokemon.Show();
            this.Hide();
        }
    }
}
