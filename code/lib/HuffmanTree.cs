using NodeNameSpace;
using BitStringNameSpace;

namespace HuffmanTree{
    public class HuffmanTree{
        private List<KeyValuePair<BitString, int>> frequencies;
        private HuffmanNode root_node;
        private Dictionary<BitString, BitString> encodeDictionary, decodeDictionary;

        private void recursiveDFS(HuffmanNode current_node, BitString current_addres){
            if(current_node.GetData() is null){
                recursiveDFS(current_node.GetLeftChild(), current_addres.Append(0));
                recursiveDFS(current_node.GetRightChild(), current_addres.Append(1));
            }
            else{
                encodeDictionary.Add(current_node.GetData(), current_addres);
                decodeDictionary.Add(current_addres, current_node.GetData()); 
            }
        }
        public HuffmanTree(List<KeyValuePair<BitString, int>> frequency_list){
            frequencies = frequency_list;
            var NodesQueue = new PriorityQueue<HuffmanNode, int>();
            HuffmanNode tmpNode1, tmpNode2, tmpRoot;
            foreach(KeyValuePair<BitString, int> nodeData in frequencies){
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
        }

        public BitString EncodeBlock(BitString arg){
            if(encodeDictionary.TryGetValue(arg, out BitString return_value)){
                return return_value;
            }
            else{
                throw new KeyNotFoundException();
            }
        }

        public BitString DecodeValue(BitString arg){
            if(decodeDictionary.TryGetValue(arg, out BitString return_value)){
                return return_value;
            }
            else{
                throw new KeyNotFoundException();
            }
        }
    }
}