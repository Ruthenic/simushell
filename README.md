# simushell
Simulation of a Linux-like shell for Linux with filesystem access

# Starting the program
~~Run by typing 'dotnet run' in CLI in SimuShell/SimuShell, Run releases by typing 'dotnet SimuShell.dll' 'mono SimuShell.dll' or 'mono Program.exe' (Warning: Program.exe may not be up to date, and only works with mono by my testing)~~
~~Updated builds can be found under ruthenic/SimuShell/SimuShell/Program.exe (mono Program.exe to run, likely out-of-date) and ruthenic/SimuShell/SimuShell/bin/debug/netcoreapp3.1/SimuShell.dll (run with dotnet SimuShell.dll in dir or mono SimuShell.dll)~~
You have to run the program (for the time being) with either `dotnet run` in the source directory or run a release, with all files around it, by running `dotnet SimuShell.dll` or `mono SimuShell.dll`.

# Windows?
No.
Due to the difference in directory structures, it would be tough to focus on supporting to operating systems at the same time, so we only support Linux for the time being.

# Included Libraries
SUCC by JimmyCushnie: https://github.com/JimmyCushnie/SUCC  

# Trello
https://trello.com/b/gBrpqk9M/simushell

# Website
We now have a completely unfinished website at https://ruthenic.github.io/simushell/ ! 

# Additional Notes
1. ~~The config file (Config.succ) may remain in a git commit. If it is, I reccomend deleting it before running the program from `/SimuShell/bin/debug/netcoreapp3.1/`.~~ Leaving this here for archival and if .gitignore screws up, the Config.succ file should no longer be there after commits
