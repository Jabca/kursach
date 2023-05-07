using System.Text;
using HuffmanTreeNamespace;

namespace FileUtilsNamespace{
    public class TreeFromSourceConstructor{
        /// form necessary data from source file
        private Dictionary<byte, uint> frequencies = new Dictionary<byte, uint>();
        public TreeFromSourceConstructor(string file_path, int chunk_size = 1024 * 1024 * 16){
            // creates dictionary where key is encoded byte and value is number of it occurrence in file
            using(FileStream fs = File.OpenRead(file_path)){
                byte cur_byte;
                byte[] read_buffer = new byte[chunk_size];
                int bytes_read;
                // read chunk of file
                while((bytes_read = fs.Read(read_buffer, 0, chunk_size)) > 0){
                    // iterate over read values
                    for(int i = 0; i < bytes_read; i++){
                        cur_byte = read_buffer[i];
                        // if byte is in dictionary add 1 to it's value, otherwise add it to dictionary
                        if(frequencies.TryGetValue(cur_byte, out uint val)){
                            frequencies[cur_byte] += 1;
                        }
                        else{
                            frequencies[cur_byte] = 1;
                        }
                    }
                }
            }
        }

        public HuffmanTree GetTree(){
            /// returns HuffmanTree object formed from source file
            return new HuffmanTree(frequencies);
        }

        public Dictionary<byte, uint> GetFrequencies(){
            /// returns dictionary of frequencies
            return frequencies;
        }

        public byte[] GetSerializedFrequencies(){
            /// returns frequencies dictionary as serialized array of bytes
            byte[] byte_buffer = new byte[(frequencies.Count+1) * 5];
            int index = 0;
            byte[] uint_byte_buffer = new byte[4];
            // iterate over dictionary
            foreach(var pair in frequencies){
                // each pair is serialized as 5 bytes
                // assign first byte to byte being encoded
                byte_buffer[index] = pair.Key;
                // encode uint as array of 4 bytes
                uint_byte_buffer = BitConverter.GetBytes(pair.Value);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(uint_byte_buffer);
                // assigns 4 bytes to serialized uint
                for(int i = 0; i < 4; i++){
                    byte_buffer[index + 1 + i] = uint_byte_buffer[i];
                }
                index += 5;
            }
            // separator between frequencies info and other data is 
            for(int i = index; i < index + 5; i++){
                byte_buffer[i] = 0;
            }
            
            return byte_buffer;
        }
    }

    public class TreeFromArchiveConstructor{
        /// decodes info needed for decoding an ssaf archive
        
        // pointer of first encoded data byte
        public uint data_start_address;
        Dictionary<byte, uint> frequencies = new Dictionary<byte, uint>();
        long original_file_length;
        string original_file_name;
        public TreeFromArchiveConstructor(string path_to_file){
            /// constructs HuffmanTree object from archive
            data_start_address = 0;
            using(FileStream fs = File.OpenRead(path_to_file)){
                int tmp_int;
                byte cur_byte;
                uint byte_frequency;
                byte[] byte_buffer = new byte[4];
                byte[] file_size_buffer = new byte[8];
                List<byte> file_name_buffer = new List<byte>();
                // read data until we encounter separator - 5 bytes of 0b0
                while(true){
                    // read byte value
                    tmp_int = fs.ReadByte();
                    cur_byte = Convert.ToByte(tmp_int);
                    // read and decode byte number of occurrences
                    for(int i = 0; i < 4; i++){
                        byte_buffer[i] = (byte)fs.ReadByte();
                    }
                    if(BitConverter.IsLittleEndian){
                        Array.Reverse(byte_buffer);
                    }
                    byte_frequency = BitConverter.ToUInt32(byte_buffer);
                    // if byte is had never occurred in a file it wouldn't be written to archive
                    // that means we found separator
                    if(byte_frequency == 0){
                        break;
                    }
                    // add pair to dictionary
                    frequencies[cur_byte] = byte_frequency;
                    data_start_address += 5;
                }
                data_start_address += 5;

                // process 8 bytes as ulong - length of original file in bytes
                fs.Read(file_size_buffer, 0, 8);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(file_size_buffer);
                original_file_length = BitConverter.ToInt64(file_size_buffer);
                data_start_address += 8;

                // process bytes as name of original file
                do{
                    file_name_buffer.Add(Convert.ToByte(fs.ReadByte()));
                }
                // 35 in byte is '#' in char which is indicates that end of name was reached
                while(file_name_buffer.Last() != 35);

                // decode array of bytes as string to get file name
                original_file_name = Encoding.UTF8.GetString(file_name_buffer.ToArray(), 0,  file_name_buffer.Count);
                // remove '#' from end of file name
                original_file_name = original_file_name.Substring(0, original_file_name.Length - 1);

                data_start_address += (uint)file_name_buffer.Count;

            }
        }
        public HuffmanTree GetTree(){
            /// get HuffmanTree object constructed from archive data
            return new HuffmanTree(frequencies);
        }

        public uint GetDataStartAddress(){
            /// get address of first byte which encodes files data
            return data_start_address;
        }

        public long GetOriginalFileLength(){
            /// get original file length in bytes
            return original_file_length;
        }

        public Dictionary<byte, uint> GetFrequencies(){
            /// return dictionary of bytes occurrence in file
            return frequencies;
        }

        public string GetOriginalFileName(){
            // return name of encoded file
            return original_file_name;
        }
    }
}