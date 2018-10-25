//定义一个树节点
function TreeNode() {
    this.id = 0;
    this.parentId = 0;
    this.name = "";
    this.spread = false;
    this.children = new Array();
}

//定义树类型
function Tree(root) {
    this.root = root;
   
    this.getNode = function (id) {
        return GetNode(this.root, id);
    }
}

function GetNode(root, id) {
    if (root.id == id) {
        return root;
    } else {
        if (root.children != null) {
            for (var i = 0; i < root.children.length; i++) {
                var node = root.children[i];
                var findNode = GetNode(node, id);
                if (null != findNode) {
                    return findNode;
                }
            }
        }
    }
    return null;
}
