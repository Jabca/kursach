using NodeNameSpace;
using BitStringNameSpace;

namespace HuffmanTreeNameSpace{
    public class HuffmanTree{
        private HuffmanNode root_node;
        private HuffmanNode cur_node;

        public HuffmanTree(Dictionary<byte, int> frequencies){
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
            cur_node = root_node;
        }

        public byte? LiveDecoding(int arg){
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