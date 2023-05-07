using HuffmanTreeNamespace;
using BitStringNamespace;
using System;
using FileUtilsNamespace;
using EncodeDecodeNamespace;
using ExecutorNamespace;

namespace Program{
    class Program{
        /// Program to compress and decompress files using ssaf archive format
        public static void Main(string[] SysArgv){
            /// main function
            // construct executor object based on console arguments 
            Executor exe = new Executor(SysArgv);
            // check whether display help message
            if(exe.returnHelp()){
                Console.WriteLine(exe.getHelpMessage());
                return;
            }
            // check whether display error message
            if(exe.getErrorMessage() != ""){
                Console.WriteLine(exe.getErrorMessage());
                return;
            }
            try{
                // try executing program
                string message = exe.Execute();
                Console.WriteLine(message);
            }
            catch(Exception e){
                // catch exception
                Console.WriteLine("Something went wrong");
                Console.WriteLine(e.Message);
            }
            
        }
    }
}
