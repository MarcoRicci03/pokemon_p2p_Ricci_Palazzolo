using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace pokemon_showdown_p2p
{
    public class DatiCondivisi
    {
        public UdpClient udpClient;
        public IPEndPoint RemoteIpEndPoint;
        //informazioni di questo peer
        public String ip_peer { get; set; }
        public int port_peer { get; set; }
        public String nome_peer { get; set; }
        public String codUtente { get; set; }
        //info peer connesso
        public String ip_peer_connesso { get; set; }
        public int port_peer_connesso { get; set; }
        public String nome_peer_connesso { get; set; }
        public String codUtente_connesso { get; set; }
        public bool connesso;
        public DatiCondivisi()
        {
            udpClient = new UdpClient(50002); //porta non registrata
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            connesso = false;

            Thread ThreadRiceviConnessione = new Thread(riceviConnessione);
        }

        public void ricevi()
        {
            Byte[] receive_data = new byte[1500];
            String to_split;
            //ricevere il pacchetto, ricomporlo e metterlo in una stringa
            if (connesso) //controllo per sicurezza che sia connesso con qualcuno
            {
                receive_data = udpClient.Receive(ref RemoteIpEndPoint);
                to_split = Encoding.ASCII.GetString(receive_data);
            }
            else
            {
                to_split = "Peer non connesso";
            }
            
        }

        public void riceviConnessione()
        {
            Byte[] receive_data = new byte[1500];
            Byte[] send_data;
            String to_split;
            String[] splitted;
            if (!connesso)
            {
                receive_data = udpClient.Receive(ref RemoteIpEndPoint);
                to_split = Encoding.ASCII.GetString(receive_data); //c;indirizzo;porta;nome;codiceUtente
                splitted = to_split.Split(';');
                ip_peer_connesso = splitted[1];
                port_peer_connesso = Int32.Parse(splitted[2]);
                nome_peer_connesso = splitted[3];
                codUtente_connesso = splitted[4];
                connesso = true;

                //invio conferma connessione
                send_data = Encoding.ASCII.GetBytes("c;" + ip_peer + ";" + port_peer.ToString() + ";" + nome_peer + ";" + codUtente); //c;indirizzo;porta;nome;codiceUtente
                Console.WriteLine("Ricevuto");
            }
            else
            {
                send_data = Encoding.ASCII.GetBytes("c;e");
                Console.WriteLine("Non connesso");
            }

            udpClient.Connect(ip_peer_connesso, port_peer_connesso);
            udpClient.Send(send_data, send_data.Length);

        }

        public void inviaConnessione()
        {
            Byte[] receive_data = new byte[1500];
            String to_split;
            String[] splitted;

            if (!connesso)
            {
                Byte[] send_data = Encoding.ASCII.GetBytes("c;" + ip_peer + ";" + port_peer.ToString() + ";" + nome_peer + ";" + codUtente); //c;indirizzo;porta;nome;codiceUtente
                udpClient.Connect(ip_peer_connesso, port_peer_connesso);
                udpClient.Send(send_data, send_data.Length);

                //ascoltare la risposta
                receive_data = udpClient.Receive(ref RemoteIpEndPoint);
                to_split = Encoding.ASCII.GetString(receive_data); //c;indirizzo;porta;nome;codiceUtente
                splitted = to_split.Split(';');
                if (to_split[1].Equals("e"))
                    Console.WriteLine("Errore");
                    
            }
        }

        public void manda(int id)
        {
            Byte[] send_data = Encoding.ASCII.GetBytes("m;" + port_peer.ToString()+ ";"+ id); //m;porta;id
            udpClient.Connect(ip_peer_connesso, port_peer_connesso);
            udpClient.Send(send_data, send_data.Length);
        }
    }
}
