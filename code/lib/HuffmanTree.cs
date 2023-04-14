using Node;

namespace HuffmanTree{
    public class HuffmanTree{
        private List<KeyValuePair<byte[], int>> frequencies;
        private HuffmanNode root_node;
        public HuffmanTree(List<KeyValuePair<byte[], int>> frequency_list){
            frequencies = frequency_list;
            var NodesQueue = new PriorityQueue<HuffmanNode, int>();
            HuffmanNode tmpNode1, tmpNode2, tmpRoot;
            foreach(KeyValuePair<byte[], int> nodeData in frequencies){
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
        }
    }
}