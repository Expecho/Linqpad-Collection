<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

BlockingCollection<int> bc = new BlockingCollection<int>(2);

async Task Main()
{
	Task.Run(() => ReadCollection());
	
	bc.Add(5);
	"Added 5".Dump();
	bc.Add(7);
	"Added 7".Dump();
	bc.Add(6);
	"Added 6".Dump();
	bc.Add(8);
	"Added 8".Dump(); // blocks adding items until count <= 2, prevents consumers of lagging behind too much

	bc.CompleteAdding();
}

async Task ReadCollection()
{
	foreach (var item in bc.GetConsumingEnumerable())
	{
		await Task.Delay(TimeSpan.FromSeconds(2));
		$"processed {item}".Dump();
	}
}