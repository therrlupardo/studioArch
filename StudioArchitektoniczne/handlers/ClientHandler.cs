using System;
using System.Collections.Generic;
using System.Linq;
using ArchitecturalStudio.interfaces;
using ArchitecturalStudio.models;
using ArchitecturalStudio.Properties;

namespace ArchitecturalStudio.handlers
{
    public class ClientHandler: AbstractHandler, IGenerator, IWritable
    {
        public List<Client> Clients { get; set; }

        public ClientHandler()
        {
            Clients = new List<Client>();
        }
        
        public void Generate(int amount, params object[] parameters)
        {
            var index = Clients.Any() ? Clients.Count : 0;
            for (var i = 0; i < amount; i++) Clients.Add(new Client(index + i));
        }

        public void Write(string time)
        {
            var dataModels = Clients.Cast<AbstractDataModel>().ToList();
            WriteToBulk(dataModels, $"{Resources.Global_Data_Path}clients_{time}.bulk");
        }
    }
}