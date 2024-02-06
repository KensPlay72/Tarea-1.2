using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.ComponentModel;
using Tarea_1._2.Controllers;
using Tarea_1._2.Models;

namespace Tarea_1._2.ViewModels
{
    public class ListadoPersonasViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Personas> _personasList;
        public PersonasController PersonasController => _personasController;


        public ObservableCollection<Personas> PersonasList
        {
            get { return _personasList; }
            set
            {
                _personasList = value;
                OnPropertyChanged(nameof(PersonasList));
            }
        }

        private PersonasController _personasController;

        public ListadoPersonasViewModel()
        {
            _personasList = new ObservableCollection<Personas>();
            _personasController = new PersonasController();
        }

        public async Task CargarPersonas()
        {
            await _personasController.Init();
            var personas = await _personasController.ObtenerTodasLasPersonas();
            PersonasList = new ObservableCollection<Personas>(personas);

            // Asegúrate de que OnPropertyChanged se llame después de actualizar la lista
            OnPropertyChanged(nameof(PersonasList));
        }


        public async Task<int> EliminarPersona(Personas persona)
        {
            int deletedRows = await _personasController.DeletePerson(persona);
            if (deletedRows > 0)
            {
                await ActualizarListaPersonas();
            }
            return deletedRows;
        }

        public async Task ActualizarListaPersonas()
        {
            await CargarPersonas();
            OnPropertyChanged(nameof(PersonasList));
        }



        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
