using HuffmanTreeNameSpace;
using BitStringNameSpace;
using System;
using FileUtilsNameSpace;
using EncodeDecodeNameSpace;

namespace Program{
    class Program{
        public void CreateArchive(string input_path, string output_path){

        }
        public static void Main(string[] SysArgv){
            var encoder = new FileEncoder();
            var decoder = new FileDecoder();

            encoder.EncodeFile("test_files/src.txt", "test_files/a.ssaf");
            decoder.DecodeFile("test_files/a.ssaf", "test_files/test_decode.txt");
        }
    }
}
