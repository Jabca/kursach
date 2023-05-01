using HuffmanTreeNameSpace;
using BitStringNameSpace;
using System;
using DecodeEncodeNameSapace;

namespace Program{
    class Program{
        public static void Main(string[] SysArgv){
            FromFileConstructor constructor = new FromFileConstructor("test_files/src_copy.txt");
            HuffmanTree tree = constructor.GetTree();
            
            BitString e_code = tree.EncodeValue((byte)'e');
            Console.WriteLine(e_code);
            byte e_value = tree.DecodeValue(e_code);
            Console.WriteLine((char)e_value);
            
            // BitString c = new BitString();
            // c.InitWithChar('e');
            // c.DelLeft();
        }
    }
}
