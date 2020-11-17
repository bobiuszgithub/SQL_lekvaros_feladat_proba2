using System;
using System.Data.SQLite;

namespace SQLConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var conn = new SQLiteConnection("Data Source=mydb.db"))
            {
                conn.Open();
                var createComm = conn.CreateCommand();

                createComm.CommandText = @"
CREATE TABLE IF NOT EXISTS lekvarok( 
    uveg_meret INT NOT NULL,
    lekvar_tipus VARCHAR(1000) NOT NULL
);
";
                //Console.WriteLine("add meg az id-t");
                //var id = Console.ReadLine();
                Console.WriteLine("add meg az üveg méretét: ");
                var meret = Console.ReadLine();
                Console.WriteLine("add meg a lekvár típusát: ");
                var tipus = Console.ReadLine();


                var insertComm = conn.CreateCommand();
                insertComm.CommandText = @"
INSERT INTO lekvarok(uveg_meret, lekvar_tipus)
    VALUES(@umeret, @ltipus)
";
                //insertComm.Parameters.AddWithValue("@lid", id);
                insertComm.Parameters.AddWithValue("@umeret", meret);
                insertComm.Parameters.AddWithValue("@ltipus", tipus);
                createComm.ExecuteNonQuery();


                int erintettSorok = insertComm.ExecuteNonQuery();

                var selectComm = conn.CreateCommand();
                selectComm.CommandText = @"
SELECT uveg_meret, lekvar_tipus
FROM lekvarok
";
                using (var reader = selectComm.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int emeret = reader.GetInt32(0);
                        string etipus = reader.GetString(1);
                        Console.WriteLine("{0} L - {1} lekvár", emeret, etipus);
                    }
                }
                Console.WriteLine(erintettSorok);


                //kiírja az összes lekvár mennyiségét literben
                var sumcomm = conn.CreateCommand();
                sumcomm.CommandText = @"
SELECT sum(meret) FROM `lekvarok
";

                int osszeslekvar = sumcomm.ExecuteNonQuery();
                Console.WriteLine(osszeslekvar);

                //kiírja fajtánként a mennyiséget
                var fajta_mennyiseg = conn.CreateCommand();
                fajta_mennyiseg.CommandText = @"
SELECT tipus, count(tipus) FROM lekvarok
group by tipus
";


                int fajtamennyiseg = fajta_mennyiseg.ExecuteNonQuery();
                Console.WriteLine(fajtamennyiseg);

                //kiírja az átlagos üvegméretet
                var atlag = conn.CreateCommand();
                atlag.CommandText = @"
SELECT AVG(meret) FROM lekvarok 
";
                int atlagmeret = atlag.ExecuteNonQuery();
                Console.WriteLine(atlagmeret);

                Console.ReadLine();


            }
        }
    }
}

