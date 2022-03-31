﻿using System.IO;

namespace BowieD.Unturned.NPCMaker.Common.Utility
{
    public static class PathUtility
    {
        public static bool IsUnturnedPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            return File.Exists(Path.Combine(path, "Unturned.exe"));
        }
        public static bool IsUnturnedWorkshopPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            return Directory.Exists(path) && path.Contains("304930") && path.Contains("workshop") && path.Contains("content");
        }
        public static string GetUnturnedWorkshopPathFromUnturnedPath(string unturnedPath)
        {
            DirectoryInfo dirInfo = Directory.GetParent(Directory.GetParent(unturnedPath).FullName);
            DirectoryInfo workshop = new DirectoryInfo(Path.Combine(dirInfo.FullName, "workshop", "content", "304930"));
            return workshop.FullName;
        }
    }
}
