using NodeNameSpace;
using BitStringNameSpace;

namespace HuffmanTreeNameSpace{
    public class HuffmanTree{
        private HuffmanNode root_node;
        private HuffmanNode cur_node;

        private Dictionary<byte, HuffmanNode> DataNodes = new Dictionary<byte, HuffmanNode>();

        public HuffmanTree(Dictionary<byte, int> frequencies){
            var NodesQueue = new PriorityQueue<HuffmanNode, int>();
            HuffmanNode tmpNode1, tmpNode2, tmpRoot;
            foreach(KeyValuePair<byte, int> nodeData in frequencies){
                tmpNode1 = new HuffmanNode(nodeData.Key, nodeData.Value);
                NodesQueue.Enqueue(tmpNode1, tmpNode1.GetWeight());
                DataNodes[nodeData.Key] = tmpNode1;
            }

            while(NodesQueue.Count > 1){
                tmpNode1 = NodesQueue.Dequeue();
                tmpNode2 = NodesQueue.Dequeue();

                tmpRoot = new HuffmanNode(null, tmpNode1.GetWeight() + tmpNode2.GetWeight());

                tmpNode1.AssignRoot(tmpRoot);
                tmpNode2.AssignRoot(tmpRoot);

                tmpRoot.AssignChildren(tmpNode1, tmpNode2);

                NodesQueue.Enqueue(tmpRoot, tmpRoot.GetWeight());
            }

            root_node = NodesQueue.Dequeue();
            cur_node = root_node;
        }

        private BitString RecursiveEncode(HuffmanNode node, BitString address){
            if(node == root_node){
                return address;
            }
            if(node.GetRoot().GetRightChild() == node){
                address.AppendLeft(1);
            }
            else{
                address.AppendLeft(0);
            }
            return RecursiveEncode(node.GetRoot(), address);
        }

        public BitString EncodeValue(byte arg){
            HuffmanNode cur_node =  DataNodes[arg];
            return RecursiveEncode(cur_node, new BitString());
        }
        private byte? RecursiveDecode(HuffmanNode cur_node, BitString address){
            if(address.IsEmpty()){
                return cur_node.GetData();
            }
            if(address.GetLeft() == 1){
                address.DelLeft();
                return RecursiveDecode(cur_node.GetRightChild(), address);
            }
            else{
                address.DelLeft();
                return RecursiveDecode(cur_node.GetLeftChild(), address);
            }
        }

        public byte DecodeValue(BitString address){
            byte? ret = RecursiveDecode(root_node, address);
            if(ret is null){
                throw new KeyNotFoundException("No value with key " + address.ToString());
            }
            return (byte)ret;
        }

        public byte? LiveDecode(int arg){
            if(arg == 1){
                cur_node = cur_node.GetRightChild();
            }
            else{
                cur_node = cur_node.GetLeftChild();
            }

            if(cur_node.GetData() != null){
                return cur_node.GetData();
            }
            return null;
        }
    }
}