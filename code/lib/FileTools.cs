using System;
using HuffmanTreeNameSpace;

namespace FileUtilsNameSpace{
    public class TreeFromSourceConstructor{
        private Dictionary<byte, uint> frequencies = new Dictionary<byte, uint>();
        public TreeFromSourceConstructor(string file_path){
            using(FileStream fs = File.OpenRead(file_path)){
                byte cur_byte;
                int tmp;
                for(int i = 0; i < fs.Length; i++){
                    tmp = fs.ReadByte();
                    if(tmp == -1){
                        throw new FileLoadException("File end unexpectedly encountered");
                    }
                    cur_byte = Convert.ToByte(tmp);
                    if(frequencies.TryGetValue(cur_byte, out uint val)){
                        frequencies[cur_byte] += 1;
                    }
                    else{
                        frequencies[cur_byte] = 1;
                    }
                }
            }
        }

        public HuffmanTree GetTree(){
            return new HuffmanTree(frequencies);
        }

        public Dictionary<byte, uint> GetFrequencies(){
            return frequencies;
        }

        public byte[] SerializeFrequencies(){
            byte[] byte_buffer = new byte[(frequencies.Count+1) * 5];
            int index = 0;
            byte[] uint_byte_buffer = new byte[4];
            foreach(var pair in frequencies){
                byte_buffer[index] = pair.Key;
                uint_byte_buffer = BitConverter.GetBytes(pair.Value);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(uint_byte_buffer);
                for(int i = 0; i < 4; i++){
                    byte_buffer[index + 1 + i] = uint_byte_buffer[i];
                }
                index += 5;
            }
            return byte_buffer;

        }
    }

    public class TreeFromArchiveConstructor{
        public uint data_start_address;
        Dictionary<byte, uint> frequiencies = new Dictionary<byte, uint>();

        long original_file_length;
        public TreeFromArchiveConstructor(string path_to_file){
            data_start_address = 0;
            using(FileStream fs = File.OpenRead(path_to_file)){
                int tmp_int;
                byte cur_byte;
                uint byte_frequency;
                byte[] byte_buffer = {0, 0, 0, 0};
                byte[] file_size_buffer = new byte[8];
                while(true){
                    tmp_int = fs.ReadByte();
                    if(tmp_int == -1){
                        throw new FileLoadException("File end unexpectedly encountered");
                    }
                    cur_byte = Convert.ToByte(tmp_int);
                    for(int i = 0; i < 4; i++){
                        byte_buffer[i] = (byte)fs.ReadByte();
                    }
                    if(BitConverter.IsLittleEndian){
                        Array.Reverse(byte_buffer);
                    }
                    byte_frequency = BitConverter.ToUInt32(byte_buffer);
                    if(byte_frequency == 0){
                        break;
                    }
                    frequiencies[cur_byte] = byte_frequency;
                    data_start_address += 5;
                }
                data_start_address += 5;
                fs.Read(file_size_buffer, 0, 8);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(file_size_buffer);
                original_file_length = BitConverter.ToInt64(file_size_buffer);
                data_start_address += 8;
            }
        }
        public HuffmanTree GetTree(){
            return new HuffmanTree(frequiencies);
        }

        public uint GetDataStartAddress(){
            return data_start_address;
        }

        public long GetOriginalFileLength(){
            return original_file_length;
        }

        public Dictionary<byte, uint> GetFrequencies(){
            return frequiencies;
        }
    }
}