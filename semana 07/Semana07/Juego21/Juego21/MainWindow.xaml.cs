using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Juego21
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Juego: dos jugadores, mazo, puntuaciones
        private List<Card> deck;
        private Random rnd = new Random();
        private int puntos1 = 0;
        private int puntos2 = 0;
        private int aces1 = 0;
        private int aces2 = 0;
        private bool turnoJugador1 = true;

        public MainWindow()
        {
            InitializeComponent();
            NuevoJuego();
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            NuevoJuego();
        }

        private void NuevoJuego()
        {
            // crear mazo
            deck = CreateDeck();
            puntos1 = puntos2 = 0;
            aces1 = aces2 = 0;
            turnoJugador1 = true;
            PanelCartas1.Children.Clear();
            PanelCartas2.Children.Clear();
            LblJugador1.Text = "Jugador 1 - Puntos: 0";
            LblJugador2.Text = "Jugador 2 - Puntos: 0";
            TxtResultado.Text = "Nuevo juego iniciado. Turno: Jugador 1";
            BtnPedir1.IsEnabled = true;
            BtnPlantarse1.IsEnabled = true;
            BtnPedir2.IsEnabled = false;
            BtnPlantarse2.IsEnabled = false;
        }

        private List<Card> CreateDeck()
        {
            var suits = new[] { "♠", "♥", "♦", "♣" };
            var ranks = new[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
            var list = new List<Card>();
            foreach (var s in suits)
            {
                foreach (var r in ranks)
                {
                    int value;
                    if (r == "A") value = 11;
                    else if (r == "J" || r == "Q" || r == "K") value = 10;
                    else value = int.Parse(r);
                    list.Add(new Card { Rank = r, Suit = s, Value = value });
                }
            }
            // mezclar
            return list.OrderBy(x => rnd.Next()).ToList();
        }

        private Card DrawCard()
        {
            if (deck == null || deck.Count == 0) deck = CreateDeck();
            var c = deck[0];
            deck.RemoveAt(0);
            return c;
        }

        private void MostrarCartaEnPanel(Card c, WrapPanel panel)
        {
            // intentar cargar imagen desde carpeta Images/Cards usando convención Rank+Suit (ej: AS.png, 10H.png, JC.png)
            string suitChar = SuitChar(c.Suit);
            string fileName = $"{c.Rank}{suitChar}.png";
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = System.IO.Path.Combine(baseDir, "Images", "Cards", fileName);

            if (File.Exists(path))
            {
                try
                {
                    var img = new Image
                    {
                        Width = 60,
                        Height = 90,
                        Margin = new Thickness(4),
                        Source = new BitmapImage(new Uri(path, UriKind.Absolute))
                    };
                    panel.Children.Add(img);
                    return;
                }
                catch
                {
                    // fallback al dibujo si hay error cargando la imagen
                }
            }

            // fallback: dibujar representación textual
            var border = new Border
            {
                Width = 60,
                Height = 90,
                Background = Brushes.White,
                CornerRadius = new CornerRadius(4),
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(4)
            };
            var txt = new TextBlock
            {
                Text = c.Rank + c.Suit,
                FontSize = 18,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontWeight = FontWeights.Bold
            };
            border.Child = txt;
            panel.Children.Add(border);
        }

        private string SuitChar(string suit)
        {
            // mapear símbolo de palo a letra usada en nombres de archivo
            return suit switch
            {
                "♠" => "S",
                "♥" => "H",
                "♦" => "D",
                "♣" => "C",
                "S" => "S",
                "H" => "H",
                "D" => "D",
                "C" => "C",
                _ => suit
            };
        }

        private void ActualizarLabels()
        {
            LblJugador1.Text = $"Jugador 1 - Puntos: {puntos1}";
            LblJugador2.Text = $"Jugador 2 - Puntos: {puntos2}";
        }

        private int AjustarPorAces(int puntos, int aces)
        {
            // si hay ases contados como 11 y se pasa de 21, convertir algunos a 1 (-10 cada uno)
            while (puntos > 21 && aces > 0)
            {
                puntos -= 10;
                aces--;
            }
            return puntos;
        }

        private void FinalizarTurnoJugador1()
        {
            turnoJugador1 = false;
            BtnPedir1.IsEnabled = false;
            BtnPlantarse1.IsEnabled = false;
            BtnPedir2.IsEnabled = true;
            BtnPlantarse2.IsEnabled = true;
            TxtResultado.Text = "Turno: Jugador 2";
        }

        private void FinalizarJuego()
        {
            BtnPedir1.IsEnabled = false;
            BtnPlantarse1.IsEnabled = false;
            BtnPedir2.IsEnabled = false;
            BtnPlantarse2.IsEnabled = false;
            // decidir ganador
            string result;
            bool bust1 = puntos1 > 21;
            bool bust2 = puntos2 > 21;
            if (bust1 && bust2) result = "Ambos se pasaron. Empate (nadie gana).";
            else if (bust1) result = $"GANADOR: Jugador 2. Jugador1 se pasó ({puntos1}).";
            else if (bust2) result = $"GANADOR: Jugador 1. Jugador2 se pasó ({puntos2}).";
            else if (puntos1 == puntos2) result = $"Empate: ambos {puntos1} puntos.";
            else if (puntos1 > puntos2) result = $"GANADOR: Jugador 1 con {puntos1} puntos frente a {puntos2}.";
            else result = $"GANADOR: Jugador 2 con {puntos2} puntos frente a {puntos1}.";
            TxtResultado.Text = result;
        }

        private void BtnPedir1_Click(object sender, RoutedEventArgs e)
        {
            if (!turnoJugador1) return;
            var c = DrawCard();
            MostrarCartaEnPanel(c, PanelCartas1);
            puntos1 += c.Value;
            if (c.Rank == "A") aces1++;
            puntos1 = AjustarPorAces(puntos1, aces1);
            ActualizarLabels();
            if (puntos1 > 21)
            {
                TxtResultado.Text = $"Jugador 1 se pasó con {puntos1} puntos.";
                FinalizarJuego();
            }
            else if (puntos1 == 21)
            {
                TxtResultado.Text = "Jugador 1 alcanzó 21!";
                FinalizarTurnoJugador1();
            }
        }

        private void BtnPlantarse1_Click(object sender, RoutedEventArgs e)
        {
            if (!turnoJugador1) return;
            FinalizarTurnoJugador1();
        }

        private void BtnPedir2_Click(object sender, RoutedEventArgs e)
        {
            if (turnoJugador1) return;
            var c = DrawCard();
            MostrarCartaEnPanel(c, PanelCartas2);
            puntos2 += c.Value;
            if (c.Rank == "A") aces2++;
            puntos2 = AjustarPorAces(puntos2, aces2);
            ActualizarLabels();
            if (puntos2 > 21)
            {
                TxtResultado.Text = $"Jugador 2 se pasó con {puntos2} puntos.";
                FinalizarJuego();
            }
            else if (puntos2 == 21)
            {
                TxtResultado.Text = "Jugador 2 alcanzó 21!";
                FinalizarJuego();
            }
        }

        private void BtnPlantarse2_Click(object sender, RoutedEventArgs e)
        {
            if (turnoJugador1) return;
            // Al plantarse, comparar resultados y finalizar
            FinalizarJuego();
        }
    }

    public class Card
    {
        public string Rank { get; set; }
        public string Suit { get; set; }
        public int Value { get; set; }
    }
}
