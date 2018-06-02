<Query Kind="Statements" />

// combine
var numbers1 = new List<int> { 1, 2, 3, 4 };
var numbers2 = new List<int> { 1, 2, 3, 4 };
numbers1.Zip(numbers2.Skip(1), (f, n) => new { One = f, Two = n}).Dump(); // 1 2, 2 3, 3 4

// added
numbers1 = new List<int> { 1, 2 };
numbers2 = new List<int> { 3, 4 };
numbers2.Except(numbers1).Dump(); // 3, 4

// removed
numbers1 = new List<int> { 1, 2, 3, 4, 5 };
numbers2 = new List<int> { 1, 2, 3, 4 };
numbers1.Except(numbers2).Dump(); // 5

// overlapping
numbers1 = new List<int> { 1, 2, 3 };
numbers2 = new List<int> { 2, 3, 4 };
numbers1.Intersect(numbers2).Dump(); // 2, 3