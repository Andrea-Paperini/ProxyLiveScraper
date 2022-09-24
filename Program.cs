using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ProxyLiveScraper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Benvenuto nel ProxyLiveScraper");
            Console.WriteLine("Scarico dalle fonti ed estraggo i proxy http e https");
            string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //SearchOption non esiste il file delle fonti lo creo
            if (!File.Exists(path + @"\Fonti.txt"))
            {
                File.WriteAllText(path + @"\Fonti.txt", "");
            }
            var date = DateTime.Now.ToString("MM-dd");

            //conto il numero totale delle fonti
            var totalefonti = File.ReadLines(path + @"\Fonti.txt").Count();
            //recupero tutte le fonti
            System.Collections.Generic.IEnumerable<String> arrayurl = File.ReadLines(path + @"\Fonti.txt");
            int vivi = 0;
            int dead = 0;
            int source = 1;
            string ApiBotTelegram = "TelegramBotApiToken";
            long chatid = ChatId;
            string percorsohttplive = path + @"\Http Live " + date + ".txt";
            string comando = @"/C curl -F ""document=@" + percorsohttplive + "\"" + " https://api.telegram.org/bot" + ApiBotTelegram + "/sendDocument?chat_id=-" + chatid;
            //Console.WriteLine(comando);
            //Se il file esiste prima lo svuoto (elimino)
            if (File.Exists(percorsohttplive))
            {
                File.Delete(percorsohttplive);
                System.Threading.Thread.Sleep(500);
                File.Create(percorsohttplive).Dispose();

            }
            foreach (string urlcorrente in arrayurl)
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = client.GetAsync(urlcorrente).Result)
                    {
                        using (HttpContent content = response.Content)
                        {
                            string data = content.ReadAsStringAsync().Result;
                            //Console.WriteLine(result);
                            string pattern = @"\d+\.\d+\.\d+\.\d+:\d{1,5}";
                            foreach (Match m in Regex.Matches(data, pattern))
                            {
                                string[] contenitore = m.Value.Split(":");
                                string proxy = contenitore[0];
                                var port = contenitore[1];
                                int portf = Convert.ToInt32(port);
                                bool isalive = SoketConnect(proxy, portf);
                                if (isalive == true)
                                {
                                    vivi++;
                                    Console.SetCursorPosition(0, 2);
                                    Console.Write("Fonti totali: " + totalefonti + " Fonte numero: " + source + " Vivi: " + vivi + " Dead: " + dead);
                                    Console.SetCursorPosition(0, 3);
                                    Console.WriteLine("Sto provando -> " + proxy + ":" + port);
                                    string createText = m.Value + Environment.NewLine;
                                    //Se il proxy è vivo lo appendo al file
                                    File.AppendAllText(percorsohttplive, createText);
                                }
                                else
                                {
                                    dead++;
                                    Console.SetCursorPosition(0, 2);
                                    Console.Write("Fonti totali: " + totalefonti + " numero corrente: " + source + " Vivi: " + vivi + " Dead: " + dead);
                                    Console.SetCursorPosition(0, 3);
                                    Console.WriteLine("Sto provando -> " + proxy + ":" + port);
                                }
                            }
                        }
                    }
                }
                source++;
            }

            //Console.WriteLine("azz "+comando);
            System.Diagnostics.Process.Start("CMD.exe", comando);
        }

        public static bool SoketConnect(string host, int port)
        {
            var is_success = false;
            try
            {
                var connsock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                connsock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 3000);
                //System.Threading.Thread.Sleep(500);
                var hip = IPAddress.Parse(host);
                var ipep = new IPEndPoint(hip, port);
                connsock.Connect(ipep);
                if (connsock.Connected)
                {
                    is_success = true;
                }
                connsock.Close();
            }
            catch (Exception)
            {
                is_success = false;
            }
            return is_success;
        }
    }
}
