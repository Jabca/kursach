using BitStringNamespace;

namespace NodeNamespace{
    public class HuffmanNode{
        /// Tree node implementation for Huffman tree structure
        /// May store references to it's two children and root, byte of data and weight
        ///
        uint weight;
        byte? data;
        HuffmanNode? root, left_child, right_child;

        public HuffmanNode(byte? node_data, uint node_weight){
            /// constructs node
            data = node_data;
            weight = node_weight;

            root = null;
            left_child = null;
            right_child = null;
        }

        public void AssignRoot(ref HuffmanNode root_node){
            /// assigns root to node
            root = root_node;
        }

        public void AssignChildren(ref HuffmanNode left_node, ref HuffmanNode right_node){
            /// assigns children to node
            left_child = left_node;
            right_child = right_node;
        }

        public uint GetWeight(){return weight;}
        public byte? GetData(){return data;}

        public HuffmanNode GetLeftChild(){
            /// returns left child. If child is null throws null reference exception
            if(left_child is null){
                throw new NullReferenceException();
            }
            return left_child;
            
        }

        public HuffmanNode GetRightChild(){
            /// returns right child. If child is null throws null reference exception
            if(right_child is null){
                throw new NullReferenceException();
            }
            return right_child;
        }

        public HuffmanNode GetRoot(){
            /// returns root. If root is null throws null reference exception
            if(root is null){
                throw new NullReferenceException();
            }
            return root;
        }


    }
}