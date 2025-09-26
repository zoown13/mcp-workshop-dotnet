
using MyMonkeyApp;

class Program
{
    static readonly string[] asciiArts = new[]
    {
	  @"  (\__/)
  (='.'=)
  (")_(")
  -- 귀여운 토끼지만 원숭이처럼 장난꾸러기!",
	  @"   w  c( ..)o   (
    \__(-)    __)
	  /     (
	 (      )
	  `-.-'
  -- 원숭이의 하루는 모험으로 가득!",
	  @"   .-"""-.
  / .===. \
  \/ 6 6 \/
  ( \___/ )
___ooo__ooo___
  -- 원숭이 웃음은 행복의 시작!"
    };

	static void Main()
	{
		RunApp().Wait();
	}

	static async Task RunApp()
	{
		await MonkeyHelper.LoadMonkeysAsync();
		var rand = new Random();
		bool running = true;
		while (running)
		{
			Console.Clear();
			// 랜덤하게 ASCII 아트 출력
			if (rand.Next(0, 2) == 0)
			{
				Console.WriteLine(asciiArts[rand.Next(asciiArts.Length)]);
				Console.WriteLine();
			}
			Console.WriteLine("==== Monkey Console App ====");
			Console.WriteLine("1. List all monkeys");
			Console.WriteLine("2. Get details for a specific monkey by name");
			Console.WriteLine("3. Get a random monkey");
			Console.WriteLine("4. Exit app");
			Console.Write("Select an option: ");
			var input = Console.ReadLine();
			Console.WriteLine();
			switch (input)
			{
				case "1":
					ListAllMonkeys();
					break;
				case "2":
					GetMonkeyDetails();
					break;
				case "3":
					GetRandomMonkey();
					break;
				case "4":
					running = false;
					Console.WriteLine("Bye! 🐒");
					break;
				default:
					Console.WriteLine("Invalid option. Try again.");
					break;
			}
			if (running)
			{
				Console.WriteLine("\nPress Enter to continue...");
				Console.ReadLine();
			}
		}
	}

	static void ListAllMonkeys()
	{
		var monkeys = MonkeyHelper.GetMonkeys();
		Console.WriteLine("| Name               | Location                | Population |");
		Console.WriteLine("------------------------------------------------------------");
		foreach (var m in monkeys)
		{
			Console.WriteLine($"| {m.Name,-18} | {m.Location,-22} | {m.Population,9} |");
		}
	}

	static void GetMonkeyDetails()
	{
		Console.Write("Enter monkey name: ");
		var name = Console.ReadLine();
		var monkey = MonkeyHelper.GetMonkeyByName(name ?? "");
		if (monkey == null)
		{
			Console.WriteLine("Monkey not found.");
			return;
		}
		Console.WriteLine($"Name: {monkey.Name}");
		Console.WriteLine($"Location: {monkey.Location}");
		Console.WriteLine($"Population: {monkey.Population}");
		Console.WriteLine($"Details: {monkey.Details}");
		Console.WriteLine($"Image: {monkey.Image}");
		Console.WriteLine($"Coordinates: ({monkey.Latitude}, {monkey.Longitude})");
	}

	static void GetRandomMonkey()
	{
		var monkey = MonkeyHelper.GetRandomMonkey();
		if (monkey == null)
		{
			Console.WriteLine("No monkeys available.");
			return;
		}
		Console.WriteLine($"Random Monkey: {monkey.Name}");
		Console.WriteLine($"Location: {monkey.Location}");
		Console.WriteLine($"Population: {monkey.Population}");
		Console.WriteLine($"Details: {monkey.Details}");
		Console.WriteLine($"Image: {monkey.Image}");
		Console.WriteLine($"Coordinates: ({monkey.Latitude}, {monkey.Longitude})");
		Console.WriteLine($"Random pick count: {MonkeyHelper.GetRandomPickCount()}");
	}
}
