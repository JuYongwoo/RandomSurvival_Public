using JYW.RandomSurvival.Commmons;

public class Gate : EnemyBase
{

    protected override float hp { get; set; } = 20;
    public override float power { get; set; } = 0;
    public override int EXP { get; set; } = 0;
    public override EnemyType enemyType { get; set; } = EnemyType.NonAttack;

}
