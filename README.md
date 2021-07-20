# QCluster
QCluster started as an though experiment, **'how can I create a microservice framework for running long running tasks?'**.

Ideally one should only need to implement a interface or inherit a class in order to integrate into the cluster. All other necessary steps should follow traditional patterns around `IHost`, `IServiceCollection`.

## Nodes
A node in QCluster is simply put any running instance of QCluster
### Example
```
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddQCluster(options => 
    {
        options.MaxTaskCount = 10;
    });
    ...
}
```
## Streams
Streams provides an abstraction for any pipeline carrying instructions for the cluster and consists of four main interfaces.
- `IStreamAdapter` - Provides an adapter between QCluster and a stream.
- `IStreamInstruction` - Provides the contract for instrucions to be carried out.
- `IStreamProvider` - Provides instructions from stream to QCluster.
- `IStreamListener<T>` - Provides the contract for services listening to instructions of type T
### Example
```
public class MyIndexingService : IStreamListener<IndexInstruction>
{
    public async Task OnSubscribeAsync(IndexInstruction instruction)
    {
        await instruction.Instruct();
    }
}
```