﻿using System.IO;

namespace SaveDataRelocator2
{
    public static class DirectoryUtils {

        // https://stackoverflow.com/questions/58744/copy-the-entire-contents-of-a-directory-in-c-sharp/55981013#55981013
        public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = true) {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (dir.Exists == false) {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (Directory.Exists(destDirName) == false) {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files) {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs) {
                foreach (DirectoryInfo subdir in dirs) {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

    }
}
