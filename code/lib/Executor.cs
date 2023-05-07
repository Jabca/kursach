using EncodeDecodeNamespace;

namespace ExecutorNamespace{
    public class Executor{
        /// executes main program
        string error_message = "";
        string help_message = 
        """
        Usage:
            ssaf <source_path> <arguments>
        Description:
            ssaf - Compress and and decompress any file. Transforms source file (archive or file) to target file (file or archive respectively).
        Base behavior:
            if source_file is .ssaf a file with original name will be created. Otherwise creates archive with original name and .ssaf extension.
        Options:
            1. -h / --help — display help message.
            2. -p / --path <file_path> — path to target file.
            3. -e / --encode — force encode file.
            4. -d / --decode — force decode archive.
            5. -cs / --chunk_size <uint bytes> — size of chunk that source file will be split to(>=1);
        """;

        public string getErrorMessage(){return error_message;}
        public string getHelpMessage(){return help_message;}
        public bool returnHelp(){return display_help;}
        // init parameters of program
        string input_path =  "#";
        string output_path = "#";
        string source_name = "#";
        bool encode = false;
        bool decode = false;
        bool display_help=  false;
        int chunk_size_var = 0;
        
        public void FormatAndCheckNames(){
            /// Fills and checks parameters needed for program execution using provided console arguments

            // check whether source file exists 
            if(!Path.Exists(input_path)){
                error_message = "no such file '" + input_path + "'";
                return;
            }
            source_name = Path.GetFileName(input_path);

            // check whether there is no conflict about mode
            if(encode && decode){
                error_message = "choose to encode or decode";
                return;
            }

            // if mode is not user provided decide based on source file extension
            if(!(encode || decode)){
                // if source file is archive we are decoding it
                if(Path.GetExtension(source_name) == ".ssaf"){
                    decode = true;
                }
                // otherwise we are decoding it
                else{
                    encode = true;
                }
            }


            // if no output_path provided and decoding the name will be formed later when executing decoding
            if(output_path == "#" && decode){
                output_path = Path.GetDirectoryName(input_path) + 
                              Path.DirectorySeparatorChar + 
                              "#";
            }

            // if no name provided create name 
            if(output_path == "#" && encode){
                output_path = Path.GetDirectoryName(input_path) + 
                              Path.DirectorySeparatorChar +
                              Path.GetFileNameWithoutExtension(source_name) +
                              ".ssaf";
            }

            // check if file with this path exists
            if(Path.Exists(output_path)){
                error_message = "File '" + output_path + "' already exists";
                return;
            }
            
        }
        public Executor(string[] sys_args){
            /// Forms necessary data for program execution from command line arguments
            if(sys_args.Length == 0){
                error_message = "No arguments were provided";
                return;
            }
            input_path = sys_args[0];
            // parse arguments separated by space
            for(int i = 1; i < sys_args.Length; i++){
                switch(sys_args[i]){
                    // display help
                    case "-h": case "--help":
                        display_help = true;
                        break;
                    // provide path for target file
                    case "-p": case "--path":
                        i++; 
                        output_path = sys_args[i];
                        break;
                    // encode file
                    case "-e": case "--encode":
                        encode = true;
                        break;
                    // decode archive
                    case "-d": case "--decode":
                        decode = true;
                        break;
                    // chunks size
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
                            error_message = "couldn't convert '" + sys_args[i] + "' to int";
                            return;
                        }
                        break;
                    // arg didn't satisfy template 
                    default:
                        error_message = "unexpected argument '" + sys_args[i] + "'";
                        display_help = true;
                        return;
                }
            }
            this.FormatAndCheckNames();
        }

        public string Execute(){
            /// encodes or decodes file based on formed arguments. Returns message to be displayed
            // if chunk size is correct value use it, otherwise use default value of 16 MB.
            if(encode){
                var encoder = new FileEncoder();
                if(chunk_size_var==0){
                    return encoder.EncodeFile(input_path, output_path);
                }
                else{
                    return encoder.EncodeFile(input_path, output_path, chunk_size_var);
                }
            }
            else{
                var decoder = new FileDecoder();
                if(chunk_size_var == 0){
                    return decoder.DecodeFile(input_path, output_path);
                }
                else{
                    return decoder.DecodeFile(input_path, output_path, chunk_size_var);
                }
            }
        }
    }
}
