using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;



namespace WpfApp
{
    public partial class MainWindow : Window
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

        public MainWindow()
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Den Text aus den Eingabefeldern holen
            string inputText = inputTextBox.Text;
            string descriptionText = descriptionTextBox.Text;
            string priceText = priceTextBox.Text;
            string quantityText = quantityTextBox.Text;

            // Preis validieren
            decimal price;
            if (!decimal.TryParse(priceText, out price))
            {
                MessageBox.Show("Bitte geben Sie einen gültigen Preis ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Anzahl validieren
            int quantity;
            if (!int.TryParse(quantityText, out quantity) || quantity <= 0)
            {
                MessageBox.Show("Bitte geben Sie eine gültige Anzahl ein (positive Zahl).", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Gesamtpreis berechnen
            decimal totalPrice = price * quantity;

            // Erstelle das ProduktEingabe-Objekt
            var eingabe = new ProduktEingabe
            {
                Produkt = inputText,
                Beschreibung = descriptionText,
                Preis = price,
                Anzahl = quantity,
                Gesamtpreis = totalPrice
            };

            // Speichere die Eingabe in der MySQL-Datenbank
            var dbHelper = new DatabaseHelper();
            dbHelper.SaveProduktEingabe(eingabe);

            // Ausgabe formatieren und anzeigen
            outputLabel.Text = $@"" +
                $"Nachricht: {inputText}" + Environment.NewLine +
                $"Beschreibung: {descriptionText}" + Environment.NewLine +
                $"Preis: {price:C}" + Environment.NewLine +
                $"Anzahl: {quantity}" + Environment.NewLine +
                $"Gesamtpreis: {totalPrice:C}";
        }

        // Nur numerische Eingabe für das Preisfeld erlauben
        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Nur Ziffern und Dezimalpunkte erlauben
            e.Handled = !IsValidPriceInput(e.Text);
        }

        // Nur numerische Eingabe für das Anzahlfeld erlauben
        private void QuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Nur Ziffern (positive Ganzzahlen) erlauben
            e.Handled = !IsValidQuantityInput(e.Text);
        }

        // Überprüfung, ob die Eingabe eine gültige Zahl im Preisfeld ist (Ziffern oder Dezimalpunkt)
        private bool IsValidPriceInput(string input)
        {
            return char.IsDigit(input[0]) || input == "." || (input == "," && !priceTextBox.Text.Contains(","));
        }

        // Überprüfung, ob die Eingabe eine gültige Zahl im Anzahlfeld ist (Ziffern)
        private bool IsValidQuantityInput(string input)
        {
            return char.IsDigit(input[0]);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
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
                var Anzahl = int.TryParse(editAnzahl.Text, out var anzahl) ? anzahl : 0;
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
            }else
            {
                MessageBox.Show("Kein Eintrag ausgewählt.");
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {

            // Alle Felder auf ihre Standardwerte zurücksetzen
            inputTextBox.Text = string.Empty;
            descriptionTextBox.Text = string.Empty;
            priceTextBox.Text = string.Empty;
            quantityTextBox.Text = string.Empty;
            outputLabel.Text = string.Empty;

            MessageBox.Show("Alle Eingaben wurden zurückgesetzt.", "Reset", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabItem2.IsSelected)
                Window_Loaded(sender, e);
        }

    }
}
