// COPYRIGHT 2013 Tuomas Laakkonen
// Bitnet powered CPU miner
// BITNET COPYRIGHT 2011 Konstantin Ineshin, Irkutsk, Russia.
// If you like this project please donate BTC 18TdCC4TwGN7PHyuRAm8XV88gcCmAHqGNs (Konstantin Ineshin)

using System;
using System.Collections;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Bitnet.Client;
using System.Threading.Tasks;

namespace Bitnet
{
  public class Program
  {
   public static int it = 0;
   public static int itl = 0;
    static void Main()
    {

      write("Welcome to tuomas56's CPU Bitcoin Miner");
      write("");
      write("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
      write("!Warning this miner does NOT support Stratum!");
      write("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
      write("");
      write("Enter URL (include http:// and port):");
      BitnetClient bc = new BitnetClient(Console.ReadLine());
      write("Enter Username:");
      string u = Console.ReadLine();
      write("Enter Password:");
      string p = Console.ReadLine();
      bc.Credentials = new NetworkCredential(u, p);
      var w = bc.GetWork();
      var t = w.Value<string>("target");
      Console.WriteLine("Target:" + t);
      Console.WriteLine("");
      string r = "";
       Start:
          Bruteforce b = new Bruteforce();
          b.min = 1;
          b.max = 100;
          b.charset = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
          string target = t;
          
          
          System.Timers.Timer tick = new System.Timers.Timer(10000);
          tick.Start();
          tick.Elapsed += tick_Elapsed;
          foreach (string res in b)
          {

              string result = sha256(sha256(res));
              if (result == target)
              {
                  r = result;
                  return;
              }

              it++;
          } 

      Console.WriteLine("Found Hash: " + r);
      string[] param = new string[0];
      param[0] = r;
      bc.InvokeMethod("getwork", param);
      tick.Stop();
          goto Start;

    } 

    static void tick_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        if (it > 90000000)
        {
            itl = 0;
            it = 0;
            write("Reset Hash Count! (over 900 million hashes)");
        }

        Console.WriteLine((it - itl) + " Hashes in last ten seconds.");
        Console.WriteLine(it + " Total Hashes");
        Console.WriteLine(((it - itl) / 10240) + " KH/s");
        Console.WriteLine("");
        itl = it;

    }

    
    static string sha256(string password)
    {
        SHA256Managed crypt = new SHA256Managed();
        string hash = String.Empty;
        byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
        foreach (byte bit in crypto)
        {
            hash += bit.ToString("x2");
        }
        return hash;
    }

    static void write(string t)
    {
        Console.WriteLine(t);
    }
  }
}