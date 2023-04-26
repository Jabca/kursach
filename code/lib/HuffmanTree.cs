using NodeNameSpace;
using BitStringNameSpace;

namespace HuffmanTreeNameSpace{
    public class HuffmanTree{
        private HuffmanNode root_node;
        private Dictionary<byte, BitString> encodeDictionary; 
        private Dictionary<BitString, byte> decodeDictionary;

        private HuffmanNode? cur_node = null;



        private void recursiveDFS(HuffmanNode current_node, BitString current_addres){
            if(current_node.GetData() is null){
                recursiveDFS(current_node.GetLeftChild(), current_addres.Append(0));
                recursiveDFS(current_node.GetRightChild(), current_addres.Append(1));
            }
            else{
                byte node_data = Convert.ToByte(cur_node.GetData());
                encodeDictionary.Add(node_data, current_addres);
                decodeDictionary.Add(current_addres, node_data); 
            }
        }
        public HuffmanTree(List<KeyValuePair<byte, int>> frequencies){
            var NodesQueue = new PriorityQueue<HuffmanNode, int>();
            HuffmanNode tmpNode1, tmpNode2, tmpRoot;
            foreach(KeyValuePair<byte, int> nodeData in frequencies){
                tmpNode1 = new HuffmanNode(nodeData.Key, nodeData.Value);
                NodesQueue.Enqueue(tmpNode1, tmpNode1.GetWeight());
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

            recursiveDFS(root_node, new BitString());

            cur_node = root_node;
        }

        public BitString EncodeBlock(byte arg){
            if(encodeDictionary.TryGetValue(arg, out BitString return_value)){
                return return_value;
            }
            else{
                throw new KeyNotFoundException();
            }
        }

        public byte DecodeValue(BitString arg){
            if(decodeDictionary.TryGetValue(arg, out byte return_value)){
                return return_value;
            }
            else{
                throw new KeyNotFoundException();
            }
        }

        public void StartLiveDecoding(){
            HuffmanNode cur_node = root_node;
        }

        public byte? LiveDecoding(bool arg){
            if(arg == true){
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