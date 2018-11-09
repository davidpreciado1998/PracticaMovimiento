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
        EstadoJuego estadoActual = EstadoJuego.Gameplay;

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

        void actualizar()
        {
            while(true)
            { 
                Dispatcher.Invoke(
                    () =>
                    {
                        var tiempoActual = stopwatch.Elapsed;
                        var deltaTime = tiempoActual - tiempoAnterior;

                        if(estadoActual == EstadoJuego.Gameplay)
                        {

                            double leftCarroActual = Canvas.GetLeft(carro);
                            Canvas.SetLeft(carro, leftCarroActual - (20 * deltaTime.TotalSeconds));
                            if (Canvas.GetLeft(carro) <= -100)
                            {
                                Canvas.SetLeft(carro, 800);
                            }


                            //Interseccioon en X
                            double xCarro = Canvas.GetLeft(carro);
                            double xVaquero = Canvas.GetLeft(imgVaquero);
                            if (xVaquero + imgVaquero.Width >= xCarro && xVaquero <= xCarro + carro.Width)
                            {
                                lblInterseccionX.Text = "Si HAY INTESECCION EN X!!!!";
                            }
                            else
                            {
                                lblInterseccionX.Text = "No hay interseccion en X";
                            }

                            //Interseccioon en Y
                            double yCarro = Canvas.GetTop(carro);
                            double yVaquero = Canvas.GetTop(imgVaquero);
                            if (yVaquero + imgVaquero.Height >= yCarro && yVaquero <= yCarro + carro.Height)
                            {
                                lblInterseccionY.Text = "Si HAY INTESECCION EN Y!!!!";
                            }
                            else
                            {
                                lblInterseccionY.Text = "No hay interseccion en Y";
                            }

                            //colision
                            if (xVaquero + imgVaquero.Width >= xCarro && xVaquero <= xCarro + carro.Width && yVaquero + imgVaquero.Height >= yCarro && yVaquero <= yCarro + carro.Height)
                            {
                                lblColision.Text = "HAY COLISION";
                                estadoActual = EstadoJuego.Gameover;
                                miCanvas.Visibility = Visibility.Collapsed;
                                canvasGameOver.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                lblColision.Text = "No Hay colision";
                            }

                        }
                        else if(estadoActual == EstadoJuego.Gameover)
                        {

                        }

                        tiempoAnterior = tiempoActual;

                    }
                    );
            }
        }

        private void miCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (estadoActual == EstadoJuego.Gameplay)
            {
                if (e.Key == Key.Up)
                {
                    double topVaqueroActual = Canvas.GetTop(imgVaquero);
                    Canvas.SetTop(imgVaquero, topVaqueroActual - 15);
                }

                if (e.Key == Key.Down)
                {
                    double downVaqueroActual = Canvas.GetTop(imgVaquero);
                    Canvas.SetTop(imgVaquero, downVaqueroActual + 15);
                }

                if (e.Key == Key.Left)
                {
                    double leftVaqueroActual = Canvas.GetLeft(imgVaquero);
                    Canvas.SetLeft(imgVaquero, leftVaqueroActual - 15);
                }

                if (e.Key == Key.Right)
                {
                    double rightVaqueroActual = Canvas.GetLeft(imgVaquero);
                    Canvas.SetLeft(imgVaquero, rightVaqueroActual + 15);
                }
            }
        }
    }
}
