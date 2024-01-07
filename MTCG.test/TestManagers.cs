using MTCG.Business_Layer;
using MTCG.Data_Layer;

namespace MTCG.test;

[TestFixture]
public class TestManagers
{
    [Test]
    public void TestBattleManagerNotNull()
    {
        BattleManager battleManager = new BattleManager(new UserDao(), new DeckDao());
        Assert.That(battleManager, Is.Not.Null);
    }

    [Test]
    public void TestCardManagerNotNull()
    {
        CardManager cardManager = new CardManager(new CardDao());
        Assert.That(cardManager, Is.Not.Null);
    }
    
    [Test]
    public void TestPackageManagerNotNull()
    {
        PackageManager packageManager = new PackageManager(new PackageDao(), new CardDao(), new UserDao());
        Assert.That(packageManager, Is.Not.Null);
    }

    [Test]
    public void TestScoreboardManagerNotNull()
    {
        ScoreboardManager scoreboardManager = new ScoreboardManager(new ScoreboardDao());
        Assert.That(scoreboardManager, Is.Not.Null);
    }

    [Test]
    public void TestTradeManagerNotNull()
    {
        TradeManager tradeManager = new TradeManager(new TradeDao());
        Assert.That(tradeManager, Is.Not.Null);
    }

    [Test]
    public void TestUserManagerNotNull()
    {
        UserManager userManager = new UserManager(new UserDao());
        Assert.That(userManager, Is.Not.Null);
    }
}