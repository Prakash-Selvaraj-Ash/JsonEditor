using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace TestWPF
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public IEnumerable<Data> Source { get; set; }

        public ViewModel()
        {
            var jsonString = File.ReadAllText("config.json");
            JsonTextReader reader = new JsonTextReader(new StringReader(jsonString));
            Stack<Data> dataStack = new Stack<Data>();
            var list = new List<Data>();
            new CustomJsonReader().Process(reader, dataStack, list);

            Source = list; 
        }
    }
}
