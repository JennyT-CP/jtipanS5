using jtipanS5.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace jtipanS5.Repositories
{
    public class PersonRepository
    {
        string _dbPath;
        private SQLiteConnection _conn;
        public string StatusMessage { get; set; }
        public PersonRepository(string path)
        {
            _dbPath = path;
        }
        private void Init()
        {
            if (_conn is not null)
                return;

            _conn = new(_dbPath);
            _conn.CreateTable<Persona>();
        }

        // INSERTAR
        public void AddNewPerson(string name)
        {
            int result = 0;

            try
            {
                Init();
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Nombre es requerido");
                Persona persona = new() { Name = name };
                result = _conn.Insert(persona);
                StatusMessage = string.Format("Dato Ingresado");
            }
            catch (Exception ex)
            {
                StatusMessage = string.Format("Error " + ex.Message);
            }

        }

        // SELECT
        public List<Persona>GetAllPerson()
        {
            try
            {
                Init();
                return _conn.Table<Persona>().ToList();
            }
            catch (Exception ex)
            {

                StatusMessage = string.Format("Error " + ex.Message);
            }

            return new List<Persona>();
        }

        // DELETE
        public void DeletePerson(int id)
        {
            int result = 0;

            try
            {
                Init();

                result = _conn.Delete<Persona>(id);

                if (result > 0)
                {
                    StatusMessage = "Persona eliminada correctamente";
                }
                else
                {
                    StatusMessage = "No se encontró la persona";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "Error " + ex.Message;
            }
        }


        // UPDATE


    }
}
