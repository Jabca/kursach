using FileUtilsNamespace;
using HuffmanTreeNamespace;
using BitStringNamespace;

namespace EncodeDecodeNamespace{
    public class FileDecoder{
        public void DecodeFile(string intput_file, string output_file, int chunk_size = 1024 * 1024 * 16){
            var tree_decoder = new TreeFromArchiveConstructor(intput_file);
            HuffmanTree tree = tree_decoder.GetTree();
            uint data_start = tree_decoder.GetDataStartAddress();

            byte? decode_buffer;
            long original_file_size = tree_decoder.GetOriginalFileLength();
            long bits_written = 0;
            int bytes_read; 
            BitString buffer = new BitString();

            byte[] read_buffer = new byte[chunk_size];
            List<byte> write_buffer = new List<byte>();

            using(FileStream source_fs = File.OpenRead(intput_file))
            using(FileStream target_fs = File.Create(output_file)){
                source_fs.Seek(data_start, 0);
                while((bytes_read = source_fs.Read(read_buffer, 0, chunk_size)) > 0){
                    write_buffer.Clear();
                    for(int i = 0; i < bytes_read; i++){
                        buffer.AppendRight(new BitString(read_buffer[i]));
                        do{
                            decode_buffer = tree.LiveDecode(buffer.PopLeft());
                            if(!(decode_buffer is null)){
                                write_buffer.Add(((byte)decode_buffer));
                                bits_written += 1;
                                if(bits_written >= original_file_size){
                                    target_fs.Write(write_buffer.ToArray(), 0, write_buffer.Count);
                                    write_buffer.Clear();
                                    break;
                                }
                            } 
                        }
                        while(!buffer.IsEmpty());
                    }
                    target_fs.Write(write_buffer.ToArray(), 0, write_buffer.Count);
                }
            }
        }
    }

    public class FileEncoder{
        public void EncodeFile(string intput_file, string output_file, int chunk_size=1024 * 1024 * 16){
            var tree_decoder = new TreeFromSourceConstructor(intput_file);
            HuffmanTree tree = tree_decoder.GetTree();
            var frequencies = tree_decoder.GetFrequencies();

            int bytes_read;
            long file_length;
            byte[] file_length_buffer;
            BitString buffer = new BitString();
            BitString encodeValue;

            byte[] read_buffer = new byte[chunk_size];
            List<byte> write_buffer = new List<byte>();

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
                                while((bytes_read = source_fs.Read(read_buffer, 0, chunk_size)) > 0){
                    write_buffer.Clear();
                    for(int i = 0; i < bytes_read; i++){
                        encodeValue = tree.EncodeValue(read_buffer[i]);
                        buffer.AppendRight(encodeValue);
                        while(buffer.getLength() >= 8){
                            write_buffer.Add(buffer.PopFirstByte());
                        }
                    }
                    target_fs.Write(write_buffer.ToArray(), 0, write_buffer.Count);
                }
                if(!buffer.IsEmpty()){
                    buffer.fillToByte();
                    write_buffer.Add(buffer.GetFirstByte());
                }
                target_fs.Write(write_buffer.ToArray(), 0, write_buffer.Count);

            }
        }
    }
}