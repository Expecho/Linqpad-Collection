<Query Kind="Program">
  <NuGetReference>Rx-Main</NuGetReference>
  <Namespace>System</Namespace>
  <Namespace>System.Reactive</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Reactive.Joins</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Reactive.PlatformServices</Namespace>
  <Namespace>System.Reactive.Subjects</Namespace>
  <Namespace>System.Reactive.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var logEntryProvider = new Subject<LogEntry>();

	var bufferTime = 2;
	var alertCountTreshold = 2;
	
	var subscription = logEntryProvider
	.GroupBy(i => i.EventId)
	.Subscribe(eg => 
		{
			eg.Buffer(TimeSpan.FromSeconds(bufferTime))
			.Where(b => b.Count >= alertCountTreshold)
			.Subscribe(ev =>
			{
				Console.WriteLine("Event with Id {0} occured {1} times the past {2} seconds which exceeds the treshold of {3}", ev.First().EventId, ev.Count, bufferTime, alertCountTreshold);
			});
		}
	);
	
	logEntryProvider.OnNext(new LogEntry(1, "A"));
	logEntryProvider.OnNext(new LogEntry(1, "A"));
	logEntryProvider.OnNext(new LogEntry(2, "A"));
	logEntryProvider.OnNext(new LogEntry(1, "A"));
		
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