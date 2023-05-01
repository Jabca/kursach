namespace BitStringNameSpace{
    [Serializable]
    public class BitString{
        private int length = 0;
        private ulong data = 0;
        private ulong bit_pointer = 1;
        public void AppendRight(int bit){
            if(bit == 1){
                data = bit_pointer | data;
            } 
            else{
                data = ((~bit_pointer) & data);
            }
            bit_pointer <<= 1;
            length++;
            if(length > 64){
                throw new OverflowException("Bit pointer overflow: block is larger then 64 bit");
            }
        }

        public void AppendLeft(int bit){
            data <<= 1;
            if(bit == 1){
                data = data | 1;
            }
            length++;
            bit_pointer <<= 1;
            if(length > 64){
                throw new OverflowException("Bit pointer overflow: block is larger then 64 bit");
            }
        }

        public int GetLeft(){
            if((data & (bit_pointer >> 1)) == 0){
                return 0;
            }
            else{
                return 1;
            }
        }

        public void DelLeft(){
            if(length == 0){
                throw new Exception("Bitstring is empty");
            }
            length--;
            data = data & (~bit_pointer);
            bit_pointer >>= 1;
        }
         
        public override string ToString(){
            if(length == 0){
                throw new Exception("BitString is empty");
            }
            string result = "";
            ulong tmp_integer = data;
            for(int i = 0; i < length; i++){
                if((tmp_integer & 1) == 1){
                    result = '1' + result;
                }
                else{
                    result = '0' + result;
                }
                
                tmp_integer = tmp_integer >> 1;
            }
            return result;
        }

        public BitString InitWithChar(char arg){
            data = Convert.ToByte(arg);
            length = 8;
            bit_pointer = bit_pointer << 8;
            return this;
        }

        public bool IsEmpty(){
            return (length == 0);
        }


    }
}