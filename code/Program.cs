using HuffmanTreeNamespace;
using BitStringNamespace;
using System;
using FileUtilsNamespace;
using EncodeDecodeNamespace;
using ExecutorNamespace;

namespace Program{
    class Program{
        public static void Main(string[] SysArgv){
            Executor exe = new Executor(SysArgv);
            if(exe.returnHelp()){
                Console.WriteLine(exe.getHelpMessage());
                return;
            }
            if(exe.getErrorMessage() != ""){
                Console.WriteLine(exe.getErrorMessage());
                return;
            }
            exe.Execute();
            Console.WriteLine(exe.getMessage());
        }
    }
}
