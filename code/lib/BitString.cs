namespace BitStringNameSpace{
    public class BitString{
        private int length = 0;
        private ulong data = 0;
        private ulong bit_pointer = 1;
        public BitString Append(bool bit){
            length++;
            if(bit){
                data = bit_pointer | data;
                bit_pointer <<= 1;
                length++;
            }            
            if(length > 64){
                throw new OverflowException("Bit pointer overflow: block is larger then 64 bit", BitString);
            }

            return this;
        }
        public string GetValue(){
            if(length == 0){
                throw new Exception("BitString is empty");
            }

            string result = "";
            ulong tmp_integer = data;
            for(int i = 0; i < length; i++){
                result += Convert.ToChar(tmp_integer & 1);
                tmp_integer = tmp_integer >> 1;
            }
            return result;
        }

        public BitString InitWithChar(char arg){
            data = Convert.ToUInt64(arg);
            length = 8;
            bit_pointer = bit_pointer << 8;
            return this;
        }

        public byte[] ToByteArray(){
            byte[] byte_array = {0};
            for(int i = 0; i < length; i++){
                if(length > 6){

                }
                else{
                    byte_array[0] = Convert.ToByte(data);
                    byte_array[0] += 0b10000000;
                }
            }
            return byte_array;
        }

        public void Extend(BitString bString){
            string value = bString.GetValue();

        }

        public BitString ToBitSting(byte[] byteArray){
            BitString newBitString = new BitString();

            return newBitString;
        }
    }
}