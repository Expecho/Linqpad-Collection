<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

BlockingCollection<int> bc = new BlockingCollection<int>();

async Task Main()
{
	Task.Run(() => ReadCollection1());
	Task.Run(() => ReadCollection2());
	
	bc.Add(5);
	
	await Task.Delay(TimeSpan.FromSeconds(2));
	
	bc.Add(6);
	bc.Add(7);
	bc.Add(8);
	bc.Add(9);
	
	bc.CompleteAdding();
}

void ReadCollection1()
{
	foreach (var item in bc.GetConsumingEnumerable()) 
	{
		$"1 processed {item}".Dump();
	}
}


void ReadCollection2()
{
	foreach (var item in bc.GetConsumingEnumerable()) 
	{
		$"2 processed {item}".Dump();
	}
}


