# QCluster
QCluster started as an though experiment, **'how can I create a microservice framework for running long running tasks?'**.

Ideally one should only need to implement a interface or inherit a class in order to integrate into the cluster. All other necessary steps should follow traditional patterns around `IHost`, `IServiceCollection`.


## Setup
1. Create a worker project, with the command below folder name will become project name.
```
dotnet new worker
```


2. AddQCluster to Program.cs, ex:

```
public class Program
{
    public static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) => 
            {
                services.AddQCluster()
                        .UseMemoryStreams();
            })
            .Build()
            .Run();
    }
}
```

3. Create listener
```
public class MyIndexingListener : IStreamListener<IndexInstruction>
{
    public async Task OnSubscribeAsync(IndexInstruction instruction)
    {
        await instruction.Instruct();
    }
}
```

4. Register listener
 WIP
