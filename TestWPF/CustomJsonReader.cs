using System.Collections.Generic;
using Newtonsoft.Json;
namespace TestWPF
{
    class CustomJsonReader
    {
        
        private Data GetCurrentData(Stack<Data> dataStack)
        {
            return dataStack.Count == 0 ? new Data() : dataStack.Peek();
        }


        private void IterateDataList(JsonReader reader, Data data)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndArray) { return; }
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        var list = new List<Data>();
                        data.ArrayPairs.Add(list);
                        Process(reader, new Stack<Data>(), list);
                        break;
                    case JsonToken.String:
                    case JsonToken.Integer:
                    case JsonToken.Float:
                        data.ArrayPairs = null;
                        data.Collection.Add(reader.Value.ToString());
                        break;
                    default:
                        continue;
                }
            }
        }

        private void ParseReader(JsonReader reader, Stack<Data> dataStack)
        {
            Data currentData = GetCurrentData(dataStack);
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    if (dataStack.Count == 0)
                    {
                        return;
                    }

                    currentData.Pairs = new List<Data>();
                    Process(reader, new Stack<Data>(), currentData.Pairs);
                    break;
                case JsonToken.PropertyName:
                    currentData = new Data();
                    dataStack.Push(currentData);
                    currentData.Key = reader.Value.ToString();
                    break;
                case JsonToken.String:
                case JsonToken.Integer:
                case JsonToken.Float:
                    currentData = GetCurrentData(dataStack);
                    currentData.Value = reader.Value.ToString();
                    break;
                case JsonToken.StartArray:
                    currentData = GetCurrentData(dataStack);
                    currentData.ArrayPairs = new List<List<Data>>();
                    currentData.Collection = new List<string>();

                    IterateDataList(reader, currentData);
                    break;
                case JsonToken.EndArray:
                case JsonToken.EndObject:
                    return;
            }
        }

        public void Process(JsonReader reader, Stack<Data> dataStack, List<Data> datas)
        {
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject) { return; }

                ParseReader(reader, dataStack);
                switch (reader.TokenType)
                {
                    case JsonToken.PropertyName:
                        datas.Add(dataStack.Peek());
                        break;
                    case JsonToken.String:
                    case JsonToken.Integer:
                    case JsonToken.Float:
                        dataStack.Pop();
                        break;
                }
            }
        }

    }

    public class Data
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public List<string> Collection { get; set; }
        public List<Data> Pairs { get; set; }
        public List<List<Data>> ArrayPairs { get; set; }
    }
}

