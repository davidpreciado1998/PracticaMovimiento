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

        public MainWindow()
        {

            InitializeComponent();
            miCanvas.Focus();

            stopwatch = new Stopwatch();
            stopwatch.Start();
            tiempoAnterior = stopwatch.Elapsed;

            //1.- Establecer instrucciones
            ThreadStart threadStart = new ThreadStart(moverEnemigos);
            //2.- Inicializar el thread
            Thread threadMoverEnemigos = new Thread(threadStart);
            //3.- Ejecutar el thread
            threadMoverEnemigos.Start();
        }

        void moverEnemigos()
        {
            while(true)
            { 
                Dispatcher.Invoke(
                    () =>
                    {
                        var tiempoActual = stopwatch.Elapsed;
                        var deltaTime = tiempoActual - tiempoAnterior;

                        double leftCarroActual = Canvas.GetLeft(carro);
                        Canvas.SetLeft(carro, leftCarroActual - (300 * deltaTime.TotalSeconds)) ;
                        if(Canvas.GetLeft(carro) <= -100)
                        {
                            Canvas.SetLeft(carro, 800);
                        }
                        tiempoAnterior = tiempoActual;
                    }
                    );
            }
        }

        private void miCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Up)
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
