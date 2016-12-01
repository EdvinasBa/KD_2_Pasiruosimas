using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KD_2_Pasiruosimas
{
    class Program
    {
        //Balses
        const string CBals = "aąeęėyiįoūų";
        const string CSkyr = ",;! ";
        static void Main(string[] args)
        {
            string[] tekstas = SkaitytiFail("Tekstas.txt");

            string[] apdorotasTekstas = new string[tekstas.Length];

            ApdorotiTeksta(tekstas, out apdorotasTekstas);
            RasytiFaila("RedTekstas.txt", apdorotasTekstas);

            int nr = 0;
            RastiEilute("RedTekstas.txt", out nr);
            Console.WriteLine("Ilgiausia eilute apdorotame faile: {0}", nr+1);
            //Console.WriteLine(apdorotasTekstas[nr]);
            //Console.WriteLine(tekstas[nr]);
            if (apdorotasTekstas[nr] == tekstas[nr])
                Console.WriteLine("Eilute neredaguota.");
            else
                Console.WriteLine("Eilute redaguota");

            //foreach (string zodis in apdorotasTekstas)
            //  Console.WriteLine(zodis);
        }

        // Grąžina string masyvą su teksto eilutėmis iš failo
        static string[] SkaitytiFail(string fPav)
        {
            string[] tekstas = File.ReadAllLines(@fPav, Encoding.GetEncoding(1257));
            return tekstas;
        }
        static void RasytiFaila(string fPav, string[] tekstas)
        {
            File.WriteAllLines(fPav,tekstas);
        }
        // Grąžina eilutės (žodžio) e skirtingų balsių skaičių
        static int EilutesSkirtBalsiuskaicius(string e)
        {
            int sk = 0;
            string balsiai = CBals;
            foreach(char raide in e)
                foreach(char balsis in balsiai)
                    if (raide == balsis)
                    {
                        sk++;
                        balsiai.Replace(balsiai, "");
                    }
            return sk;
        }

        // Eilutėje e randa ilgiausią žodį zod iš tų, kuriuose yra 3 skirtingos balsės. Neradus grąžina tuščią eilutę.
        // pr - žodžio pradžia, s – skyrikliai.
        // e = eilute
        // s = skyrikliai
        // zod = zodis kuri randa
        // zodis eiluteje string
        static void RastiZodiEil(string e, string s, out string zod, out int pr)
        {
            string[] eilute = e.Split(s.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            string[] tinkamiZod = new string[eilute.Length];

            int tinkamuSk = 0;

            foreach (string zodis in eilute)
            {
                int balsiuZodyje = 0;

                foreach (char raide in zodis)
                    foreach (char balse in CBals)
                        if (raide == balse)
                            balsiuZodyje++;
                //Console.WriteLine(balsiuZodyje);
                if (balsiuZodyje == 3)
                    tinkamiZod[tinkamuSk++] = zodis;
                else
                    tinkamiZod[tinkamuSk] = "-";
            }
            if (tinkamuSk == 0)
            {
                pr = 0;
                zod = null;
                return;
            }
            else
                Array.Sort(tinkamiZod, (x, y) => y.Length.CompareTo(x.Length));
            //foreach (string zodis in tinkamiZod)
                //Console.WriteLine(zodis);
            zod = tinkamiZod[0];

            pr = e.IndexOf(zod);
        }

        // Eilutėje e ilgiausią žodį zod iš tų, kuriuose yra 3 skirtingos balsės, perkelia į eilutės pradžią
        // kartu su skyrikliais už jo , s – skyrikliai, pr – žodžio pradžia.
        static void PerkeltiZodiEil(ref string e, string s, string zod, int pr)
        {
            RastiZodiEil(e, s, out zod, out pr);
           // Console.WriteLine(zod);
            if (zod != null)
            {
                for (int i = pr + zod.Length - 1; i < e.Length; i++)
                    foreach (char skyr in s)
                        if (skyr == e[i])
                            zod += skyr;
                        else
                            break;
            //Console.WriteLine(e);
            e = e.Remove(pr, zod.Length);
            //Console.WriteLine("Remove {0}", zod);
            e = zod + e;
            }
        }

        // Faile fv randa ilgiausios eilutės numerį nr.
        static void RastiEilute(string fv, out int nr)
        {
            nr = 0;
            string[] failas = File.ReadAllLines(fv);
            int max = 0;
            for (int i = 0; i < failas.Length; i++)
                if (failas[i].Length > max)
                {
                    nr = i;
                    max = failas[i].Length;
                }
        }

        // Kiekvienoje eilutėje ilgiausią žodį iš tų, kuriuose yra trys skirtingos balsės, perkelti į eilutės pradžią kartu su
        // skyrikliais už jo.
        
        static void ApdorotiTeksta(string[] tekstas, out string[] apdorotasTekstas)
        {
            apdorotasTekstas = new string[tekstas.Length];
            for(int i = 0; i < tekstas.Length; i++)
            {
                apdorotasTekstas[i] = tekstas[i];
                string zodis = "";
                int pradzia = 0;
                PerkeltiZodiEil(ref apdorotasTekstas[i], CSkyr, zodis, pradzia);
            }

        }
    }
}
