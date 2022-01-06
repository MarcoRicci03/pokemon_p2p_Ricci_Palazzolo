using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace pokemon_showdown_p2p
{
    public class DatiCondivisiGioco
    {
        public bool mioTurno { get; set; }
        public List<CMoves> listMosse { get; set; } //lista delle mosse
        public List<CPokemon> listPokemon { get; set; } //lista di tutti i pokemon basica
        public List<CPokemon> listPokemonSelezionati { get; set; } //lista pokemon selezionati basica
        public List<CPokemonConMosse> listPokemonSelezionatiConMosse { get; set; } //lista dei pokemon selezionati con mosse assegnate

        Lotta wpfLotta;

        public DatiCondivisi datiConnessione;
        int index = 0;


        public DatiCondivisiGioco(DatiCondivisi datiConnessione)
        {
            loadDataFromJSON();
            this.datiConnessione = datiConnessione;
        }
        public CPokemon searchListPokemon(int idEsterno)
        {
            CPokemon pokemonReturn = null;
            foreach (CPokemon pokemon in listPokemon)
            {
                if (pokemon.id == idEsterno)
                {
                    pokemonReturn = pokemon;
                }
            }
            return pokemonReturn;
        }
        public CMoves searchListMoves(int idEsterno)
        {
            CMoves moveReturn = null;
            foreach (CMoves moves in listMosse)
            {
                if (moves.id == idEsterno)
                {
                    moveReturn = moves;
                }
            }
            return moveReturn;
        }
        public void setWpfLotta(Lotta wpfLotta)
        {
            this.wpfLotta = wpfLotta;
        }

        public void setDatiConnessione(DatiCondivisi datiConnessione)
        {
            this.datiConnessione = datiConnessione;
        }
        public CPokemon pokemonNemicoAttuale { get; set; }
        public CPokemonConMosse pokemonAlleatoAttuale { get; set; }
        private BitmapImage bitimg = null;
        private readonly object bitimgLock = new object();

        public BitmapImage getBitimg()
        {
            lock (bitimgLock)
            {
                return bitimg;
            }
        }
        public int pippo = 0;
        public void aggGrafica()
        {
            string[] temp;
            int dannoRicevuto = 0;
            datiConnessione.manda("p;" + datiConnessione.peerQuesto.port_peer + ";" + listPokemonSelezionati[index].id);
            datiConnessione.ricevi();
            temp = datiConnessione.risAscolto;
            datiConnessione.risAscolto = null;
            if (temp[0] == "p")
            {
                pokemonNemicoAttuale = searchListPokemon(Int32.Parse(temp[2]));
                pokemonAlleatoAttuale = listPokemonSelezionatiConMosse[index];
                refreshMyPokemon(index);
            }
            if (temp[0] == "m")
            {
                //pokemonAlleatoAttuale = listPokemonSelezionatiConMosse[index];
                dannoRicevuto = searchListMoves(Int32.Parse(temp[2])).power;
                pokemonAlleatoAttuale.pokemonScelto.HP -= dannoRicevuto;
                wpfLotta.pBNostra.Value = pokemonAlleatoAttuale.pokemonScelto.HP;
                if (pokemonAlleatoAttuale.pokemonScelto.HP <= 0)
                {
                    //pokemon sconfitto
                    if (index < 5)//passo al pokemon dopo
                    {
                        index++;
                        refreshMyPokemon(index);
                    }
                    else
                    {
                        //invio pacchetto con sconfitta
                    }
                }
            }
        }

        public void aggPokemon()
        {

        }
        public void loadDataFromJSON()
        {
            //leggiamo dati da file JSON
            StreamReader r = new StreamReader("moves.json");
            string jsonString = r.ReadToEnd();
            listMosse = JsonConvert.DeserializeObject<List<CMoves>>(jsonString);

            r = new StreamReader("pokemon.json");
            jsonString = r.ReadToEnd();
            listPokemon = JsonConvert.DeserializeObject<List<CPokemon>>(jsonString);
            //inizializzo liste
            listPokemonSelezionati = new List<CPokemon>();
            foreach(CPokemon pokemon in listPokemon)
            {
                pokemon.HP = pokemon.HP * 3;
            }
        }

        public void assegnaMosse()
        {
            listPokemonSelezionatiConMosse = new List<CPokemonConMosse>();
            List<CMoves> listMosseTipo = new List<CMoves>();
            CPokemonConMosse pl;
            Random r = new Random();
            List<CMoves> t;

            //ciclo che scorre tutte le mosse in relazione a un pokemon
            for (int i = 0; i < listPokemonSelezionati.Count; i++)
            {
                t = new List<CMoves>();
                for (int j = 0; j < listMosse.Count; j++)
                {
                    if (listPokemonSelezionati[i].type1 == listMosse[j].type || listPokemonSelezionati[i].type2 == listMosse[j].type)
                    {
                        listMosseTipo.Add(listMosse[j]);
                    }
                }
                for (int h = 0; h < 4; h++)
                {
                    CMoves m = listMosseTipo[r.Next(0, listMosseTipo.Count)];
                    if (!t.Contains(m))
                    {
                        t.Add(m);
                    }
                    else
                    {
                        h--;
                    }
                }
                CPokemonConMosse tempPokemon = new CPokemonConMosse(listPokemonSelezionati[i], t[0], t[1], t[2], t[3]);
                listPokemonSelezionatiConMosse.Add(tempPokemon);
            }
        }

        public void scegliTurno() {
            Random r = new Random();
            int nRandom;
            bool finito = false;
            do
            {
                nRandom = r.Next(0, 2);
                nRandom = 1;
                datiConnessione.manda("t;" + datiConnessione.peerQuesto.port_peer + ";" + nRandom.ToString());
                datiConnessione.ricevi();
                if (datiConnessione.risAscolto[0] == "t")
                {
                    if (nRandom == 1 && Int32.Parse(datiConnessione.risAscolto[2]) == 0)
                    {
                        mioTurno = true;
                        finito = true;
                    }
                    else if (nRandom == 0 && Int32.Parse(datiConnessione.risAscolto[2]) == 1)
                    {
                        mioTurno = false;
                        finito = true;
                    }
                }
            } while (!finito);
            
        }


        public void refreshMyPokemon(int index)
        {
            //aggiorno pokemon nostri
            wpfLotta.btnMossa1.Content = listPokemonSelezionatiConMosse[index].move1.ename + " " + listPokemonSelezionatiConMosse[index].move1.power.ToString();
            wpfLotta.btnMossa2.Content = listPokemonSelezionatiConMosse[index].move2.ename + " " + listPokemonSelezionatiConMosse[index].move2.power.ToString();
            wpfLotta.btnMossa3.Content = listPokemonSelezionatiConMosse[index].move3.ename + " " + listPokemonSelezionatiConMosse[index].move3.power.ToString();
            wpfLotta.btnMossa4.Content = listPokemonSelezionatiConMosse[index].move4.ename + " " + listPokemonSelezionatiConMosse[index].move4.power.ToString();
            wpfLotta.lblHPAlleato.Content = listPokemonSelezionatiConMosse[index].pokemonScelto.HP;
            BitmapImage bitimg = new BitmapImage();
            bitimg.BeginInit();
            bitimg.UriSource = new Uri(@"Properties/" + listPokemonSelezionati[index].hires, UriKind.RelativeOrAbsolute);
            bitimg.EndInit();
            wpfLotta.imgPokemonN.Source = bitimg;

            wpfLotta.pBNostra.Maximum = listPokemonSelezionatiConMosse[index].pokemonScelto.HP;
            wpfLotta.pBNostra.Value = listPokemonSelezionatiConMosse[index].pokemonScelto.HP;
        }

        private readonly object turnoLock = new object();
        public void setTurno(bool b)
        {
            lock (turnoLock)
            {
                mioTurno = b;
            }
        }
        public bool togliPP(int nMossa)
        {
            bool ok = false;
            switch (nMossa)
            {
                case 1:
                    if(listPokemonSelezionatiConMosse[index].move1.PP - 1 >= 0)
                    {
                        ok = true;
                        listPokemonSelezionatiConMosse[index].move1.PP -= 1;
                    }
                    break;

                case 2:
                    if (listPokemonSelezionatiConMosse[index].move2.PP - 1 >= 0)
                    {
                        ok = true;
                        listPokemonSelezionatiConMosse[index].move2.PP -= 1;
                    }
                    break;

                case 3:
                    if (listPokemonSelezionatiConMosse[index].move3.PP - 1 >= 0)
                    {
                        ok = true;
                        listPokemonSelezionatiConMosse[index].move3.PP -= 1;
                    }
                    break;

                case 4:
                    if (listPokemonSelezionatiConMosse[index].move4.PP - 1 >= 0)
                    {
                        ok = true;
                        listPokemonSelezionatiConMosse[index].move4.PP -= 1;
                    }
                    break;
            }
            return ok;
        }
    }
    public class CPokemon
    {
        public int id { get; set; }
        public string name { get; set; }
        public string type1 { get; set; }
        public string type2 { get; set; }
        public int HP { get; set; }
        public string description { get; set; }
        public string sprite { get; set; }
        public string hires { get; set; }

        public override string ToString()
        {
            return name + " " + HP + "hp";
        }
    }
    public class CMoves
    {
        public string accuracy { get; set; }
        public string ename { get; set; }
        public int id { get; set; }
        public int power { get; set; }
        public string type { get; set; }
        public int PP { get; set; }

        public CMoves()
        {

        }

    }
    public class CPokemonConMosse
    {
        public CPokemon pokemonScelto { get; set; }
        public CMoves move1 { get; set; }
        public CMoves move2 { get; set; }
        public CMoves move3 { get; set; }
        public CMoves move4 { get; set; }

        public CPokemonConMosse(CPokemon pokemonScelto, CMoves move1, CMoves move2, CMoves move3, CMoves move4)
        {
            this.pokemonScelto = pokemonScelto;
            this.move1 = move1;
            this.move2 = move2;
            this.move3 = move3;
            this.move4 = move4;
        }
    }
    public class CPokemonPerLotta
    {
        public string nome { get; set; }
        public int id { get; set; }
        public int HP { get; set; }

        public CPokemonPerLotta(string nome, int id, int HP)
        {
            this.nome = nome;
            this.id = id;
            this.HP = HP;
        }

        public string toString()
        {
            return nome + "," + id + "," + HP;
        }
    }

}
