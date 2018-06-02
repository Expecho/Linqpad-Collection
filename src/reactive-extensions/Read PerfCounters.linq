<Query Kind="Program">
  <NuGetReference>System.Reactive</NuGetReference>
  <NuGetReference>System.Reactive.Compatibility</NuGetReference>
  <NuGetReference>Tx.All</NuGetReference>
  <Namespace>Tx.Windows</Namespace>
</Query>

// https://github.com/Microsoft/Tx

void Main()
{
	var observable = PerfCounterObservable.FromRealTime(
		TimeSpan.FromSeconds(1), 
		new[]
	        {
	            @"\Processor(_Total)\% processor time",
	            //@"\Memory(_Total)\% Committed Bytes In Use",
	            //@"\Memory(_Total)\Available MBytes"
	        });

	using (observable.Subscribe(pc => pc.Dump()))
	{
		Console.ReadLine();
	}
}