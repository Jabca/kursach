using NodeNamespace;
using BitStringNamespace;

namespace HuffmanTreeNamespace{
    /// implements huffman tree to decode and encode data
    public class HuffmanTree{
        private HuffmanNode root_node;
        private HuffmanNode cur_node;

        private byte? return_value;

        // dictionary of nodes that which contain data aka data nodes
        private Dictionary<byte, HuffmanNode> DataNodes = new Dictionary<byte, HuffmanNode>();

        public HuffmanTree(Dictionary<byte, uint> frequencies){
            /// constructs huffman tree from dictionary of bytes number of occurrences in file
            // priority queue for tree construction. It stores subtrees as there root nodes and sorts them by there weight
            var NodesQueue = new PriorityQueue<HuffmanNode, uint>();
            HuffmanNode tmpNode1, tmpNode2, tmpRoot;
            // add all data nodes to queue
            foreach(KeyValuePair<byte, uint> nodeData in frequencies){
                tmpNode1 = new HuffmanNode(nodeData.Key, nodeData.Value);
                NodesQueue.Enqueue(tmpNode1, tmpNode1.GetWeight());
                // add pointer for dictionary of data nodes 
                DataNodes[nodeData.Key] = tmpNode1;
            }
            // merge subtrees with lowest weight until we get only one. 
            // by merging trees we create new root of which roots of 2 trees become children
            // when there is only one node left we got huffman tree
            while(NodesQueue.Count > 1){
                // get from queue trees with least weights
                tmpNode1 = NodesQueue.Dequeue();
                tmpNode2 = NodesQueue.Dequeue();

                // create new root for dequeued trees
                tmpRoot = new HuffmanNode(null, tmpNode1.GetWeight() + tmpNode2.GetWeight());

                // assign new root to old root nodes
                tmpNode1.AssignRoot(ref tmpRoot);
                tmpNode2.AssignRoot(ref tmpRoot);


                // assign children for new root
                tmpRoot.AssignChildren(ref tmpNode1, ref tmpNode2);

                // add new tree to queue
                NodesQueue.Enqueue(tmpRoot, tmpRoot.GetWeight());
            }
            
            // assign global root
            root_node = NodesQueue.Dequeue();
            cur_node = root_node;
        }

        private BitString RecursiveEncode(HuffmanNode node, BitString address){
            /// recursive function to encode value by going from data value to root
            // if reached root we encoded path and got full address
            if(node == root_node){
                return address;
            }
            // if current node is right child add 1 from front of address 
            // because we are going in opposite direction of decoding
            if(node.GetRoot().GetRightChild() == node){
                address.AppendLeft(1);
            }
            // add 0 otherwise
            else{
                address.AppendLeft(0);
            }
            // recursive call from root of current node
            return RecursiveEncode(node.GetRoot(), address);
        }

        public BitString EncodeValue(byte arg){
            /// return encoded value of byte as bitstring(corresponding data node address)
            // get node with argument's data
            try{
                HuffmanNode cur_node =  DataNodes[arg];
            }
            catch(KeyNotFoundException){
                throw new KeyNotFoundException("No node with value: " + arg);
            }
            return RecursiveEncode(cur_node, new BitString());
        }
        private byte? RecursiveDecode(HuffmanNode cur_node, BitString address){
            /// Moves to child depending on argument
            /// if node contains data returns decoded byte otherwise returns null 
            if(address.IsEmpty()){
                return cur_node.GetData();
            }
            // recursive call on child node
            if(address.GetLeft() == 1){
                // remove bit from decoding value
                address.PopLeft();
                return RecursiveDecode(cur_node.GetRightChild(), address);
            }
            else{
                address.PopLeft();
                return RecursiveDecode(cur_node.GetLeftChild(), address);
            }
        }

        public byte DecodeValue(BitString address){
            /// returns decoded value of bitstring(data node address) as byte
            byte? ret = RecursiveDecode(root_node, address);
            if(ret is null){
                throw new KeyNotFoundException("No value with key " + address.ToString());
            }
            return (byte)ret;
        }

        public byte? LiveDecode(int arg){
            /// Replaces current node with it's child depending on argument
            /// if node contains data returns decoded byte otherwise returns null

            // change current node based on argument
            if(arg == 1){
                cur_node = cur_node.GetRightChild();
            }
            else{
                cur_node = cur_node.GetLeftChild();
            }

            // if node contains data return it otherwise return null
            if(cur_node.GetData() != null){
                return_value = cur_node.GetData();
                cur_node = root_node;
                return return_value;
            }
            return null;
        }
    }
}