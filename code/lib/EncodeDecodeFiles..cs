using FileUtilsNamespace;
using HuffmanTreeNamespace;
using BitStringNamespace;
using System.Text;

namespace EncodeDecodeNamespace{
    public class FileDecoder{
        public string DecodeFile(string input_file, string output_file, int chunk_size = 1024 * 1024 * 16){
            /// create file from ssaf archive. chunk_size is size of file parts that are scanned one at a time

            // create HuffmanTree object from serialized info of bytes frequencies.
            var tree_decoder = new TreeFromArchiveConstructor(input_file);
            HuffmanTree tree = tree_decoder.GetTree();
            uint data_start = tree_decoder.GetDataStartAddress();

            byte? decode_buffer;
            // get size of original file in bytes
            long original_file_size = tree_decoder.GetOriginalFileLength();
            long bits_written = 0;
            int bytes_read; 
            BitString buffer = new BitString();

            byte[] read_buffer = new byte[chunk_size];
            List<byte> write_buffer = new List<byte>();


            // if output name is not provided (and with #) form it based on name encoded in archive 
            if(Path.GetFileName(output_file) == "#"){
                output_file = Path.GetDirectoryName(output_file) + 
                              Path.DirectorySeparatorChar +
                              tree_decoder.GetOriginalFileName();
                if(File.Exists(output_file)){
                    return "File with name '" + output_file + "' exists, provide another path for it";
                }
            }

            using(FileStream source_fs = File.OpenRead(input_file))
            using(FileStream target_fs = File.Create(output_file)){
                // place cursor at the first byte of encoded file
                source_fs.Seek(data_start, 0);
                // read chunk of file to byte array
                while((bytes_read = source_fs.Read(read_buffer, 0, chunk_size)) > 0){
                    write_buffer.Clear();
                    // iterate over bytes of read chunk
                    for(int i = 0; i < bytes_read; i++){
                        // add value of byte to Bitstring
                        buffer.AppendRight(new BitString(read_buffer[i]));
                        do{
                            // move one level down in tree structure
                            // if bit value is 1 move to right child, move to left child otherwise
                            decode_buffer = tree.LiveDecode(buffer.PopLeft());
                            // if function returns null - data node wasn't reach
                            // Every simple path from root end in data node.
                            // if function returns value - data node is reached, current position is returned to root
                            if(!(decode_buffer is null)){
                                // as data node was reached decoded byte is written to write buffer
                                write_buffer.Add(((byte)decode_buffer));
                                bits_written += 1;
                                if(bits_written >= original_file_size){
                                    // last byte of file was decoded - end decoding and write data from write_buffer
                                    target_fs.Write(write_buffer.ToArray(), 0, write_buffer.Count);
                                    write_buffer.Clear();
                                    break;
                                }
                            } 
                        }
                        while(!buffer.IsEmpty());
                    }
                    // write decoded values from read_buffer to file
                    target_fs.Write(write_buffer.ToArray(), 0, write_buffer.Count);
                }
            }
            return "Successfully created file '" + output_file + "'";
        }
    }

    public class FileEncoder{
        public string EncodeFile(string input_file, string output_file, int chunk_size=1024 * 1024 * 16){
            /// create ssaf archive from source file. chunk_size is size of file parts that are scanned one at a time

            /*
            Archive has following structure
                1. Serialized info about frequencies of every byte
                2. separator of 5 zero bytes
                3. 8 bytes as ulong describing length of original file in bytes
                4. original file name
                5. '#' symbol as separator
                6. bytes of encoded file 
            */

            // create tree object from source file
            var tree_decoder = new TreeFromSourceConstructor(input_file);
            HuffmanTree tree = tree_decoder.GetTree();
            var frequencies = tree_decoder.GetFrequencies();

            int bytes_read;
            long file_length;
            byte[] file_length_buffer;
            BitString buffer = new BitString();
            BitString encodedValue;

            byte[] read_buffer = new byte[chunk_size];
            List<byte> write_buffer = new List<byte>();

            string file_name = Path.GetFileName(input_file) + "#";
            byte[] file_name_bytes = Encoding.UTF8.GetBytes(file_name);

            // get serialized data needed to recreate tree object from (frequency of every byte occurrence)
            byte[] serialized_tree_info = tree_decoder.GetSerializedFrequencies();

            using(FileStream source_fs = File.OpenRead(input_file))
            using(FileStream target_fs = File.Create(output_file)){
                // write info for tree recreation
                target_fs.Write(serialized_tree_info, 0, serialized_tree_info.Count());

                // write as ulong length of source file in bytes
                file_length = source_fs.Length;
                file_length_buffer = BitConverter.GetBytes(file_length);
                if(BitConverter.IsLittleEndian){
                    Array.Reverse(file_length_buffer);
                }
                target_fs.Write(file_length_buffer, 0, 8);

                // write original file name
                target_fs.Write(file_name_bytes);
                
                // read chunk of file to byte array
                while((bytes_read = source_fs.Read(read_buffer, 0, chunk_size)) > 0){
                    
                    // iterate over bytes of read chunk
                    for(int i = 0; i < bytes_read; i++){
                        // encode byte and add it to bitstring
                        encodedValue = tree.EncodeValue(read_buffer[i]);
                        buffer.AppendRight(encodedValue);
                        // while possible get bytes from bitstring and add them to write buffer 
                        while(buffer.getLength() >= 8){
                            write_buffer.Add(buffer.PopFirstByte());
                        }
                    }
                    // write encoded chunk to archive
                    target_fs.Write(write_buffer.ToArray(), 0, write_buffer.Count);
                    write_buffer.Clear();
                }
                // after processing chunks data smaller then 1 byte accumulates in bitstring
                // as all chunks were encoded we need to encode this residual data as 1 byte
                if(!buffer.IsEmpty()){
                    // fill bitstring to size of byte and encode it
                    buffer.fillToByte();
                    write_buffer.Add(buffer.GetFirstByte());
                
                }
                // write last buffer to file
                target_fs.Write(write_buffer.ToArray(), 0, write_buffer.Count);
            }
            return "Successfully created file '" + output_file + "'";
        }
        
    }
}