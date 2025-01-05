using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp
{

    public partial class Tab1Control : UserControl
    {
        public Tab1Control()
        {
            InitializeComponent();
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
            decimal quantity;
            if (!decimal.TryParse(quantityText, out quantity) || quantity <= 0)
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

            MessageBox.Show("Anzahl: " + eingabe.Anzahl.ToString());

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

        // Nur numerische Eingabe für das Preisfeld erlauben
        private void PriceTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Nur Ziffern und Dezimalpunkte erlauben
            e.Handled = !IsValidPriceInput(e.Text);
        }

        // Nur numerische Eingabe für das Anzahlfeld erlauben
        private void QuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Nur Ziffern und Dezimalpunkte erlauben
            e.Handled = !IsValidQuantityInput(e.Text);
        }

        // Überprüfung, ob die Eingabe eine gültige Zahl im Preisfeld ist (Ziffern oder Dezimalpunkt)
        private bool IsValidPriceInput(string input)
        {
            return char.IsDigit(input[0]) || input == "." || (input == "," && !priceTextBox.Text.Contains(","));
        }

        // Überprüfung, ob die Eingabe eine gültige Zahl im Anzahlfeld ist  (Ziffern oder Dezimalpunkt)
        private bool IsValidQuantityInput(string input)
        {
            return char.IsDigit(input[0]) || input == "." || (input == "," && !quantityTextBox.Text.Contains(","));
        }
    }
}


