namespace lab3DirectoryScanner.DirTreeManager
{
    public interface ITreeManager
    {
        public void AddChildTo(TreeNode parent, TreeNode child);
        public TreeNode Head();

        public void CalcSize();

        public void PrintToConsole();
    }
}