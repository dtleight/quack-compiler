open System
open System.Diagnostics
open System.Threading.Tasks
open System

type ProcessResult = { 
    ExitCode : int; 
    StdOut : string; 
    StdErr : string 
}

let executeProcess (processName: string) (processArgs: string) =
    let psi = new Diagnostics.ProcessStartInfo(processName, processArgs) 
    psi.UseShellExecute <- false
    psi.RedirectStandardOutput <- true
    psi.RedirectStandardError <- true
    psi.CreateNoWindow <- true        
    let proc = Diagnostics.Process.Start(psi) 
    let output = new Text.StringBuilder()
    let error = new Text.StringBuilder()
    proc.OutputDataReceived.Add(fun args -> output.Append(args.Data)|> ignore; output.Append("\n") |> ignore)
    proc.ErrorDataReceived.Add(fun args -> error.Append(args.Data)|> ignore; error.Append("\n")  |> ignore)
    proc.BeginErrorReadLine()
    proc.BeginOutputReadLine()
    proc.WaitForExit()
    { ExitCode = proc.ExitCode; StdOut = output.ToString(); StdErr = error.ToString() }

let args = System.Environment.GetCommandLineArgs()
// Invocation sample
if args.Length >= 1 then
  //Get output file name
  let fileName = args.[1]
  let newName = fileName.Replace(".quack", ".ll")
  let outFile = newName.Split("/").[1]
  
  //Run relevant scripts
  executeProcess "python" "duck.py" |> ignore
  //Run compiler
  let result = executeProcess "./quackmain.exe" (fileName.ToString())
  printfn "%s" result.StdOut
  //Run compiler output
  let result2 = executeProcess "clang" (sprintf "Quack-Programs/Outputs/%s Quack-Programs/Outputs/cheats.o" outFile );
  if result2.ExitCode = 0 then 
    printfn "--------------------"
    printfn "Program Output"
    printfn "--------------------"
    let result3 = executeProcess "./a.out" "";
    printfn "%s" result3.StdOut
    //Remove the generated file after successful run
    executeProcess "rm" "a.out" |> ignore
  else
    printfn "Compilation Failed"

