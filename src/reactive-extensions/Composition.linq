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
</Query>

void Main()
{
	Directory.CreateDirectory(@"D:\Temp\RxDemo");
	
	var fsw = new FileSystemWatcher();
	fsw.Path = @"D:\Temp\RxDemo";
	fsw.EnableRaisingEvents = true;
	
	var createdEvents = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                h => fsw.Created += h,
                h => fsw.Created -= h);
				
	var deletedEvents = Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                h => fsw.Deleted += h,
                h => fsw.Deleted -= h);		

	var subscription = createdEvents.Merge(deletedEvents)
		// Insert code here
		.Subscribe(change => change.Dump(), exception =>
		{
			exception.Dump();
		});
	
	Console.ReadLine();
	
	subscription.Dispose();
}