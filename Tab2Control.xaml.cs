using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;


namespace WpfApp
{
    public partial class Tab2Control : UserControl
    {
        private ProduktEingabe _selectedProdukt;
        internal ProduktEingabe SelectedProdukt
        {
            get => _selectedProdukt;
            set
            {
                _selectedProdukt = value;
                UpdateEditForm(); // Aktualisiert das Formular, wenn sich die Auswahl ändert
            }
        }

        public Tab2Control()
        {
            InitializeComponent();
            // Initialisiere _selectedProdukt mit einem Standardwert
            _selectedProdukt = new ProduktEingabe
            {
                Produkt = string.Empty,
                Beschreibung = string.Empty,
                Preis = 0,
                Anzahl = 0,
                Gesamtpreis = 0
            };
        }

        private void UpdateEditForm()
        {
            if (SelectedProdukt != null)
            {
                // Formular sichtbar machen und Felder vorfüllen
                editForm.Visibility = Visibility.Visible;
                editProdukt.Text = SelectedProdukt.Produkt;
                editBeschreibung.Text = SelectedProdukt.Beschreibung;
                editPreis.Text = SelectedProdukt.Preis.ToString();
                editAnzahl.Text = SelectedProdukt.Anzahl.ToString();
            }
            else
            {
                // Formular ausblenden
                editForm.Visibility = Visibility.Collapsed;
            }
        }



        private void Window_Loaded()
        {
            var dbHelper = new DatabaseHelper();
            dataGrid.ItemsSource = dbHelper.GetAllProdukte();
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            var produkt = SelectedProdukt;

            if (produkt != null)
            {
                // Aktualisiere die Daten des ausgewählten Produkts
                var Id = produkt.Id;
                var Produkt = editProdukt.Text;
                var Beschreibung = editBeschreibung.Text;
                var Preis = decimal.TryParse(editPreis.Text, out var preis) ? preis : 0;
                var Anzahl = decimal.TryParse(editAnzahl.Text, out var anzahl) ? anzahl : 0;
                var Gesamtpreis = Preis * Anzahl;

                // Erstelle das ProduktEingabe-Objekt
                var eingabe = new ProduktEingabe
                {
                    Id = Id,
                    Produkt = Produkt,
                    Beschreibung = Beschreibung,
                    Preis = Preis,
                    Anzahl = Anzahl,
                    Gesamtpreis = Gesamtpreis
                };

                var dbHelper = new DatabaseHelper();
                dbHelper.UpdateProdukt(eingabe);
                MessageBox.Show("Änderungen gespeichert.");

                // DataGrid aktualisieren
                dataGrid.ItemsSource = dbHelper.GetAllProdukte();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var produkt = SelectedProdukt;

            if (produkt != null)
            {

                var dbHelper = new DatabaseHelper();
                dbHelper.DeleteProdukt(produkt.Id);
                MessageBox.Show("Eintrag gelöscht.");

                // DataGrid aktualisieren
                dataGrid.ItemsSource = dbHelper.GetAllProdukte();
            }
            else
            {
                MessageBox.Show("Kein Eintrag ausgewählt.");
            }
        }

        private void ResetButton2_Click(object sender, RoutedEventArgs e)
        {

            // Alle Felder auf ihre Standardwerte zurücksetzen
            editProdukt.Text = SelectedProdukt.Produkt;
            editBeschreibung.Text = SelectedProdukt.Beschreibung;
            editPreis.Text = SelectedProdukt.Preis.ToString();
            editAnzahl.Text = SelectedProdukt.Anzahl.ToString();

            MessageBox.Show("Alle Eingaben wurden zurückgesetzt.", "Reset", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // Sicherstellen, dass eine Auswahl vorhanden ist
            if (dataGrid.SelectedItem is ProduktEingabe selectedProdukt)
            {

                Trace.WriteLine($"Ausgewählt: {selectedProdukt.Produkt}, {selectedProdukt.Beschreibung}");
                // MessageBox.Show($"Eintrag {selectedProdukt.Produkt}, {selectedProdukt.Beschreibung} selected.");

                // Formular sichtbar machen
                editForm.Visibility = Visibility.Visible;
                SelectedProdukt = selectedProdukt;

                // Felder vorfüllen
                editProdukt.Text = selectedProdukt.Produkt;
                editBeschreibung.Text = selectedProdukt.Beschreibung;
                editPreis.Text = selectedProdukt.Preis.ToString();
                editAnzahl.Text = selectedProdukt.Anzahl.ToString();
            }

        }

        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dbHelper = new DatabaseHelper();
            dataGrid.ItemsSource = dbHelper.GetAllProdukte();
        }


    }

}

