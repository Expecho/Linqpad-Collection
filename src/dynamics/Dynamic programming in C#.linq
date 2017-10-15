<Query Kind="Program">
  <Namespace>System.Dynamic</Namespace>
</Query>

void Main()
{
	dynamic myDic = new MyDictionary();
	myDic.OneItem = DateTime.Now;
	myDic.AnotherItem = 8; // vs normalDic["AnotherItem"] = 8;

	Console.WriteLine("myDic contains {0} items", myDic.Count); // Count is accessible using intellisense
	Console.WriteLine("myDic first item: {0}", myDic.AnotherItem); // AnotherItem is not accessible using intellisense
	// Console.WriteLine("myDic first item: {0}", myDic.IDoNotExist); --> will throw exception

	var anonymousType = ReturnAnonymousType();
	// No intellisense here, so accessing anonymousType.Id should be covered by unit tests
	
	var dynamicType = ReturnTrueDynamicType();
	// No intellisense either but...
	Console.WriteLine(String.Format("result with id {0} has {1} items", dynamicType.Id, dynamicType.Items.Count));
	
	//.. we can create member on the fly. We cannot do that on anonymousType since it is not dynamic from origin (its anonymous)
	dynamicType.GoodbyeMessage = "So Long, and Thanks for All the Fish (Press [ENTER] key to exit.";
	dynamicType.AskForEnterKey = new Action(() => Console.ReadLine());
    
	Console.WriteLine(dynamicType.GoodbyeMessage);
	
	dynamicType.AskForEnterKey();
}

private dynamic ReturnTrueDynamicType()
{
	dynamic result = new ExpandoObject();
	
	result.Id = 1;
	result.Items = new List<int> { 1, 2, 3 }; 
	
	// No intellisense here;		
	
	return result;
}

private dynamic ReturnAnonymousType()
{
	var customObject = new
		{
			Id = 8,
			Items = new List<int> { 1, 2, 3 }
		};
		
	// Here we have intellisense, like customObject.Id;		
	return customObject;	
}

class MyDictionary : DynamicObject
{
	private Dictionary<String, Object> dic = new Dictionary<String, Object>();

	public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        string name = binder.Name.ToLower();

        return dic.TryGetValue(name, out result);
    }

    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        dic[binder.Name.ToLower()] = value;

        return true;
    }

	public int Count
	{
		get { return dic.Count; }
	}
}

