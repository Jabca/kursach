namespace BitStringNamespace{
    public class BitString{
        /// Data structure that allows you to operate with individual bits(up to 64 at a time).
        private int length;
        private ulong data;
        private ulong bit_pointer;

        public BitString(){
            /// init empty Bitstring
            length = 0;
            data = 0;
            bit_pointer = 1;
        }
        public BitString(byte fill){
            /// init bitstring filled with one byte of data
            length = 8;
            data = Convert.ToUInt64(fill);
            bit_pointer <<= 8;
        }
        public void AppendLeft(int bit){
            /// append one bit of data from left side of bitstring
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
            /// append one bit of data from right side of bitstring
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
            /// return leftmost bit
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

        public int PopLeft(){
            // remove and return leftmost bit 
            if(length == 0){
                throw new Exception("Bitstring is empty");
            }

            int ret_value = this.GetLeft();

            length--;
            bit_pointer >>= 1;
            data = data & (~bit_pointer);
            return ret_value;
        }

        public int GetRight(){
            /// return rightmost bit
            if(length == 0){
                throw new Exception("Bitstring is empty");
            }
            return (int)(data & 1);
        }

        public int PopRight(){
            /// remove and return rightmost bit
            if(length == 0){
                throw new Exception("Bitstring is empty");
            }

            int ret_vlue = this.GetRight();

            data >>= 1;
            length--;
            bit_pointer >>= 1;
            return ret_vlue;
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
            /// whether contains any data
            return (length == 0);
        }

        public int getLength(){
            /// get number if bits in bitstring
            return length;
        }

        public ulong GetData(){
            /// get raw bitstring representation as 64 bit unsigned integer.
            return data;
        }

        public void AppendRight(BitString bs){
            /// add data from another bitstring to the right side
            int l = bs.getLength();
            bit_pointer <<= l;
            data <<= l;
            length += l;
            data += bs.GetData();
        }

        public byte GetFirstByte(){
            /// get leftmost byte from bitstring
            if(length < 8){
                throw new Exception("Bitstring is less then a byte");
            }
            return (byte)(data >> (length - 8));
        }

        public void fillToByte(){
            /// fills bitstring with zeroes from right to length of 8
            if(length >= 8){
                throw new Exception("Bitstring is alredy minimum 1 byte long");
            }
            bit_pointer <<= (8 - length);
            data <<= (8 - length);
            length = 8;
        }

        public byte PopFirstByte(){
            /// remove and return leftmost byte
            if(length < 8){
                throw new Exception("Bitstring is less then a byte");
            }

            byte ret_value = this.GetFirstByte();

            data = data & (~((uint)255 << (length - 8)));
            length -= 8;
            bit_pointer >>= 8;

            return ret_value;
        }


    }
}