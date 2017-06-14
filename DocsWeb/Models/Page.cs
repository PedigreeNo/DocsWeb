using System.Collections.Generic;

namespace DocsWeb.Models
{
    public class Page
    {
        public string Content { get; set; }
        public string Title { get; set; }
        public string Folder { get; set; }
        public List<TreeViewItem> TreeViewItems { get; set; }
    }
}
