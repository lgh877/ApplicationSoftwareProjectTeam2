using ApplicationSoftwareProjectTeam2.network;
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("서버 시작 중...");
        Server server = new Server();
        server.AddTestPlayers(); // Player1, Player2 등록
        server.Start(5555);      // 포트 5555로 수신 시작
        Console.WriteLine("서버 실행 중. 종료하려면 아무 키나 누르세요.");
        Console.ReadKey();
    }
}
