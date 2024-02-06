using System;
namespace Tarea_1._2
{
    public partial class ListadoPersonasPage : ContentPage
    {
        ViewModels.ListadoPersonasViewModel _viewModel;

        public ListadoPersonasPage()
        {
            InitializeComponent();
            _viewModel = new ViewModels.ListadoPersonasViewModel();
            BindingContext = _viewModel;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.CargarPersonas();
        }

        private async void RegistrarNuevaPersona_Clicked(object sender, EventArgs e)
        {
            // Navegar a la p�gina de registro (MainPage)
            await Navigation.PushAsync(new MainPage());
        }

        private async void Editar_Clicked(object sender, EventArgs e)
        {
            var persona = (Models.Personas)((Button)sender).CommandParameter;

            // Mostrar ventanas emergentes para cada campo y actualizar la informaci�n de la persona
            var nuevoNombre = await DisplayPromptAsync("Editar Persona", "Nuevo nombre", "Guardar", "Cancelar", placeholder: "Nombre", keyboard: Keyboard.Text, initialValue: persona.Nombre);

            if (nuevoNombre != null)
                persona.Nombre = nuevoNombre;

            var nuevoApellido = await DisplayPromptAsync("Editar Persona", "Nuevo apellido", "Guardar", "Cancelar", placeholder: "Apellido", keyboard: Keyboard.Text, initialValue: persona.Apellido);

            if (nuevoApellido != null)
                persona.Apellido = nuevoApellido;

            var nuevaEdad = await DisplayPromptAsync("Editar Persona", "Nueva edad", "Guardar", "Cancelar", placeholder: "Edad", keyboard: Keyboard.Numeric, initialValue: persona.Edad.ToString());

            if (nuevaEdad != null && int.TryParse(nuevaEdad, out int edad))
                persona.Edad = edad;

            var nuevoCorreo = await DisplayPromptAsync("Editar Persona", "Nuevo correo", "Guardar", "Cancelar", placeholder: "Correo", keyboard: Keyboard.Email, initialValue: persona.Correo);

            if (nuevoCorreo != null)
                persona.Correo = nuevoCorreo;

            var nuevaDireccion = await DisplayPromptAsync("Editar Persona", "Nueva direcci�n", "Guardar", "Cancelar", placeholder: "Direcci�n", keyboard: Keyboard.Text, initialValue: persona.Direccion);

            if (nuevaDireccion != null)
                persona.Direccion = nuevaDireccion;

            try
            {
                // Validaciones antes de la actualizaci�n
                if (string.IsNullOrEmpty(persona.Nombre) || string.IsNullOrEmpty(persona.Apellido))
                {
                    MostrarMensaje("El nombre y el apellido son obligatorios.");
                    return;
                }

                // Realizar la actualizaci�n en la base de datos
                bool actualizacionExitosa = await _viewModel.PersonasController.StoreOrUpdatePerson(persona);

                // Verificar si la actualizaci�n fue exitosa
                if (actualizacionExitosa)
                {
                    await _viewModel.ActualizarListaPersonas();
                    MostrarMensaje("Actualizaci�n Exitosa");
                }
                else
                {
                    MostrarMensaje("Error al actualizar la persona. Por favor, int�ntelo de nuevo.");
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al actualizar la persona: {ex.Message}");
            }
        }


        private async void Eliminar_Clicked(object sender, EventArgs e)
        {
            var persona = (Models.Personas)((Button)sender).CommandParameter;

            // Mostrar un mensaje de confirmaci�n
            bool result = await DisplayAlert("Confirmaci�n", "�Seguro que quieres eliminar esta persona?", "S�", "No");

            if (result)
            {
                // Llamar al m�todo para eliminar la persona de la base de datos
                int deletedRows = await _viewModel.EliminarPersona(persona);

                if (deletedRows > 0)
                {
                    // Actualizar la lista de personas despu�s de la eliminaci�n
                    await _viewModel.CargarPersonas();
                }
            }
        }

        private void MostrarMensaje(string mensaje)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Mensaje", mensaje, "OK");
            });
        }
    }
}
