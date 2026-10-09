using System.Windows;
using OrdenesInfraestructura;
using OrdenesApplication;
using OrdenesDomain;

namespace GestiondeOrdenesdeVenta
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Crear repositorios
            var clienteRepo = new ClienteRepository();
            var empleadoRepo = new EmpleadoRepository();
            var ordenRepo = new OrdenRepository();

            // Crear servicios de aplicación
            var clienteService = new ClienteService(clienteRepo);
            var empleadoService = new EmpleadoService(empleadoRepo);
            var ordenService = new OrdenService(ordenRepo);

            // Resolver MainWindow pasando las dependencias
            var main = new MainWindow(clienteService, empleadoService, ordenService);
            main.Show();

            base.OnStartup(e);
        }
    }

}
