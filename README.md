# windows_sandbox_environment_for_untrusted_programs
A tool for creating a windows sandbox environment for excuting untrusted programs such that they do not harm a computing system

## Todo list

- [x] set up the base directory, either user sets a base directory or we use the application base directory
- [x] Reorganize arguments for the inserted program
- [x] design the Interactive UI
- [x] find a way to add permissions to the sandbox environment

# How it works
- dotnet build .\sandboxer\sandboxer.csproj --no-incremental