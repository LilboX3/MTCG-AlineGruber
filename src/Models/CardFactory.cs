using NUnit.Framework.Constraints;

namespace MTCG.Models;

public class CardFactory
{
    public float Damage { get; set; }
    public string Name { get; set; }
    public string Id { get; set; }

    public Card Create()
    {
        Element element;
        //Find right element
        if (Name.Contains("Water"))
        {
            element = Element.Water;
        } else if (Name.Contains("Fire"))
        {
            element = Element.Fire;
        }
        else
        {
            element = Element.Regular;
        }
        //create spellcard
        if (Name.Contains("Spell"))
        {
            return new SpellCard(Damage, Id, element);
        }
        //else create MonsterCard
        Monster monster = GetMonsterType();
        return new MonsterCard(monster, Damage, Id, element);
    }

    private Monster GetMonsterType()
    {
        if (Name.Contains("Goblin"))
        {
            return Monster.Goblin;
        } 
        if (Name.Contains("Dragon"))
        {
            return Monster.Dragon;
        } 
        if (Name.Contains("Ork"))
        {
            return Monster.Ork;
        }
        if (Name.Contains("Elf"))
        {
            return Monster.Ork;
        }
        if (Name.Contains("Troll"))
        {
            return Monster.Troll;
        }
        if (Name.Contains("Knight"))
        {
            return Monster.Knight;
        }

        if (Name.Contains("Wizzard"))
        {
            return Monster.Wizzard;
        }

        return Monster.Kraken;
        


    }
}