namespace BitStringNameSpace{
    public class BitString{
        private int length = 0;
        private ulong data = 0;
        private ulong bit_pointer = 1;
        public BitString Append(dynamic integer){
            length++;
            if(Convert.ToBoolean(integer)){
                data = bit_pointer | data;
            }
            bit_pointer = bit_pointer << 1;
            if(length > 64){
                throw new OverflowException("Bit pointer overflow: block is larger then 64 bit");
            }

            return this;
        }
        public string GetValue(){
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
    }
}