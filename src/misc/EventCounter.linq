<Query Kind="Program">
  <Output>DataGrids</Output>
  <NuGetReference>System.Diagnostics.Tracing</NuGetReference>
  <Namespace>System.Diagnostics.Tracing</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

// https://github.com/dotnet/corefx/blob/master/src/System.Diagnostics.Tracing/documentation/EventCounterTutorial.md

async Task Main()
{
	var options = new Dictionary<string, string>();
	options.Add("EventCounterIntervalSec", "1");

	using (var listener = new EventCounterListener())
	{
		listener.EnableEvents(ServiceEventSource.Log, EventLevel.LogAlways, EventKeywords.All, options);
		
		ServiceEventSource.Log.Counter(1);
		ServiceEventSource.Log.Counter(2);
		ServiceEventSource.Log.Counter(5);
		
		await listener.FirstEventWritten;
	}
}

public class EventCounterListener : EventListener
{
	private TaskCompletionSource<EventWrittenEventArgs> tcs = new TaskCompletionSource<System.Diagnostics.Tracing.EventWrittenEventArgs>();
	
	public Task FirstEventWritten => tcs.Task;
	
	protected override void OnEventWritten(EventWrittenEventArgs eventData)
	{
		IEnumerable<string> parts;
		if (eventData.EventName != "EventCounters")
		{
			parts = Enumerable.Range(0, eventData.Payload.Count).Select(i =>
				$"{eventData.PayloadNames[i]}: {eventData.Payload[i]}");
		}
		else
		{
			var eventCounterPayload = (IDictionary<string, object>)eventData.Payload[0];
			parts = eventCounterPayload.Select(data =>	$"{data.Key}: {data.Value}");
		}
		
		string.Join(", ", parts).Dump();
		
		tcs.TrySetResult(eventData);
	}
}

[EventSource(Name = "Demo-EventSource")]
public class ServiceEventSource : EventSource
{
	public static ServiceEventSource Log = new ServiceEventSource();
	private EventCounter counter;

	private ServiceEventSource()
	{
		counter = new EventCounter("counter", this);
	}

	public void Counter(float count)
	{
		this.counter.WriteMetric(count);
	}
}
