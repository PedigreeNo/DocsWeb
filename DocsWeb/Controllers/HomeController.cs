using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DocsWeb.Models;
using System.IO;
using HeyRed.MarkdownSharp;

namespace DocsWeb.Controllers
{
    public class HomeController : Controller
    {
        private const string RootPath = "C:\\Sources\\docs\\src\\Docs\\docroot";
        private const string Extension = ".md";
        private const string IndexFileName = "\\index.md";
        private const string Secure = "/../";

        public IActionResult Index(string path)
        {
            var page = new Page();

            if (path == null)
            {
                page.Content = System.IO.File.Exists(RootPath + IndexFileName) ? ParseMarkdownFile(RootPath + IndexFileName) : "";

                page.Title = "";
                page.Folder = "";
            }
            else if (path.Contains(Secure))
            {
                path = "";
            }
            else
            {
                var absolutePath = GetAbsoluteFilePath(path);
                if (Directory.Exists(absolutePath))
                {
                    if (System.IO.File.Exists(absolutePath + IndexFileName))
                        page.Content = ParseMarkdownFile(absolutePath + IndexFileName);
                }
                else
                {
                    absolutePath = absolutePath + Extension;

                    if (System.IO.File.Exists(absolutePath))
                        page.Content = ParseMarkdownFile(absolutePath);
                    else
                        return NotFound();
                }
            }

            page.TreeViewItems = GetTreeViewItems(RootPath, "*" + Extension, "?path=" + path);

            return View(page);
        }

        public IActionResult Error()
        {
            return View();
        }

        public static List<TreeViewItem> GetTreeViewItems(string path, string searchPattern, string currentPath, string parentHref = "?path=")
        {
            var list = new List<TreeViewItem>();

            foreach (var p in Directory.GetDirectories(path))
            {
                var name = Path.GetFileName(p);
                var item = new TreeViewItem
                {
                    Icon = "fa fa-fw fa-folder",
                    SelectedIcon = "fa fa-fw fa-folder-open",
                    Href = parentHref + "/" + name
                };

                if(item.Href == currentPath)
                    item.State.Selected = true;

                if (currentPath.StartsWith(item.Href))
                    item.State.Expanded = true;

                item.Text = System.IO.File.Exists(p + "\\.meta") ? System.IO.File.ReadAllText(p + "\\.meta") : name;

                var subItems = GetTreeViewItems(p, searchPattern, currentPath, item.Href);
                if (subItems.Count > 0)
                    item.Nodes = subItems;

                list.Add(item);
            }

            foreach (var p in Directory.GetFiles(path, searchPattern))
            {
                var name = Path.GetFileNameWithoutExtension(p);
                if (name == "index")
                    continue;

                var item = new TreeViewItem
                {
                    Icon = "fa fa-fw fa-file-o",
                    Href = parentHref + "/" + name
                };

                if (item.Href == currentPath)
                    item.State.Selected = true;

                item.Text = System.IO.File.Exists(p + ".meta") ? System.IO.File.ReadAllText(p + ".meta") : name;

                list.Add(item);
            }

            return list;
        }

        private static string GetAbsoluteFilePath(string relativeFilePath)
        {
            return RootPath + relativeFilePath.Replace('/', '\\');
        }

        private static string ParseMarkdownFile(string absoluteFilePath)
        {
            return System.IO.File.Exists(absoluteFilePath) ? new Markdown().Transform(System.IO.File.ReadAllText(absoluteFilePath)) : "";
        }
    }
}


