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
        private ListCollectionView collectionViewPokemon = null;
        private DatiCondivisi dati;
        private DatiCondivisiGioco datiGioco;
        private Window gioco;
        public selectPokemon(DatiCondivisi dati, DatiCondivisiGioco datiGioco, Window gioco)
        {
            InitializeComponent();
            this.gioco = gioco;
            this.dati = dati;
            this.datiGioco = datiGioco;
            collectionViewPokemon = new ListCollectionView(datiGioco.listPokemon);
            listBoxListaPokemon.DataContext = collectionViewPokemon;
            collectionViewPokemon.Refresh();
            caricaLista();
            cmbTipi.Items.Add("");
            for (int i = 0; i < datiGioco.tipiPokemon.Count; i++)
            {
                cmbTipi.Items.Add(datiGioco.tipiPokemon[i]);

            }
        }
        //metodi per la lista
        private void caricaLista()
        {
            for (int i = 0; i < datiGioco.listPokemonSelezionati.Count; i++)
            {
                BitmapImage bitimg = new BitmapImage();
                bitimg.BeginInit();
                bitimg.UriSource = new Uri(@"Properties/" + datiGioco.listPokemonSelezionati[i].hires, UriKind.RelativeOrAbsolute);
                bitimg.EndInit();
                switch (i)
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
        }

        private void listBoxLista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CPokemon pokSel = datiGioco.listPokemon[datiGioco.trovaPokemonPerNome(listBoxListaPokemon.SelectedItem.ToString().Split(' ')[0])];
            txtDesc.Text = pokSel.description;
            BitmapImage bitimg = new BitmapImage();
            bitimg.BeginInit();
            bitimg.UriSource = new Uri(@"Properties/" + pokSel.hires, UriKind.RelativeOrAbsolute);
            bitimg.EndInit();
            img.Source = bitimg;
        }
        //metodo per l'aggiunta del pokemon alla lista di pokemon selezionati
        private void btnAggiungi_Click(object sender, RoutedEventArgs e)
        {
            //prendo il pokemon selezionato cercando la posizione del pokemon nella lista principale, questo serve nel caso dovesse essere attivo un filtro
            CPokemon pokSel = datiGioco.listPokemon[datiGioco.trovaPokemonPerNome(listBoxListaPokemon.SelectedItem.ToString().Split(' ')[0])];
            if (!datiGioco.listPokemonSelezionati.Contains(pokSel))
            {
                if (datiGioco.listPokemonSelezionati.Count < 6)
                {
                    datiGioco.listPokemonSelezionati.Add(pokSel);
                    BitmapImage bitimg = new BitmapImage();
                    bitimg.BeginInit();
                    bitimg.UriSource = new Uri(@"Properties/" + pokSel.hires, UriKind.RelativeOrAbsolute);
                    bitimg.EndInit();
                    switch (datiGioco.listPokemonSelezionati.Count - 1)
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
                }
            }
            else
            {
                lblDup.Content = "Non è possibile inserire due pokemon uguali.";
            }
        }
        //torno al menu
        private void btnConferma_Click(object sender, RoutedEventArgs e)
        {
            gioco.Show();
            this.Close();
        }
        //rimuovo il pokemon selezionato dalla lista di pokemon selezionati
        private void btnRimuovi_Click(object sender, RoutedEventArgs e)
        {
            CPokemon pokSel = datiGioco.listPokemon[datiGioco.trovaPokemonPerNome(listBoxListaPokemon.SelectedItem.ToString().Split(' ')[0])];
            if (datiGioco.listPokemonSelezionati.Count != 0)
            {
                if (datiGioco.listPokemonSelezionati.Contains(pokSel))
                {
                    datiGioco.listPokemonSelezionati.Remove(pokSel);
                    if (datiGioco.listPokemonSelezionati.Count > 0)
                    {
                        for (int i = 0; i < datiGioco.listPokemonSelezionati.Count; i++)
                        {
                            BitmapImage bitmapimage = new BitmapImage();
                            bitmapimage.BeginInit();
                            bitmapimage.UriSource = new Uri(@"Properties/" + datiGioco.listPokemonSelezionati[i].hires, UriKind.RelativeOrAbsolute);
                            bitmapimage.EndInit();
                            if (i + 1 == datiGioco.listPokemonSelezionati.Count)
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
                    else
                    {
                        imgPokemon1.Source = null;
                    }
                }
            }
        }
        //aggiorno la lista in base al filtro
        private void cmbTipi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            collectionViewPokemon.Filter = new Predicate<object>(predicatoTipo);
            collectionViewPokemon.Refresh();
        }
        //condizioni del filtro
        private bool predicatoTipo(object obj)
        {
            CPokemon p = obj as CPokemon;

            if (cmbTipi.SelectedIndex.Equals(0))
                return true;
            if (p.type1 == cmbTipi.SelectedItem.ToString() || p.type2 == cmbTipi.SelectedItem.ToString())
                return true;
            else
                return false;
        }
    }
}