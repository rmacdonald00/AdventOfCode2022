using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2022.DaySolutions
{
    class Day7 : DaySolver
    {
        public Day7(string input) : base(input)
        {
        }

        public override string GetPart1Solution()
        {
            var root = BuildRoot();
            var allDirs = root.getAllDirectories();
            var totalOfAllLessLike10k = allDirs.Select(c => c.getSize()).Where(x => x < 100000).Sum();
            return totalOfAllLessLike10k.ToString();
        }

        public override string GetPart2Solution()
        {
            var root = BuildRoot();
            var allDirs = root.getAllDirectories();
            var minAmount = 30000000 - (70000000 - root.getSize());
            var sizeOfDirToDelete = allDirs.Select(x => x.getSize()).Where(y => y > minAmount).OrderBy(s => s).ToList()[0];
            return sizeOfDirToDelete.ToString();
        }

        private DirectoryClass BuildRoot()
        {
            var root = new DirectoryClass("root", null);

            var commands = new List<string>(_rawInput.Split("\r\n$ "));
            commands.RemoveAt(0);

            var currentDirectory = root;
            foreach(var commandString in commands)
            {
                var lines = commandString.Split("\r\n");

                var commandText = lines[0].Split(" ")[0];

                if(commandText == "cd")
                {
                    currentDirectory = currentDirectory.getDirectory(lines[0].Split(" ")[1]);
                } else if(commandText == "ls")
                {
                    for(var i = 1; i < lines.Length; i++)
                    {
                        var fileData = lines[i].Split(" ");
                        if (fileData[0] == "dir")
                        {
                            currentDirectory.AddFileObject(new DirectoryClass(fileData[1], currentDirectory));
                        }
                        else
                        {
                            currentDirectory.AddFileObject(new FileClass(int.Parse(fileData[0]), fileData[1]));
                        }
                    }
                }
            }

            return root;
        }

        private interface FileObject
        {
            public int getSize();

            public string getName();
        }

        private class DirectoryClass : FileObject
        {
            private readonly string _name;
            private List<FileObject> _files;
            private DirectoryClass _parent;

            public DirectoryClass(string name, DirectoryClass parent)
            {
                _name = name;
                _parent = parent;
                _files = new List<FileObject>();
            }

            public int getSize()
            {
                return _files.Sum(x => x.getSize());
            }

            public string getName()
            {
                return _name;
            }

            public DirectoryClass getDirectory(string name)
            {
                if(name == "..")
                {
                    return _parent;
                } else
                {
                    return (DirectoryClass) _files.FirstOrDefault(x => x.getName() == name);
                }
            }

            public List<DirectoryClass> getAllDirectories()
            {
                var allDirectories = _files.Where(x => typeof(DirectoryClass) == x.GetType()).Select(x => (DirectoryClass)x).ToList();
                var innerDirecs = new List<DirectoryClass>();
                foreach(var direc in allDirectories)
                {
                    innerDirecs.AddRange(direc.getAllDirectories());
                }
                allDirectories.AddRange(innerDirecs);
                return allDirectories;
            }

            public void AddFileObject(FileObject fileObject)
            {
                _files.Add(fileObject);
            }

        }

        private class FileClass : FileObject
        {
            private readonly int _size;
            private readonly string _name;
            public FileClass(int size, string name)
            {
                _name = name;
                _size = size;
            }

            public int getSize()
            {
                return _size;
            }

            public string getName()
            {
                return _name;
            }
        }

    }
}
