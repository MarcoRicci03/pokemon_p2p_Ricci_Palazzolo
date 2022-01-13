using System;
using System.Collections.Generic;
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
        DatiCondivisiGioco datiGioco;
        Window gioco;
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
            for (int i = 0 ; i< datiGioco.tipiPokemon.Count; i++)
            {
                cmbTipi.Items.Add(datiGioco.tipiPokemon[i]);

            }
            
        }

        public void caricaLista() {
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

        private void btnAggiungi_Click(object sender, RoutedEventArgs e)
        {
            CPokemon pokSel = datiGioco.listPokemon[datiGioco.trovaPokemonPerNome(listBoxListaPokemon.SelectedItem.ToString().Split(' ')[0])];
            if (!datiGioco.listPokemonSelezionati.Contains(pokSel))
            {
                datiGioco.listPokemonSelezionati.Add(pokSel);
                if (datiGioco.listPokemonSelezionati.Count < 7)
                {
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
                    datiGioco.listPokemonSelezionati.RemoveAt(datiGioco.listPokemonSelezionati.Count - 1);
                }
            }
            else
            {
                lblDup.Content = "Non è possibile inserire due pokemon uguali.";
            }
        }

        private void btnConferma_Click(object sender, RoutedEventArgs e)
        {
            gioco.Show();
            this.Close();
        }

        private void btnRimuovi_Click(object sender, RoutedEventArgs e)
        {
            if (datiGioco.listPokemonSelezionati.Count != 0)
            {
                if (datiGioco.listPokemonSelezionati.Contains(listBoxListaPokemon.SelectedItem))
                {
                    datiGioco.listPokemonSelezionati.Remove(datiGioco.listPokemon[listBoxListaPokemon.SelectedIndex]);
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
                    } else
                    {
                        imgPokemon1.Source = null;
                    }
                }
            }
        }

        private void cmbTipi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            collectionViewPokemon.Filter = new Predicate<object>(predicatoTipo);
            collectionViewPokemon.Refresh();
        }

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