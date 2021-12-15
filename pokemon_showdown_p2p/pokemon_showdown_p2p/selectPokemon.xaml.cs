using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace pokemon_showdown_p2p
{
    /// <summary>
    /// Logica di interazione per selectPokemon.xaml
    /// </summary>
    public partial class selectPokemon : Window
    {
        ListCollectionView collectionViewPokemon = null;
        DatiCondivisi dati;
        Window gioco;
        public selectPokemon(DatiCondivisi dati, Window gioco)
        {
            InitializeComponent();
            this.gioco = gioco;
            this.dati = dati;
            collectionViewPokemon = new ListCollectionView(dati.listPokemon);
            listBoxListaPokemon.DataContext = collectionViewPokemon;
            collectionViewPokemon.Refresh();
        }

        private void listBoxLista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BitmapImage bitimg = new BitmapImage();
            bitimg.BeginInit();
            bitimg.UriSource = new Uri(@"Properties/" + dati.listPokemon[listBoxListaPokemon.SelectedIndex].hires, UriKind.RelativeOrAbsolute);
            bitimg.EndInit();
            img.Source = bitimg;
        }

        private void btnAggiungi_Click(object sender, RoutedEventArgs e)
        {
            dati.listPokemonSelezionati.Add(dati.listPokemon[listBoxListaPokemon.SelectedIndex]);
            if (dati.listPokemonSelezionati.Count < 7)
            {
                BitmapImage bitimg = new BitmapImage();
                bitimg.BeginInit();
                bitimg.UriSource = new Uri(@"Properties/" + dati.listPokemonSelezionati[dati.listPokemonSelezionati.Count-1].hires, UriKind.RelativeOrAbsolute);
                bitimg.EndInit();
                switch (dati.listPokemonSelezionati.Count - 1)
                {
                   case 0:
                        imgPokemon1.Source = bitimg;
                        break;
                    case 1:
                        imgPokemon2.Source = bitimg;
                        break;
                    case 2:
                        imgPokemon3.Source = bitimg;
                        break;
                    case 3:
                        imgPokemon4.Source = bitimg;
                        break;
                    case 4:
                        imgPokemon5.Source = bitimg;
                        break;
                    case 5:
                        imgPokemon6.Source = bitimg;
                        break;
                }
            }
            else
            {
                lblMAX.Content = "Puoi selezionare massimo 6 pokemon";
                dati.listPokemonSelezionati.RemoveAt(dati.listPokemonSelezionati.Count - 1);
            }
                
        }

        private void btnConferma_Click(object sender, RoutedEventArgs e)
        {
            gioco.Show();
            this.Close();
        }

        private void btnRimuovi_Click(object sender, RoutedEventArgs e)
        {
            if(dati.listPokemonSelezionati.Count != 0)
            {
                if(dati.listPokemonSelezionati.Contains(listBoxListaPokemon.SelectedItem))
                {
                    dati.listPokemonSelezionati.Remove(dati.listPokemon[listBoxListaPokemon.SelectedIndex]);
                    for (int i = 0; i < dati.listPokemonSelezionati.Count; i++)
                    {
                        BitmapImage bitmapimage = new BitmapImage();
                        bitmapimage.BeginInit();
                        bitmapimage.UriSource = new Uri(@"Properties/" + dati.listPokemonSelezionati[i].hires, UriKind.RelativeOrAbsolute);
                        bitmapimage.EndInit();
                        if (i + 1 == dati.listPokemonSelezionati.Count)
                        {
                            switch (i + 1)
                            {
                                case 0:
                                    imgPokemon1.Source = null;
                                    break;
                                case 1:
                                    imgPokemon2.Source = null;
                                    break;
                                case 2:
                                    imgPokemon3.Source = null;
                                    break;
                                case 3:
                                    imgPokemon4.Source = null;
                                    break;
                                case 4:
                                    imgPokemon5.Source = null;
                                    break;
                                case 5:
                                    imgPokemon6.Source = null;
                                    break;
                            }
                        }
                        switch (i)
                        {
                            case 0:
                                imgPokemon1.Source = bitmapimage;
                                break;
                            case 1:
                                imgPokemon2.Source = bitmapimage;
                                break;
                            case 2:
                                imgPokemon3.Source = bitmapimage;
                                break;
                            case 3:
                                imgPokemon4.Source = bitmapimage;
                                break;
                            case 4:
                                imgPokemon5.Source = bitmapimage;
                                break;
                            case 5:
                                imgPokemon6.Source = bitmapimage;
                                break;
                        }
                    }
                }
            }
        }
    }
}