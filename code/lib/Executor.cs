using EncodeDecodeNamespace;

namespace ExecutorNamespace{
    public class Executor{
        string message = "Complete";
        string error_message = "";
        string help_message = 
        """
        Usage:
            ssaf <source_path> <arguments>
        Description:
            ssaf - Compress and and decompress any file. Transfroms source file (archive or file) to target file (file or archive respectively).
        Options:
            1. -h / --help — display help message.
            2. -p / --path <file_path> — path to target file. You must provide name.
            3. -e / --encode — force encode file.
            4. -d / --decode — force decode archive.
            5. -cs / --chunk_size <uint bytes> — size of chunk that source file will be split to(>=1);
        """;

        public string getErrorMessage(){return error_message;}
        public string getHelpMessage(){return help_message;}
        public string getMessage(){return message;}
        public bool returnHelp(){return display_help;}
        string input_path =  "#";
        string output_path = "#";
        string source_name = "#";
        bool encode = false;
        bool decode = false;
        bool display_help=  false;
        int chunk_size_var = (int)1;
        
        public void FormatAndCheckNames(){
            if(!Path.Exists(input_path)){
                error_message = "no such file '" + input_path + "'";
                return;
            }
            source_name = Path.GetFileName(input_path);

            if(encode && decode){
                error_message = "choose to encode or decode";
                return;
            }

            if(!(encode || decode)){
                if(Path.GetExtension(source_name) == ".ssaf"){
                    decode = true;
                }
                else{
                    encode = true;
                }
            }

            if(output_path == "#" && decode){
                error_message = "you must provide path when decoding";
                return;
            }

            if(output_path == "#"){
                output_path = Path.GetDirectoryName(input_path) + 
                Path.DirectorySeparatorChar +
                Path.GetFileNameWithoutExtension(source_name) +
                ".ssaf";
            }
            else{
                if(Path.Exists(output_path)){
                    error_message = "This file already exists";
                    return;
                }
            }
            
        }
        public Executor(string[] sys_args){
            input_path = sys_args[0];
            for(int i = 1; i < sys_args.Length; i++){
                switch(sys_args[i]){
                    case "-h": case "--help":
                        display_help = true;
                        break;
                    case "-p": case "--path":
                        i++; 
                        output_path = sys_args[i];
                        break;
                    case "-e": case "--encode":
                        encode = true;
                        break;
                    case "-d": case "--decode":
                        decode = true;
                        break;
                    case "-cs": case "--chunk_size":
                        i++;
                        try{
                            chunk_size_var = Convert.ToInt32(sys_args[i]);
                            if(chunk_size_var <= 0){
                                error_message = "chunk size can't be zero or less";
                                return;
                            }
                        }
                        catch(FormatException){
                            error_message = "couldn't conver '" + sys_args[i] + "' to uint";
                            return;
                        }
                        break;
                    default:
                        error_message = "unexpected argument '" + sys_args[i] + "'";
                        display_help = true;
                        return;
                }
            }
            this.FormatAndCheckNames();
        }

        public void Execute(){
            if(encode){
                var encoder = new FileEncoder();
                if(chunk_size_var==0){
                    encoder.EncodeFile(input_path, output_path);
                }
                else{
                    encoder.EncodeFile(input_path, output_path, chunk_size_var);
                }
            }
            else{
                var decoder = new FileDecoder();
                if(chunk_size_var == 0){
                    decoder.DecodeFile(input_path, output_path);
                }
                else{
                    decoder.DecodeFile(input_path, output_path, chunk_size_var);
                }
            }
        }
    }
}
