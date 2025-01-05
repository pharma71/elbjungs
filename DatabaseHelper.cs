using System.Windows;
using MySql.Data.MySqlClient;

namespace WpfApp
{
    internal class DatabaseHelper
    {
     
        private string connectionString = "Server=127.0.0.1; Database=elbjungs; Uid=root; Pwd=;";

        public void SaveProduktEingabe(ProduktEingabe eingabe)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO Produkte (Produkt, Beschreibung, Preis, Anzahl, Gesamtpreis) VALUES (@Produkt, @Beschreibung, @Preis, @Anzahl, @Gesamtpreis)";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Produkt", eingabe.Produkt);
                        cmd.Parameters.AddWithValue("@Beschreibung", eingabe.Beschreibung);
                        cmd.Parameters.AddWithValue("@Preis", eingabe.Preis);
                        cmd.Parameters.AddWithValue("@Anzahl", eingabe.Anzahl);
                        cmd.Parameters.AddWithValue("@Gesamtpreis", eingabe.Gesamtpreis);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Fehlerbehandlung
                    Console.WriteLine($"Fehler: {ex.Message}");
                }
            }
        }
   

    public List<ProduktEingabe> GetAllProdukte()
        {
            var produkte = new List<ProduktEingabe>();

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Id, Produkt, Beschreibung, Preis, Anzahl, Gesamtpreis FROM Produkte";

                    using (var cmd = new MySqlCommand(query, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            produkte.Add(new ProduktEingabe
                            {
                                Id = reader.GetInt32("Id"),
                                Produkt = reader.GetString("Produkt"),
                                Beschreibung = reader.GetString("Beschreibung"),
                                Preis = reader.GetDecimal("Preis"),
                                Anzahl = reader.GetDecimal("Anzahl"),
                                Gesamtpreis = reader.GetDecimal("Gesamtpreis")
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler: {ex.Message}");
                }
            }

            return produkte;
        }


        public void DeleteProdukt(int id)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Produkte WHERE Id = @Id";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler: {ex.Message}");
                }
            }
        }

        public void UpdateProdukt(ProduktEingabe produkt)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"UPDATE Produkte SET Produkt = @Produkt, Beschreibung = @Beschreibung, 
                             Preis = @Preis, Anzahl = @Anzahl, Gesamtpreis = @Gesamtpreis WHERE Id = @Id";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Id", produkt.Id);
                        cmd.Parameters.AddWithValue("@Produkt", produkt.Produkt);
                        cmd.Parameters.AddWithValue("@Beschreibung", produkt.Beschreibung);
                        cmd.Parameters.AddWithValue("@Preis", produkt.Preis);
                        cmd.Parameters.AddWithValue("@Anzahl", produkt.Anzahl);
                        cmd.Parameters.AddWithValue("@Gesamtpreis", produkt.Gesamtpreis);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler: {ex.Message}");
                }
            }
        }


    }

}