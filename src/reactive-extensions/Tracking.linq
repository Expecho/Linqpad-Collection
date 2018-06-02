<Query Kind="Program">
  <NuGetReference>Rx-Main</NuGetReference>
  <NuGetReference>System.Reactive</NuGetReference>
  <Namespace>System</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Reactive.Joins</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Reactive.PlatformServices</Namespace>
  <Namespace>System.Reactive.Subjects</Namespace>
  <Namespace>System.Reactive.Threading.Tasks</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	var logEntryProvider = new Subject<LogEntry>();

	var subscription = logEntryProvider
	.TimeInterval()
	.Timestamp()
	.Subscribe(entry => 
		{
			string.Format("this entry is received at {0}, and was {1} ms behind the last entry",
				entry.Timestamp.ToString(),
				entry.Value.Interval.TotalMilliseconds
			).Dump();
		}
	);
	
	for(int i = 0; i <= 5; ++i)
	{
		await Task.Delay(new Random().Next(10, 500));
		logEntryProvider.OnNext(new LogEntry(i, ""));
	}
		
	Console.ReadLine();
	
	subscription.Dispose();
}

public class LogEntry
{
	public long EventId { get;set;}
	public string Message { get;set;}
	
	public LogEntry(long eventId, string message)
	{
		EventId = eventId;
		Message = message;
	}
}