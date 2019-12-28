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
        private readonly Random _random;
        public ClientHandler()
        {
            Clients = new List<Client>();
            _random = new Random(int.Parse(Resources.Global_Random_Seed));
        }
        
        public void Generate(int amount, params object[] parameters)
        {
            for (var i = 0; i < amount; i++) Clients.Add(new Client(Clients.Count + 1));
        }

        public void Write(string time)
        {
            var dataModels = Clients.Cast<AbstractDataModel>().ToList();
            WriteToBulk(dataModels, $"{Resources.Global_Data_Path}clients_{time}.bulk");
        }

        public Client GetRandomClient()
        {
            return Clients[_random.Next(Clients.Count)];
        }
    }
}