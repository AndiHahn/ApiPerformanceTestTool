using System;
using System.Windows;
using ApiTester.GUI.ViewModels;
using ApiTester.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace ApiTester.GUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            //Windows and ViewModels
            services.AddTransient(typeof(MainWindow));
            services.AddTransient(typeof(MainViewModel));

            services.AddHttpClient();

            services.RegisterLogic();
        }
    }
}
