using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
//Librerias para miltiprocesamiento
using System.Threading;
using System.Diagnostics;

namespace PracticaMovimiento
{
    
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Stopwatch stopwatch;
        TimeSpan tiempoAnterior;

        enum EstadoJuego { Gameplay, Gameover };
        EstadoJuego estadoActual = EstadoJuego.Gameover;

        enum Direccion { Arriba, Abajo, Izquierda, Derecha, Ninguna };
        Direccion direccionJugador = Direccion.Ninguna;

        double velocidadCarro=300;
        double vidas = 4;
        int contadorAuxiliarVidas = 0;

        public MainWindow()
        {

            InitializeComponent();
            miCanvas.Focus();

            stopwatch = new Stopwatch();
            stopwatch.Start();
            tiempoAnterior = stopwatch.Elapsed;

            //1.- Establecer instrucciones
            ThreadStart threadStart = new ThreadStart(actualizar);
            //2.- Inicializar el thread
            Thread threadMoverEnemigos = new Thread(threadStart);
            //3.- Ejecutar el thread
            threadMoverEnemigos.Start();

        }


        private void btnPlayAgain_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(carro, 180);
            estadoActual = EstadoJuego.Gameplay;
            miCanvas.Visibility = Visibility.Visible;
            canvasGameOver.Visibility = Visibility.Collapsed;
            miCanvas.Focus();
            velocidadCarro = 300;
            vidas = 4;
            lblVidas.Text = "Vidas: " + (vidas);
            miCanvas.Focusable = true;
        }



        void actualizar()
        {
            while(true)
            { 
                Dispatcher.Invoke(
                    () =>
                    {
                        miCanvas.Focusable = true;
                        var tiempoActual = stopwatch.Elapsed;
                        var deltaTime = tiempoActual - tiempoAnterior;

                        if(velocidadCarro < 2950)
                        {
                            velocidadCarro += 30 * deltaTime.TotalSeconds;
                        }

                        if(estadoActual == EstadoJuego.Gameplay)
                        {
                            //moverJugador(deltaTime);
                            double topCarreteraActual = Canvas.GetTop(carretera);
                            double topCarreteraActual_copy = Canvas.GetTop(carretera2);
                            double topCarreteraActual_copy_2 = Canvas.GetTop(carretera3);
                            double topEnemigoActual = Canvas.GetTop(enemigo1);
                            double topEnemigoActual_2 = Canvas.GetTop(enemigo2);
                            double topEnemigoActual_3 = Canvas.GetTop(enemigo3);
                            Canvas.SetTop(carretera, topCarreteraActual + (velocidadCarro * deltaTime.TotalSeconds));
                            Random rnd = new Random();
                            int time = rnd.Next(1, 100);
                            int rnd_1 = rnd.Next(1, 100);
                            int rnd_2 = rnd.Next(1, 100);
                            int rnd_3 = rnd.Next(1, 100);
                            if (Canvas.GetTop(carretera) >= 847)
                            {
                                Canvas.SetTop(carretera, -442);
                            }

                            Canvas.SetTop(carretera2, topCarreteraActual_copy + (velocidadCarro * deltaTime.TotalSeconds));
                            if (Canvas.GetTop(carretera2) >= 847)
                            {
                                Canvas.SetTop(carretera2, -442);
                            }

                            Canvas.SetTop(carretera3, topCarreteraActual_copy_2 + (velocidadCarro * deltaTime.TotalSeconds));
                            if (Canvas.GetTop(carretera3) >= 847)
                            {
                                Canvas.SetTop(carretera3, -442);
                            }

                            Canvas.SetTop(enemigo1, topEnemigoActual + (velocidadCarro * deltaTime.TotalSeconds));
                            if (Canvas.GetTop(enemigo1) >= 847 && time == rnd_1)
                            {
                                Canvas.SetTop(enemigo1, -437);
                            }

                            Canvas.SetTop(enemigo2, topEnemigoActual_2 + (velocidadCarro * deltaTime.TotalSeconds));
                            if (Canvas.GetTop(enemigo2) >= 847 && time == rnd_2)
                            {
                                Canvas.SetTop(enemigo2, -437);
                            }

                            Canvas.SetTop(enemigo3, topEnemigoActual_3 + (velocidadCarro * deltaTime.TotalSeconds));
                            if (Canvas.GetTop(enemigo3) >= 847 && time == rnd_3)
                            {
                                Canvas.SetTop(enemigo3, -437);
                            } 

                            //hitbox
                            double xCarro = Canvas.GetTop(carro);
                            double yCarro = Canvas.GetLeft(carro);

                            double yEnemigo1 = Canvas.GetLeft(enemigo1);
                            double xEnemigo1 = Canvas.GetTop(enemigo1);

                            double yEnemigo2 = Canvas.GetLeft(enemigo2);
                            double xEnemigo2 = Canvas.GetTop(enemigo2);

                            double yEnemigo3 = Canvas.GetLeft(enemigo3);
                            double xEnemigo3 = Canvas.GetTop(enemigo3);

                            //colision
                            if (
                            xCarro + carro.Width >= xEnemigo1 && xCarro <= xEnemigo1 + enemigo1.Width && yCarro + carro.Height >= yEnemigo1 && yCarro <= yEnemigo1 + enemigo1.Height ||
                            xCarro + carro.Width >= xEnemigo2 && xCarro <= xEnemigo2 + enemigo2.Width && yCarro + carro.Height >= yEnemigo2 && yCarro <= yEnemigo2 + enemigo2.Height ||
                            xCarro + carro.Width >= xEnemigo3 && xCarro <= xEnemigo3 + enemigo3.Width && yCarro + carro.Height >= yEnemigo3 && yCarro <= yEnemigo3 + enemigo3.Height
                            )
                            {
                                contadorAuxiliarVidas = contadorAuxiliarVidas + 1;

                                if(contadorAuxiliarVidas == 1 && vidas>=0)
                                {
                                    vidas = vidas - 1;
                                }
                                velocidadCarro = 300;
                                if(vidas>=0)
                                {
                                    lblVidas.Text = "Vidas: " + (vidas);
                                }
                                if(vidas == 0)
                                {
                                    estadoActual = EstadoJuego.Gameover;
                                    miCanvas.Visibility = Visibility.Collapsed;
                                    canvasGameOver.Visibility = Visibility.Visible;
                                    miCanvas.Focusable = true;
                                }

                            }
                            else
                            {
                                contadorAuxiliarVidas = 0;
                            }

                            if (vidas != 0)
                            {
                                lblVelocidad.Text = "Velocidad: " + Convert.ToInt32(velocidadCarro)/10 + " Km/H";
                            }
                            

                        }
                        else if(estadoActual == EstadoJuego.Gameover)
                        {
                            velocidadCarro = 0;
                            Canvas.SetTop(enemigo1, -506);
                            Canvas.SetTop(enemigo2, -776);
                            Canvas.SetTop(enemigo3, -632);
                        }

                        tiempoAnterior = tiempoActual;

                    }
                    );
            }
        }


        private void miCanvas_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (estadoActual == EstadoJuego.Gameplay)
            {
                if (e.Key == Key.Left)
                {
                    double leftCarroActual = Canvas.GetLeft(carro);
                    if(leftCarroActual == 320)
                    {
                        Canvas.SetLeft(carro, leftCarroActual = 180);
                    }
                    else
                    {
                        Canvas.SetLeft(carro, leftCarroActual = 35);
                    }
                }

                if (e.Key == Key.Right)
                {
                    double leftCarroActual = Canvas.GetLeft(carro);
                    if(leftCarroActual == 35)
                    {
                        Canvas.SetLeft(carro, leftCarroActual = 180);
                    }
                    else
                    {
                        Canvas.SetLeft(carro, leftCarroActual = 320);
                    }
                }
            }
        }

        private void miCanvas_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Left && direccionJugador == Direccion.Izquierda)
            {
                direccionJugador = Direccion.Ninguna;
            }
            if (e.Key == Key.Right && direccionJugador == Direccion.Derecha)
            {
                direccionJugador = Direccion.Ninguna;
            }
        }

        


    }
}
