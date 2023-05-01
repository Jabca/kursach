using BitStringNameSpace;

namespace NodeNameSpace{
    public class HuffmanRootNode{

    }
    public class HuffmanNode{
        private int weight;
        byte? data;
        private HuffmanNode? root, left_child, right_child;

        public HuffmanNode(byte? node_data, int node_weight){
            data = node_data;
            weight = node_weight;

            root = null;
            left_child = null;
            right_child = null;
        }

        public void AssignRoot(HuffmanNode root_node){
            root = root_node;
        }

        public void AssignChildren(HuffmanNode left_node, HuffmanNode right_node){
            left_child = left_node;
            right_child = right_node;
        }

        public int GetWeight(){return weight;}
        public byte? GetData(){return data;}

        public HuffmanNode GetLeftChild(){
            if(left_child is null){
                throw new NullReferenceException();
            }
            return left_child;
            
        }

        public HuffmanNode GetRightChild(){
            if(right_child is null){
                throw new NullReferenceException();
            }
            return right_child;
        }

        public HuffmanNode GetRoot(){
            if(root is null){
                throw new NullReferenceException();
            }
            return root;
        }


    }
}