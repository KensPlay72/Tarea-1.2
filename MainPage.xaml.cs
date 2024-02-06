using System;

namespace Tarea_1._2
{
    public partial class MainPage : ContentPage
    {
        Controllers.PersonasController personasController;

        public MainPage()
        {
            InitializeComponent();
            personasController = new Controllers.PersonasController();
        }

        async void Save_info(System.Object sender, System.EventArgs e)
        {
            var person = new Models.Personas
            {
                Nombre = Nombre.Text,
                Apellido = Apellido.Text,
                Correo = correo.Text,
                Direccion = direccion.Text,
            };

            if (!string.IsNullOrWhiteSpace(Edad.Text) && int.TryParse(Edad.Text, out int edad))
            {
                person.Edad = edad;

                try
                {
                    if (await personasController.StoreOrUpdatePerson(person))
                    {
                        await DisplayAlert("Aviso", "Registro Ingresado con éxito!!", "ok");

                        // Después de guardar, navega a la página de listado
                        await Navigation.PushAsync(new ListadoPersonasPage());
                    }
                    else
                    {
                        await DisplayAlert("Aviso", "Error al almacenar la persona", "ok");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al almacenar la persona: {ex.Message}");
                    await DisplayAlert("Error", "Hubo un problema al almacenar la persona.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Aviso", "Ingrese una edad válida.", "ok");
            }
        }


        async void VerListado_Clicked(System.Object sender, System.EventArgs e)
        {
            // Navegar a la página de listado
            await Navigation.PushAsync(new ListadoPersonasPage());
        }
    }

}
