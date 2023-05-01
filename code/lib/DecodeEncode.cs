using System;
using HuffmanTreeNameSpace;

namespace DecodeEncodeNameSapace{
    public class FromFileConstructor{
        private Dictionary<byte, int> frequencies = new Dictionary<byte, int>(0);
        public FromFileConstructor(string file_path){
            using(FileStream fs = File.OpenRead(file_path)){
                byte cur_byte;
                int tmp;
                for(int i = 0; i < fs.Length; i++){
                    tmp = fs.ReadByte();
                    if(tmp == -1){
                        throw new FieldAccessException("File end unexpectedly encountered");
                    }
                    cur_byte = Convert.ToByte(tmp);
                    if(frequencies.TryGetValue(cur_byte, out int val)){
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
    }
}