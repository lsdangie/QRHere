using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SQLite;
using System.ComponentModel;

namespace QRHere
{
    [Table("Usuarios")]
    public class Usuarios: INotifyPropertyChanged
    {
        private int _id;
        private string _username;
        private string _password;
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                this._id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        [NotNull, MaxLength(50)]
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                this._username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        [MaxLength(20)]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                this._password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this,
              new PropertyChangedEventArgs(propertyName));
        }
    }
}