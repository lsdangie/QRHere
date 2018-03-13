using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace QRHere
{

    class UsuariosDataAccess
    {
        private SQLiteConnection database;
        private static object collisionLock = new object();
        public ObservableCollection<Usuarios> Usuarios { get; set; }

        public UsuariosDataAccess()
        {
            database = DependencyService.Get<IDatabaseConnection>().DbConnection();
            database.CreateTable<Usuarios>();
            this.Usuarios = new ObservableCollection<Usuarios>(database.Table<Usuarios>());
            // If the table is empty, initialize the collection
            if (!database.Table<Usuarios>().Any())
            {
                AddNewUsuario();
            }
        }

        public void AddNewUsuario()
        {
            this.Usuarios.Add(new Usuarios
              {
                  Username = "Username...",
                  Password = "Password..."
              });
        }

        // Use LINQ to query and filter data
        public IEnumerable<Usuarios> GetFilteredUsuarios(string username)
        {
            // Use locks to avoid database collitions
            lock (collisionLock)
            {
                var query = from cust in database.Table<Usuarios>()
                            where cust.Username == username
                            select cust;
                return query.AsEnumerable();
            }
        }

        // Use SQL queries against data
        public IEnumerable<Usuarios> GetFilteredUsuario()
        {
            lock (collisionLock)
            {
                return database.Query<Usuarios>("SELECT * FROM Usuarios").AsEnumerable();
            }
        }

        public Usuarios GetUsuario(int id)
        {
            lock (collisionLock)
            {
                return database.Table<Usuarios>().FirstOrDefault(usuario => usuario.Id == id);
            }
        }
        public int SaveUsuario(Usuarios usuarioInstance)
        {
            lock (collisionLock)
            {
                if (usuarioInstance.Id != 0)
                {
                    database.Update(usuarioInstance);
                    return usuarioInstance.Id;
                }
                else
                {
                    database.Insert(usuarioInstance);
                    return usuarioInstance.Id;
                }
            }
        }

        public void SaveAllUsuario()
        {
            lock (collisionLock)
            {
                foreach (var usuarioInstance in this.Usuarios)
                {
                    if (usuarioInstance.Id != 0)
                    {
                        database.Update(usuarioInstance);
                    }
                    else
                    {
                        database.Insert(usuarioInstance);
                    }
                }
            }
        }

        public int DeleteUsuario(Usuarios usuarioInstance)
        {
            var id = usuarioInstance.Id;
            if (id != 0)
            {
                lock (collisionLock)
                {
                    database.Delete<Usuarios>(id);
                }
            }
            this.Usuarios.Remove(usuarioInstance);
            return id;
        }

        public void DeleteAllCustomers()
        {
            lock (collisionLock)
            {
                database.DropTable<Usuarios>();
                database.CreateTable<Usuarios>();
            }
            this.Usuarios = null;
            this.Usuarios = new ObservableCollection<Usuarios>(database.Table<Usuarios>());
        }

    }
}