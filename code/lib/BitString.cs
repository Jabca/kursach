namespace BitStringNameSpace{
    public class BitString{
        private int length;
        private ulong data;
        private ulong bit_pointer;

        public BitString(){
            length = 0;
            data = 0;
            bit_pointer = 1;
        }
        public BitString(byte fill){
            length = 8;
            data = Convert.ToUInt64(fill);
            bit_pointer <<= 8;
        }
        public void AppendLeft(int bit){
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

        public void AppendRight(int bit){
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
            if(length == 0){
                throw new Exception("Bitstring is empty");
            }
            if((data & (bit_pointer >> 1)) == 0){
                return 0;
            }
            else{
                return 1;
            }
        }

        public void PopLeft(){
            if(length == 0){
                throw new Exception("Bitstring is empty");
            }
            length--;
            bit_pointer >>= 1;
            data = data & (~bit_pointer);
        }

        public int GetRight(){
            if(length == 0){
                throw new Exception("Bitstring is empty");
            }
            return (int)(data & 1);
        }

        public void PopRight(){
            if(length == 0){
                throw new Exception("Bitstring is empty");
            }
            data >>= 1;
            length--;
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

        public bool IsEmpty(){
            return (length == 0);
        }

        public int getLength(){
            return length;
        }

        public ulong GetData(){
            return data;
        }

        public void AppendRight(BitString bs){
            int l = bs.getLength();
            bit_pointer <<= l;
            data <<= l;
            length += l;
            data += bs.GetData();
        }

        public byte GetFirstByte(){
            if(length < 8){
                throw new Exception("Bitstring is less then a byte");
            }
            return (byte)(data >> (length - 8));
        }

        public void fillToByte(){
            if(length >= 8){
                throw new Exception("Bitstring is alredy minimum 1 byte long");
                
            }
            bit_pointer <<= (8 - length);
            data <<= (8 - length);
            length = 8;
        }

        public void PopLastByte(){
            if(length < 8){
                throw new Exception("Bitstring is less then a byte");
            }
            data >>= 8;
            length -= 8;
            bit_pointer >>= 8;
        }

        public void PopFirstByte(){
            if(length < 8){
                throw new Exception("Bitstring is less then a byte");
            }
            data = data & (~((uint)255 << (length - 8)));
            length -= 8;
            bit_pointer >>= 8;
        }


    }
}