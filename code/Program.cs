using HuffmanTreeNameSpace;
using BitStringNameSpace;
using System;
using DecodeEncodeNameSapace;

namespace Program{
    class Program{
        public static void Main(string[] SysArgv){
            FromFileConstructor constructor = new FromFileConstructor("test_files/src_copy.txt");
            HuffmanTree tree = constructor.GetTree();

            
        }
    }
}
