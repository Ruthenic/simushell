using System;
using System.IO;
using System.Linq;

namespace SimuShell
{
    class Program
    {
        public static string currentdir = "/";
        
        
        static void Interpret()
        {
            string currentcommand;
            
            //ask for command
            Console.Write(currentdir);
            Console.Write(">");
            currentcommand = (Console.ReadLine());
            CommandExec(currentcommand);
                
        }

        public static void PrintValues(String[] myArr)
        {
            foreach (String i in myArr)
            {
                Console.WriteLine(i);
            }
            //code from https://docs.microsoft.com/en-us/dotnet/api/system.array?view=netcore-3.1 and edited to handle strings and some other things by me
        }
        public static void CommandExec(String currentcommand)
        {
            if (currentcommand == "dir")
            {
                Console.WriteLine(currentdir);
            }
            if (currentcommand == "list" || currentcommand == "ls")
            {
                string[] filesindir = Directory.GetFiles(currentdir);
                string[] foldsindir = Directory.GetDirectories(currentdir);
                Console.WriteLine(currentdir, "contains:");
                foreach (string name in foldsindir)
                {
                    Console.WriteLine(name, " ");
                }
                foreach (string name in filesindir)
                {
                    Console.WriteLine(name, " ");
                }
            }
                if (currentcommand.Contains("cd"))
            {
                string oldpath = currentdir;
                string newpath = currentcommand.Replace("cd ", "");
                if (newpath.Contains(".."))
                {
                    int index = currentdir.LastIndexOf("/");
                    if (index > 0)
                        newpath = currentdir.Substring(0, index);
                    if (newpath == "")
                        newpath = "/";
                }
                else
                {
                    string[] folders = Directory.GetDirectories(currentdir);
                    if (currentdir == "/" && Array.Exists(folders, element => element.Contains(newpath)))
                    {
                        newpath = currentdir + newpath;
                    }
                    else
                    {
                        if (Array.Exists(folders, element => element.Contains(newpath)))
                        {
                        newpath = currentdir + "/" + newpath;
                        }
                        else
                        {
                            Console.WriteLine("Not a valid directory; Fallback to default");
                            currentdir = oldpath;
                        }
                    }

                }
                currentdir = newpath;
                if (!currentdir.Contains("/")){
                
                    currentdir = "/";
                }   
                if (currentdir == "..")
                {
                    currentdir = "/";
                }
            }
            if (currentcommand == "man")
            {
                string[] man = new string[6]; //index must be the amount of commands in the array/man page
                man[0] = "cd; change directory (append '..' to go back a directory)";
                man[1] = "ls or list; list all files and folders in directory";
                man[2] = "dir; list current directory";
                man[3] = "man; prints this";
                man[4] = "exit; closes SimuShell";
                man[5] = "echo; prints whatever is behind it";
                PrintValues(man);

            }
            if (currentcommand == "exit")
            {
                System.Environment.Exit(1);
            }
            if (currentcommand.Contains("echo"))
            {
                string to_print = currentcommand.Replace("echo ", "");
                Console.WriteLine(to_print);
            }
            Interpret();
        }
        static void Main(){
            Console.WriteLine("Welcome to SimuShell. Type 'man' for manual.");
            Interpret();
        }

    }
}