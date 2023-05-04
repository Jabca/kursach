using FileUtilsNameSpace;
using HuffmanTreeNameSpace;
using BitStringNameSpace;

namespace EncodeDecodeNameSpace{
    public class FileDecoder{
        public void DecodeFile(string intput_file, string output_file, int chunk_size = 1024 * 1024 * 32){
            var tree_decoder = new TreeFromArchiveConstructor(intput_file);
            HuffmanTree tree = tree_decoder.GetTree();
            uint data_start = tree_decoder.GetDataStartAddress();

            byte? write_buffer;
            int tmp_int;
            long original_file_size = tree_decoder.GetOriginalFileLength();
            long bits_written = 0;  
            BitString buffer = new BitString();

            using(FileStream source_fs = File.OpenRead(intput_file))
            using(FileStream target_fs = File.Create(output_file)){
                source_fs.Seek(data_start, 0);
                for(uint i = data_start; i < source_fs.Length; i++){
                    tmp_int = source_fs.ReadByte();
                    if(tmp_int == -1){
                        target_fs.Close();
                        throw new FileLoadException("File end unexpectedly encountered");
                    }
                    buffer.AppendRight(new BitString((byte)tmp_int));
                    do{
                        write_buffer = tree.LiveDecode(buffer.GetLeft());
                        buffer.PopLeft();
                        if(!(write_buffer is null)){
                            target_fs.WriteByte((byte)write_buffer);
                            bits_written += 1;
                            if(bits_written >= original_file_size){
                                break;
                            }
                        } 
                    }
                    while(!buffer.IsEmpty());
                }
            }
        }
    }

    public class FileEncoder{
        public void EncodeFile(string intput_file, string output_file){
            var tree_decoder = new TreeFromSourceConstructor(intput_file);
            HuffmanTree tree = tree_decoder.GetTree();
            var frequencies = tree_decoder.GetFrequencies();

            byte read_buffer;
            byte write_buffer;
            int tmp_int;
            long file_length;
            byte[] file_length_buffer;
            BitString buffer = new BitString();
            BitString encodeValue;

            byte[] serizlized_tree_info = tree_decoder.SerializeFrequencies();

            using(FileStream source_fs = File.OpenRead(intput_file))
            using(FileStream target_fs = File.Create(output_file)){
                target_fs.Write(serizlized_tree_info, 0, serizlized_tree_info.Count());
                file_length = source_fs.Length;
                file_length_buffer = BitConverter.GetBytes(file_length);
                if(BitConverter.IsLittleEndian){
                    Array.Reverse(file_length_buffer);
                }
                target_fs.Write(file_length_buffer, 0, 8);
                for(int i = 0; i < source_fs.Length; i++){
                    tmp_int = source_fs.ReadByte();
                    if(tmp_int == -1){
                        throw new FileLoadException("File end unexpectedly encountered");
                    }
                    read_buffer = (byte)tmp_int;
                    encodeValue = tree.EncodeValue(read_buffer);
                    buffer.AppendRight(encodeValue);
                    while(buffer.getLength() >= 8){
                        write_buffer = buffer.GetFirstByte();
                        target_fs.WriteByte(write_buffer);
                        buffer.PopFirstByte();
                    }
                }
                if(!buffer.IsEmpty()){
                    buffer.fillToByte();
                    target_fs.WriteByte(buffer.GetFirstByte());
                }
            }
        }
    }
}