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
        public List<CMoves> listMosse { get; set; } //lista delle mosse
        public List<CPokemon> listPokemon { get; set; } //lista di tutti i pokemon basica
        public List<CPokemon> listPokemonSelezionati { get; set; } //lista pokemon selezionati basica
        public List<CPokemonConMosse> listPokemonSelezionatiConMosse { get; set; } //lista dei pokemon selezionati con mosse assegnate
        public List<CPokemonPerLotta> listPokemonPerLotta { get; set; } //lista dei pokemon selezionati con vita, id e nome


        public DatiCondivisiGioco()
        {
            loadDataFromJSON();
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
            listPokemonPerLotta = new List<CPokemonPerLotta>();
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
            CPokemonPerLotta ppl;
            for (int i = 0; i < listPokemonSelezionati.Count; i++)
            {
                ppl = new CPokemonPerLotta(listPokemonSelezionati[i].name, listPokemonSelezionati[i].id, listPokemonSelezionati[i].HP);
                listPokemonPerLotta.Add(ppl);
            }
        }

        public void aggiornaLotta(Lotta wpfLotta)
        {
            //aggiorno pokemon nostri
            wpfLotta.btnMossa1.Content = listPokemonSelezionatiConMosse[0].move1.ename;
            wpfLotta.btnMossa2.Content = listPokemonSelezionatiConMosse[0].move2.ename;
            wpfLotta.btnMossa3.Content = listPokemonSelezionatiConMosse[0].move3.ename;
            wpfLotta.btnMossa4.Content = listPokemonSelezionatiConMosse[0].move4.ename;
            BitmapImage bitimg = new BitmapImage();
            bitimg.BeginInit();
            bitimg.UriSource = new Uri(@"Properties/" + listPokemonSelezionati[0].hires, UriKind.RelativeOrAbsolute);
            bitimg.EndInit();
            wpfLotta.imgPokemonN.Source = bitimg;

            //aggiorno pokemon avversari

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
    }

}
