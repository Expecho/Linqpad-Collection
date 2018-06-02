<Query Kind="Program">
  <Connection>
    <ID>2bc315e8-ad77-49a5-ae68-952a9d8cad14</ID>
    <Persist>true</Persist>
    <Server>.\sql2012</Server>
    <Database>Develop_SE</Database>
  </Connection>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Net</Namespace>
</Query>

async Task Main()
{
	ReadDataUsingBeginEnd();
	
	await ReadDataUsingTaskAsync();
	
	DownloadStringUsingEvent();
	
	await DownloadStringUsingTaskAsync();
	
	Console.ReadLine();
}

// Old fashioned way using IAsyncResult
public void ReadDataUsingBeginEnd()
{
	// cannot call using(..) here
	var connection = new SqlConnection(this.Connection.ConnectionString);
	var command = new SqlCommand("SELECT TOP 10 * FROM [core].[application_setting]", connection);
	connection.Open();
	
	var callback = new AsyncCallback(DataRead);
	command.BeginExecuteReader(callback, command);
}

public void DataRead(IAsyncResult result)
{
	var command = (SqlCommand)result.AsyncState;
	var reader = command.EndExecuteReader(result);
	
	reader.HasRows.Dump();
	
	// have to manually release resources
	command.Connection.Dispose();
}

// async / await bridge using FromAsync
public async Task ReadDataUsingTaskAsync()
{
	using(var connection = new SqlConnection(this.Connection.ConnectionString))
	using(var command = new SqlCommand("SELECT TOP 10 * FROM [core].[application_setting]", connection))
	{
		connection.Open();
	
		var datareader = await Task<SqlDataReader>.Factory.FromAsync(
                    command.BeginExecuteReader(CommandBehavior.CloseConnection),
                    command.EndExecuteReader);
					
		datareader.HasRows.Dump();
	}		
}

// Get result using event
public void DownloadStringUsingEvent()
{
	var wc = new WebClient();
	wc.DownloadStringCompleted += (s,a) => { a.Result.Substring(0, 10).Dump(); };
	wc.DownloadStringAsync(new Uri("http://www.nu.nl"));
}

// async / await bridge using TaskCompletionSource
public async Task DownloadStringUsingTaskAsync()
{
	var tcs = new TaskCompletionSource<String>();
	
	var wc = new WebClient();
	wc.DownloadStringCompleted += (s,a) => { tcs.SetResult(a.Result.Substring(0, 10)); };
	wc.DownloadStringAsync(new Uri("http://www.nu.nl"));
	
	var ct = await tcs.Task;
	
	ct.Dump();
}