using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyMonkeyApp;

/// <summary>
/// Monkey 데이터 관리를 위한 static helper 클래스
/// </summary>
public static class MonkeyHelper
{
    private static List<Monkey> monkeys = new();
    private static int randomPickCount = 0;

    /// <summary>
    /// MCP 서버에서 원숭이 데이터를 비동기로 로드합니다.
    /// </summary>
    public static async Task LoadMonkeysAsync()
    {
        // MCP 서버에서 데이터를 받아오는 부분은 실제 API 연동 시 구현
        // 예시: monkeys = await MonkeyMcpClient.GetMonkeysAsync();
        // 현재는 샘플 데이터로 대체
        monkeys = new List<Monkey>
        {
            new Monkey { Name = "Baboon", Location = "Africa & Asia", Details = "Baboons are African and Arabian Old World monkeys belonging to the genus Papio, part of the subfamily Cercopithecinae.", Image = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/baboon.jpg", Population = 10000, Latitude = -8.783195, Longitude = 34.508523 },
            new Monkey { Name = "Capuchin Monkey", Location = "Central & South America", Details = "The capuchin monkeys are New World monkeys of the subfamily Cebinae.", Image = "https://raw.githubusercontent.com/jamesmontemagno/app-monkeys/master/capuchin.jpg", Population = 23000, Latitude = 12.769013, Longitude = -85.602364 },
            // ... 나머지 원숭이 데이터 추가 ...
        };
    }

    /// <summary>
    /// 모든 원숭이 목록을 반환합니다.
    /// </summary>
    public static List<Monkey> GetMonkeys() => monkeys;

    /// <summary>
    /// 이름으로 원숭이를 찾습니다.
    /// </summary>
    public static Monkey? GetMonkeyByName(string name) => monkeys.FirstOrDefault(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// 랜덤 원숭이를 반환하고, 호출 횟수를 추적합니다.
    /// </summary>
    public static Monkey? GetRandomMonkey()
    {
        if (monkeys.Count == 0) return null;
        randomPickCount++;
        var rand = new Random();
        return monkeys[rand.Next(monkeys.Count)];
    }

    /// <summary>
    /// 랜덤 원숭이 선택 횟수를 반환합니다.
    /// </summary>
    public static int GetRandomPickCount() => randomPickCount;
}
