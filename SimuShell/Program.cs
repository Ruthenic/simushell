using System;
using System.IO;

namespace SimuShell
{
    class Program
    {
        static void Main()
        {
            //init all var
            string currentdir = "/";
            string currentcommand;
            //manual (yes this is inconvinent i know this is a work in progress)
            string [] man = new string[6]; //index must be the amount of commands in the array/man page
            man[0] = "cd; change directory (append '..' to go back a directory)";
            man[1] = "ls or list; list all files and folders in directory";
            man[2] = "dir; list current directory";
            man[3] = "man; prints this";
            man[4] = "exit; closes SimuShell";
            man[5] = "echo; prints whatever is behind it";
            //finish manual
            Console.WriteLine("Welcome to SimuShell. Type 'man' for manual.");
            for (int i = 0; i < 5;)
            {


                //ask for command
                Console.Write(currentdir);
                Console.Write(">");
                currentcommand = (Console.ReadLine());

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
                        if (currentdir == "/")
                        {
                            newpath = currentdir + newpath;
                        }
                        else
                        {
                            newpath = currentdir + "/" + newpath;
                        }

                    }
                    currentdir = newpath;
                    if (!currentdir.Contains("/")){
                    
                        currentdir = "/";
                    }    
                }
                if (currentcommand == "man")
                {
                    //Console.WriteLine(manual);
                    PrintValues(man);
                    
                }
                if (currentcommand == "exit")
                {
                    break;
                }
                if (currentcommand.Contains("echo"))
                {
                    string to_print = currentcommand.Replace("echo ", "");
                    Console.WriteLine(to_print);
                }

            }
        }

        public static void PrintValues(String[] myArr)
        {
            foreach (String i in myArr)
            {
                Console.WriteLine(i);
            }
            //Console.WriteLine();
            //code from https://docs.microsoft.com/en-us/dotnet/api/system.array?view=netcore-3.1 and edited to handle strings and some other things by me
        }

    }
}