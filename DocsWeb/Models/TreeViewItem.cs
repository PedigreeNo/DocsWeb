using System.Collections.Generic;

namespace DocsWeb.Models
{
    public class TreeViewItem
    {
        public string Text { get; set; }
        public string Icon { get; set; }
        public string Href { get; set; }
        public string SelectedIcon { get; set; }
        public TreeViewItemState State { get; set; }

        public List<TreeViewItem> Nodes { get; set; }

        public TreeViewItem()
        {
            State = new TreeViewItemState();
        }
    }
}
