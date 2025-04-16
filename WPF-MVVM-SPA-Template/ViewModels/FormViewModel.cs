using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using WPF_MVVM_SPA_Template.Models;
using WPF_MVVM_SPA_Template.Views;

namespace WPF_MVVM_SPA_Template.ViewModels
{
    class FormViewModel : INotifyPropertyChanged
    {
        private readonly MainViewModel _mainViewModel;
        private readonly ClientsViewModel _clientsViewModel;

        private Client _client;
        private Client _originalClient;

        public Client Client
        {
            get => _client;
            set
            {
                _client = value;
                _originalClient = CloneClient(_client); // Guardar una copia del cliente original
                OnPropertyChanged();
            }
        }

        private Client CloneClient(Client client)
        {
            return new Client
            {
                Id = client.Id,
                DNI = client.DNI,
                Nom = client.Nom,
                Cognoms = client.Cognoms,
                Email = client.Email,
                Telefon = client.Telefon,
                DataAlta = client.DataAlta
            };
        }

        // Comandos para los botones de la vista
        public RelayCommand GuardarCommand { get; set; }
        public RelayCommand CancelarCommand { get; set; }

        public FormViewModel(MainViewModel mainViewModel, ClientsViewModel clientsViewModel)
        {
            _mainViewModel = mainViewModel;
            _clientsViewModel = clientsViewModel;

            // Inicializamos los comandos
            GuardarCommand = new RelayCommand(x => Guardar());
            CancelarCommand = new RelayCommand(x => Cancelar());
        }

        private void Guardar()
        {
            if (Client != null)
            {
                // Verifica si el cliente ya existe
                var clienteExistente = _clientsViewModel.Clients.FirstOrDefault(c => c.Id == Client.Id);

                if (clienteExistente != null)
                {
                    // Actualiza las propiedades del cliente existente
                    clienteExistente.DNI = Client.DNI;
                    clienteExistente.Nom = Client.Nom;
                    clienteExistente.Cognoms = Client.Cognoms;
                    clienteExistente.Email = Client.Email;
                    clienteExistente.Telefon = Client.Telefon;
                    clienteExistente.DataAlta = Client.DataAlta;
                }
                else
                {
                    // Si es un nuevo cliente, lo agregas a la lista
                    _clientsViewModel.Clients.Add(Client);
                }

                // Regresar a la vista de clientes
                _mainViewModel.CurrentView = new ClientsView { DataContext = _clientsViewModel };
            }
        }

        private void Cancelar()
        {
            // Restaurar los valores originales del cliente
            if (_originalClient != null)
            {
                Client.DNI = _originalClient.DNI;
                Client.Nom = _originalClient.Nom;
                Client.Cognoms = _originalClient.Cognoms;
                Client.Email = _originalClient.Email;
                Client.Telefon = _originalClient.Telefon;
                Client.DataAlta = _originalClient.DataAlta;
            }

            // Cambia la vista actual a la vista de clientes sin guardar cambios
            _mainViewModel.CurrentView = new ClientsView { DataContext = _mainViewModel.ClientsVM };
        }

        // Implementación de INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
