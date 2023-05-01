using HuffmanTreeNameSpace;
using BitStringNameSpace;
using System;
using DecodeEncodeNameSapace;
using System.Text.Json;


namespace Program{
    class Program{
        public void CreateArchive(string input_path, string output_path){
            
        }
        public static void Main(string[] SysArgv){
            FromFileConstructor constructor = new FromFileConstructor("test_files/src_copy.txt");
            HuffmanTree tree = constructor.GetTree();
            var t = constructor.GetFrequencies();

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize<Dictionary<byte, ulong>>(t, options);

            Console.WriteLine(jsonString);
        }
    }
}
