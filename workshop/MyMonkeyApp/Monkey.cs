namespace MyMonkeyApp;

/// <summary>
/// 원숭이 정보를 나타내는 모델 클래스입니다.
/// </summary>
public class Monkey
{
    /// <summary>
    /// 원숭이 이름
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 서식지 정보
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// 상세 설명
    /// </summary>
    public string Details { get; set; }

    /// <summary>
    /// 이미지 URL
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// 개체수
    /// </summary>
    public int Population { get; set; }

    /// <summary>
    /// 위도
    /// </summary>
    public double Latitude { get; set; }

    /// <summary>
    /// 경도
    /// </summary>
    public double Longitude { get; set; }
}
