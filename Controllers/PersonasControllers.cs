using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using Tarea_1._2.Models;

namespace Tarea_1._2.Controllers
{
    public class PersonasController
    {
        SQLiteAsyncConnection _connection;

        // Constructor vacío
        public PersonasController() { }

        // Inicialización de la conexión a la base de datos
        public async Task Init()
        {
            if (_connection != null)
            {
                return;
            }

            SQLiteOpenFlags flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, "DBPersonas.db3"), flags);
            await _connection.CreateTableAsync<Personas>();
        }

        // Método para obtener todas las personas
        public async Task<List<Personas>> ObtenerTodasLasPersonas()
        {
            await Init();
            return await _connection.Table<Personas>().ToListAsync();
        }

        // Método para almacenar o actualizar una persona
        public async Task<bool> StoreOrUpdatePerson(Personas persona)
        {
            await Init();

            if (persona.Id == 0)
            {
                return await _connection.InsertAsync(persona) > 0;
            }
            else
            {
                return await _connection.UpdateAsync(persona) > 0;
            }
        }





        // Método para eliminar una persona
        public async Task<int> DeletePerson(Personas persona)
        {
            await Init();
            return await _connection.DeleteAsync(persona);
        }
    }
}
